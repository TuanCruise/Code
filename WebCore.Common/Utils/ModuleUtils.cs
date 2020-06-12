using System.Collections.Generic;
using System.Linq;
using WebCore.Common;
using WebCore.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Reflection;

namespace WebCore.Utils
{
    public static class ModuleUtils
    {
        private static Dictionary<string, string> m_ExtraProfileProperty = new Dictionary<string, string>();
        public static ModuleInfo NewCopyModule(ModuleInfo modInfo)
        {
            var type = modInfo.GetType();
            var newObject = (ModuleInfo)type.GetConstructor(new Type[0]).Invoke(new object[0]);
            foreach (var prop in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                prop.SetValue(newObject, prop.GetValue(modInfo, new object[0]), new object[0]);
            }
            return newObject;
        }
        #region GetModuleInfo
        public static ModuleInfo GetModuleInfo(string moduleID)
        {
            var module = (from item in AllCaches.ModulesInfo
                          where
                              item.ModuleID == moduleID &&
                              ((item.SubModule == CODES.DEFMOD.SUBMOD.MAINTAIN_ADD) ||
                               (item.SubModule == CODES.DEFMOD.SUBMOD.MODULE_MAIN))
                          select item).ToList();

            if (module.Count == 0)
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_NOT_FOUND, moduleID);

            if (module.Count > 1)
                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_HAVE_TO_CALL_SUB, moduleID);

            return NewCopyModule(module[0]);
        }

        public static ModuleInfo GetModuleInfo(string moduleID, string subModule)
        {
            var module = (from item in AllCaches.ModulesInfo
                           where item.ModuleID == moduleID && (string.IsNullOrEmpty(subModule) || subModule == item.SubModule)
                           select item).ToList();

            if (module.Count == 0)
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_NOT_FOUND, moduleID + "." + subModule);
            
            if (module.Count > 1)
                throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_MODULE_HAVE_TO_CALL_SUB, moduleID + "." + subModule);

            return NewCopyModule(module[0]);
        }

//#if DEBUG
        public static void ForceLoad(string moduleID, 
            List<ModuleInfo> moduleInfos,
            List<ModuleFieldInfo> fieldInfos,
            List<ButtonInfo> buttonsInfo,
            List<ButtonParamInfo> buttonParamsInfo,
            List<LanguageInfo> languageInfo,
            List<OracleParam> oracleParamsInfo)
        {
            AllCaches.ModulesInfo = (
                from item in AllCaches.ModulesInfo
                where item.ModuleID != moduleID
                select item).ToList();
            AllCaches.ModulesInfo.AddRange(moduleInfos);

            AllCaches.ModuleFieldsInfo = (
                from item in AllCaches.ModuleFieldsInfo
                where item.ModuleID != moduleID
                select item).ToList();
            AllCaches.ModuleFieldsInfo.AddRange(fieldInfos);

            if (AllCaches.SearchButtonsInfo != null)
            {
                AllCaches.SearchButtonsInfo = (
                    from item in AllCaches.SearchButtonsInfo
                    where item.ModuleID != moduleID
                    select item).ToList();
                AllCaches.SearchButtonsInfo.AddRange(buttonsInfo);                
            }

            if (AllCaches.SearchButtonParamsInfo != null)
            {
                AllCaches.SearchButtonParamsInfo = (
                    from item in AllCaches.SearchButtonParamsInfo
                    where item.ModuleID != moduleID
                    select item).ToList();
                AllCaches.SearchButtonParamsInfo.AddRange(buttonParamsInfo);
            }

            if (AllCaches.LanguageInfo != null)
            {
                AllCaches.BaseLanguageInfo = (
                    from item in AllCaches.BaseLanguageInfo
                    where languageInfo.Count(lang => lang.LanguageName == item.LanguageName) == 0
                    select item).ToList();
                AllCaches.BaseLanguageInfo.AddRange(languageInfo);

                AllCaches.LanguageInfo = AllCaches.BaseLanguageInfo.ToDictionary(
                    item => item.LanguageName,
                    item => string.IsNullOrEmpty(item.LanguageValue) ? item.LargerLanguageValue : item.LanguageValue);
            }

            if (AllCaches.OracleParamsInfo != null)
            {
                AllCaches.OracleParamsInfo = (
                    from item in AllCaches.OracleParamsInfo
                    where oracleParamsInfo.Count(p => p.StoreName == item.StoreName) == 0
                    select item).ToList();
                AllCaches.OracleParamsInfo.AddRange(oracleParamsInfo);
            }
        }

//#endif
        #endregion

        public static List<ButtonInfo> GetSearchButtons(string moduleID)
        {
            return (from button in AllCaches.SearchButtonsInfo
                    where button.ModuleID == moduleID
                    select button).ToList();
        }

        public static List<ButtonParamInfo> GetSearchButtonParams(ButtonInfo button)
        {
            return (from param in AllCaches.SearchButtonParamsInfo
                    where button.ModuleID == param.ModuleID && button.ButtonName == param.ButtonName
                    select param).ToList();
        }

        public static List<OracleParam> GetOracleParams(string storeName)
        {
            return (from param in AllCaches.OracleParamsInfo
                    where
                        param.StoreName.ToUpper() == storeName.ToUpper() &&
                        param.Name != CONSTANTS.ORACLE_SESSION_USER
                    select param).ToList();
        }

        public static string BuildSearchConditionKey(ModuleInfo moduleInfo, SearchConditionInstance conditionIntance)
        {
            if (conditionIntance.SQLLogic != null)
            {
                var conditions = new List<string>();
                foreach (var pCondition in conditionIntance.SubCondition)
                {
                    var strCondition = BuildSearchConditionKey(moduleInfo, pCondition);
                    if (!string.IsNullOrEmpty(strCondition)) conditions.Add(strCondition);
                }

                if (conditions.Count == 0)
                    return string.Empty;
                
                switch (conditionIntance.SQLLogic)
                {
                    case CODES.SQL_EXPRESSION.SQL_LOGIC.OR:
                        return "(" +  string.Join(" OR ", conditions.ToArray()) + ")";
                    default:
                        return "(" + string.Join(" AND ", conditions.ToArray()) + ")";
                }
            }

            var condition = FieldUtils.GetModuleFieldByID(moduleInfo.ModuleID, conditionIntance.ConditionID);
            string conditionFormat;
            switch (conditionIntance.Operator)
            {
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LIKE:
                    conditionFormat = "{0} LIKE '%{1}%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTLIKE:
                    conditionFormat = "{0} NOT LIKE '%{1}%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTEQUAL:
                    conditionFormat = "{0} <> {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.BEGINWITH:
                    conditionFormat = "{0} LIKE '{1}%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.ENDWITH:
                    conditionFormat = "{0} LIKE '%{1}'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LARGER:
                    conditionFormat = "{0} > {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LARGEROREQUAL:
                    conditionFormat = "{0} >= {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLER:
                    conditionFormat = "{0} < {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLEROREQUAL:
                    conditionFormat = "{0} <= {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.INARRAY:
                    conditionFormat = "{0} IN ({1})";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTINARRAY:
                    conditionFormat = "{0} NOT IN ({1})";
                    break;
                default:
                    conditionFormat = "{0}={1}";
                    break;
            }

            string fieldName;
            //if (App.Environment.EnvironmentType == EnvironmentType.CLIENT_APPLICATION)
            //    fieldName = string.Format("[{0}]",
            //                              LangUtils.Translate(
            //                                  LangType.LABEL_FIELD,
            //                                  moduleInfo.ModuleName,
            //                                  condition.FieldName));
            //else
                fieldName = condition.ParameterName;

            if (conditionIntance.Value == null)
            {
                return string.Format(conditionFormat, fieldName, null);
            }

            return string.Format(conditionFormat, fieldName, conditionIntance.Value);
        }

        //add by TrungTT - 8.12.2011 - Customize Column of Grid - Source by LongND5
        public static string GetProfileExtraProperty(ModuleInfo moduleInfo, string extraProperty)
        {
            return moduleInfo.ModuleName + "." + extraProperty;
        }
        public static bool IsProfileExtraPropertyExisted(ModuleInfo moduleInfo, string extraProperty)
        {
            return m_ExtraProfileProperty.ContainsKey(GetProfileExtraProperty(moduleInfo, extraProperty));
        }
        public static void SetExtraProfile(ModuleInfo moduleInfo, string extraProperty, string extraValue)
        {
            m_ExtraProfileProperty[GetProfileExtraProperty(moduleInfo, extraProperty)] = extraValue;
        }
        public static string GetProfileExtraValue(ModuleInfo moduleInfo, string extraProperty)
        {
            return m_ExtraProfileProperty[GetProfileExtraProperty(moduleInfo, extraProperty)];
        }
        //End TrungTT

        public static string BuildSearchCondition(ModuleInfo moduleInfo, ref string whereExtension, OracleCommand comm, SearchConditionInstance conditionIntance)
        {
            // Parse And/Or Group Condition
            if (conditionIntance.SQLLogic != null)
            {
                var conditions = new List<string>();
                foreach (var pCondition in conditionIntance.SubCondition)
                {
                    var strCondition = BuildSearchCondition(moduleInfo, ref whereExtension, comm, pCondition);
                    if (!string.IsNullOrEmpty(strCondition)) conditions.Add(strCondition);
                }

                if (conditions.Count == 0)
                    return string.Empty;
                
                switch (conditionIntance.SQLLogic)
                {
                    case CODES.SQL_EXPRESSION.SQL_LOGIC.OR:
                        return "(" + string.Join(" OR ", conditions.ToArray()) + ")";
                    default:
                        return "(" + string.Join(" AND ", conditions.ToArray()) + ")";
                }
            }

            var condition = FieldUtils.GetModuleFieldByID(moduleInfo.ModuleID, conditionIntance.ConditionID);

            string conditionFormat;
            switch (conditionIntance.Operator)
            {
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LIKE:
                    conditionFormat = "{0} LIKE '%' || {1} || '%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTLIKE:
                    conditionFormat = "{0} NOT LIKE '%' || {1} || '%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTEQUAL:
                    conditionFormat = "{0} <> {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.BEGINWITH:
                    conditionFormat = "{0} LIKE {1} || '%'";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.ENDWITH:
                    conditionFormat = "{0} LIKE '%' || {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LARGER:
                    conditionFormat = "{0} > {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.LARGEROREQUAL:
                    conditionFormat = "{0} >= {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLER:
                    conditionFormat = "{0} < {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLEROREQUAL:
                    conditionFormat = "{0} <= {1}";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.INARRAY:
                    conditionFormat = "{0} IN ({1})";
                    break;
                case CODES.DEFMODFLD.CONDITION_OPERATOR.NOTINARRAY:
                    conditionFormat = "{0} NOT IN ({1})";
                    break;
                default:
                    conditionFormat = "{0} = {1}";
                    break;
            }

            // Build Left Operator
            var leftOperator = condition.ParameterName;
            switch(condition.TextCase)
            {
                case CODES.DEFMODFLD.TEXTCASE.UPPER:
                    leftOperator = string.Format("UPPER({0})", condition.ParameterName);
                    break;
                case CODES.DEFMODFLD.TEXTCASE.LOWER:
                    leftOperator = string.Format("LOWER({0})", condition.ParameterName);
                    break;
            }

            string rightOperator;
            if (string.IsNullOrEmpty(conditionIntance.Value))
            {
                var parameterName = ":" + (comm.Parameters.Count + 1);
                comm.Parameters.Add(parameterName, DBNull.Value);

                rightOperator = parameterName;
            }
            else if (
                conditionIntance.Operator == CODES.DEFMODFLD.CONDITION_OPERATOR.INARRAY ||
                conditionIntance.Operator == CODES.DEFMODFLD.CONDITION_OPERATOR.NOTINARRAY)
            {
                var values = conditionIntance.Value.Split(new[] { "," }, StringSplitOptions.None);
                var parameterNames = new string[values.Length];

                for (var i = 0; i < values.Length; i++)
                {
                    var paramName = ":" + (comm.Parameters.Count + 1);
                    parameterNames[i] = paramName;
                    comm.Parameters.Add(paramName, values[i].Decode(condition));
                }
                    
                rightOperator = string.Join(",", parameterNames);

            }
            else
            {
                var paramName = ":" + (comm.Parameters.Count + 1);

                switch (condition.TextCase)
                {
                    case CODES.DEFMODFLD.TEXTCASE.UPPER:
                        comm.Parameters.Add(paramName, conditionIntance.Value.ToUpper());
                        break;
                    case CODES.DEFMODFLD.TEXTCASE.LOWER:
                        comm.Parameters.Add(paramName, conditionIntance.Value.ToLower());
                        break;
                    default:
                        comm.Parameters.Add(paramName, conditionIntance.Value.Decode(condition));
                        break;
                }

                rightOperator = paramName;
            }

            if(!string.IsNullOrEmpty(condition.WhereExtension))
            {
                whereExtension = whereExtension.Replace("{" + condition.ParameterName + ":RIGHT}", string.Format(conditionFormat, "", rightOperator));
                whereExtension = whereExtension.Replace("{" + condition.ParameterName + ":VALUE}", rightOperator);
            }

            if(!string.IsNullOrEmpty(condition.CustomSearchCondition))
            {
                var customSearchInstance = condition.CustomSearchCondition;
                customSearchInstance = customSearchInstance.Replace("{" + condition.ParameterName + ":RIGHT}", string.Format(conditionFormat, "", rightOperator));
                customSearchInstance = customSearchInstance.Replace("{" + condition.ParameterName + ":VALUE}", rightOperator);
                return customSearchInstance;
            }

            return string.Format(conditionFormat, leftOperator, rightOperator);
        }
    }
}
