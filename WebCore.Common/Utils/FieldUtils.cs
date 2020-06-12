using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using WebCore.Common;
using WebCore.Entities;

namespace WebCore.Utils
{
    public static class FieldUtils
    {
        #region GetModuleField Functions
        public static List<ModuleFieldInfo> GetModuleFields(string moduleID)
        {
            return GetModuleFields(moduleID, null);
        }

        public static List<ModuleFieldInfo> GetModuleFields(string moduleID, string fieldGroup)
        {
            if (AllCaches.ModuleFieldsInfo == null)
                return new List<ModuleFieldInfo>();
            return (from field in AllCaches.ModuleFieldsInfo
                    where field.ModuleID == moduleID && (field.FieldGroup == fieldGroup || fieldGroup == null)
                    select field).ToList();
        }

        public static ModuleFieldInfo GetModuleFieldByID(string moduleID, string fieldID)
        {
            try
            {
                return (from field in AllCaches.ModuleFieldsInfo
                        where (field.ModuleID == moduleID) && field.FieldID == fieldID
                        select field).Single();
            }          
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE, ex.Message,
                    "GetModuleFieldByID", moduleID, fieldID);
            }
        }

        public static ModuleFieldInfo GetModuleFieldByModule(ModuleInfo modInfo, string fieldID)
        {
            try
            {
                return (from field in AllCaches.ModuleFieldsInfo
                        where (field.ModuleID == modInfo.ModuleID || field.ModuleID == modInfo.ModuleType) && field.FieldID == fieldID
                        select field).Single();
            }          
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE, ex.Message,
                    "GetModuleFieldByID", modInfo.ModuleID, fieldID);
            }
        }

        public static List<ModuleFieldInfo> GetModuleFieldsByName(string moduleID, string fieldName)
        {
            return GetModuleFieldsByName(moduleID, null, fieldName);
        }

        public static List<ModuleFieldInfo> GetModuleFieldsByName(string moduleID, string fieldGroup, string fieldName)
        {
            return (from field in AllCaches.ModuleFieldsInfo
                    where field.ModuleID == moduleID && (field.FieldGroup == fieldGroup || fieldGroup == null) && field.FieldName == fieldName
                    select field).ToList();
        }

        public static ModuleFieldInfo GetModuleFieldByName(string moduleID, string fieldGroup, string fieldName)
        {            
            var fieldInfos =  (from field in AllCaches.ModuleFieldsInfo
                    where field.ModuleID == moduleID && field.FieldGroup == fieldGroup && field.FieldName == fieldName
                    select field).ToArray();

            if (fieldInfos.Length == 0) throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_FIELD_NOT_FOUND, fieldName);
            if (fieldInfos.Length > 1) throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_FIELD_DUPLICATED, fieldName);

            return fieldInfos[0];
        }

#if DEBUG
        public static void ForceLoad(string moduleID)
        {
        }
#endif
        #endregion
        public static object ConvertExtract(ModuleFieldInfo fieldInfo, object value)
        {
            switch (fieldInfo.FieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.INT32:
                    return System.Convert.ToInt32(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.INT64:
                    return System.Convert.ToInt64(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    return System.Convert.ToSingle(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    return System.Convert.ToDouble(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DATE:
                    try
                    {
                        var dblValue = System.Convert.ToDouble(value, App.Environment.ClientInfo.Culture);
                        return DateTime.Now.AddDays(dblValue);
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(fieldInfo.FieldFormat))
                            return DateTime.Parse(string.Format(App.Environment.ClientInfo.Culture, "{0}", value), App.Environment.ClientInfo.Culture);
                        return DateTime.ParseExact(string.Format(App.Environment.ClientInfo.Culture, "{0}", value), fieldInfo.FieldFormat, App.Environment.ClientInfo.Culture);
                    }
                default:
                    return value;
            }
        }

        public static object Convert(ModuleFieldInfo fieldInfo, object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;

            switch (fieldInfo.FieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.INT32:
                    return System.Convert.ToInt32(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.INT64:
                    return System.Convert.ToInt64(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    return System.Convert.ToSingle(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    return System.Convert.ToDouble(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    return System.Convert.ToDecimal(value, App.Environment.ClientInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DATE:
                case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                    try
                    {
                        var dblValue = System.Convert.ToDouble(value, App.Environment.ClientInfo.Culture);
                        return DateTime.Now.AddDays(dblValue);
                    }
                    catch
                    {
                        return DateTime.Parse(string.Format(App.Environment.ClientInfo.Culture, "{0}", value), App.Environment.ClientInfo.Culture);
                    }
                default:
                    return value;
            }
        }

        public static object ConvertImport(ModuleFieldInfo fieldInfo, object value, string fieldName)
        {
            try
            {
                if (fieldInfo.Nullable == CODES.DEFMODFLD.NULLABLE.YES &&
                    (value == null || string.IsNullOrEmpty(value.ToString().Trim()))) return null;

                switch (fieldInfo.ImportType)
                {
                    case CODES.DEFMODFLD.FLDTYPE.INT32:
                        return System.Convert.ToInt32(value, App.Environment.ClientInfo.Culture);
                    case CODES.DEFMODFLD.FLDTYPE.INT64:
                        return System.Convert.ToInt64(value, App.Environment.ClientInfo.Culture);
                    case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                        return System.Convert.ToSingle(value, App.Environment.ClientInfo.Culture);
                    case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                        return System.Convert.ToDouble(value, App.Environment.ClientInfo.Culture);
                    case CODES.DEFMODFLD.FLDTYPE.DATE:
                        if(string.IsNullOrEmpty(fieldInfo.FieldFormat))
                            return DateTime.ParseExact(value.ToString(), CONSTANTS.DEFAULT_DATETIME_FORMAT, null);
                        return DateTime.ParseExact(value.ToString(), fieldInfo.FieldFormat, null);
                    default:
                        return value;
                }
            }
            catch
            {
                //var errorFormat = LangUtils.TranslateBasic("Convert column {0} failed", "IMPORT_DATA_CONVERT_FAIL");
                //throw ErrorUtils.CreateErrorWithSubMessage(ERR_IMPORT.ERR_CONVERT_IMPORT_DATA_FAIL,
                //    string.Format(errorFormat, fieldName));
                return null;
            }
        }

        public static Type GetType(string fieldType)
        {
            switch (fieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.INT32:
                    return typeof(int);
                case CODES.DEFMODFLD.FLDTYPE.INT64:
                    return typeof(long);
                case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    return typeof(float);
                case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    return typeof(double);
                case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    return typeof(decimal);
                case CODES.DEFMODFLD.FLDTYPE.DATE:
                case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                    return typeof(DateTime);
                default:
                    return typeof(string);
            }
        }

        public static ValidateInfo GetValidateInfo(string validateName)
        {
            return AllCaches.ValidatesInfo[validateName];
        }

        public static string GetValidateName(ModuleInfo moduleInfo, ModuleFieldInfo fieldInfo)
        {
            switch (moduleInfo.SubModule)
            {
                case CODES.DEFMOD.SUBMOD.MAINTAIN_ADD:
                    if (!string.IsNullOrEmpty(fieldInfo.ValidateRuleOnAdd))
                    {
                        return fieldInfo.ValidateRuleOnAdd;
                    }
                    break;
                case CODES.DEFMOD.SUBMOD.MAINTAIN_EDIT:
                    if (!string.IsNullOrEmpty(fieldInfo.ValidateRuleOnEdit))
                    {
                        return fieldInfo.ValidateRuleOnEdit;
                    }
                    break;
            }

            return fieldInfo.ValidateRule;
        }

        public static List<GroupSummaryInfo> GetModuleGroupSummary(string moduleID, string moduleType)
        {
            return (from field in AllCaches.GroupSummaryInfos
                    where field.ModuleID == moduleID
                    select field).ToList();
        }
    }

    public static class ValueEncoding
    {
        public static string Encode(this object value, ModuleFieldInfo fieldInfo)
        {
            if (fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.CHECKEDCOMBOBOX ||
               fieldInfo.ControlType == CODES.DEFMODFLD.CTRLTYPE.LOOKUPVALUES)
            {
                var regex = new Regex("[^,<]+<(?<VALUE>[^>]+)>|(?<VALUE>[^,<]+)[,]");
                var matches = regex.Matches(value + ",");

                switch (fieldInfo.TextCase)
                {
                    case CODES.DEFMODFLD.TEXTCASE.UPPER:
                        return string.Join(",",
                            (from Match match in matches
                             select match.Groups["VALUE"].Value.Trim()).ToArray()
                        ).ToUpper();
                    case CODES.DEFMODFLD.TEXTCASE.LOWER:
                        return string.Join(",",
                            (from Match match in matches
                             select match.Groups["VALUE"].Value.Trim()).ToArray()
                        ).ToLower();
                    default:
                        return string.Join(",",
                            (from Match match in matches
                             select match.Groups["VALUE"].Value.Trim()).ToArray()
                        );
                } 
            }

            if (value == null && fieldInfo.Nullable == CODES.DEFMODFLD.NULLABLE.YES)
                return null;

            switch (fieldInfo.FieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.DATE:                    
                    return string.Format(App.Environment.ServerInfo.Culture, "{0:d}", value);                    
                default:
                    if (value == null) return null;

                    switch (fieldInfo.TextCase)
                    {
                        case CODES.DEFMODFLD.TEXTCASE.UPPER:
                            return string.Format(App.Environment.ServerInfo.Culture, "{0}", value).ToUpper();
                        case CODES.DEFMODFLD.TEXTCASE.LOWER:
                            return string.Format(App.Environment.ServerInfo.Culture, "{0}", value).ToLower();
                        default:
                            return string.Format(App.Environment.ServerInfo.Culture, "{0}", value);
                    }
            }
        }

        public static object DecodeAny(this object value, ModuleFieldInfo fieldInfo)
        {
            // If value is null or string.Empty return null
            if (value == null || value == DBNull.Value || (value is string && ((string)value) == string.Empty))
                return null;
            
            // If type of value same as typeof field return value
            if (value.GetType() == FieldUtils.GetType(fieldInfo.FieldType))
                return value;

            // If value is string, correct typeof value
            if(value is string && !string.IsNullOrEmpty(fieldInfo.FieldFormat))
            {
                return Decode((string) value, fieldInfo);
            }
            
            // Other Convert Value
            switch (fieldInfo.FieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.INT32:
                    return Convert.ToInt32(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.INT64:
                    return Convert.ToInt64(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    return Convert.ToSingle(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    return Convert.ToDouble(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    return Convert.ToDecimal(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DATE:
                case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                    return Convert.ToDateTime(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.STRING:
                    return Convert.ToString(value);
                default:
                    return value;
            }
        }

        public static object Decode(this string value, ModuleFieldInfo fieldInfo)
        {
            if (value == null || string.IsNullOrEmpty(value))
                return null;

            switch (fieldInfo.FieldType)
            {
                case CODES.DEFMODFLD.FLDTYPE.INT32:
                    return Convert.ToInt32(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.INT64:
                    return Convert.ToInt64(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.FLOAT:
                    return Convert.ToSingle(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DOUBLE:
                    return Convert.ToDouble(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DECIMAL:
                    return Convert.ToDecimal(value, App.Environment.ServerInfo.Culture);
                case CODES.DEFMODFLD.FLDTYPE.DATE:
                case CODES.DEFMODFLD.FLDTYPE.DATETIME:
                        try
                        {
                            return DateTime.Now.Date.AddDays(double.Parse(value));
                        }
                        catch
                        {
                            if (!string.IsNullOrEmpty(fieldInfo.FieldFormat))
                            {
                                return DateTime.ParseExact(value, fieldInfo.FieldFormat, App.Environment.ServerInfo.Culture);
                            }

                            return DateTime.Parse(value, App.Environment.ServerInfo.Culture);
                        }
                case CODES.DEFMODFLD.FLDTYPE.STRING:
                        // Set cho truong hop doi voi thang Defaulvalue
                        if (value == "YYYY" && !string.IsNullOrEmpty(fieldInfo.DefaultValue))
                        { 
                            value = DateTime.Now.Year.ToString();
                        }
                        return value;
                default:
                    return value;
            }
        }
    }
}
