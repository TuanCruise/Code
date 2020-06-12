using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess;
using Npgsql;
using NpgsqlTypes;
using WebCore.Common;
using WebCore.Entities;
using WebCore.Utils;

namespace WebApiCore.Common
{
    public class BuildQuerySearch
    {
        public BuildQuerySearch(string connectionString)
        {
            ConnectionString = connectionString;
        }
        private string ConnectionString = "";
        public void BuildStaticConditions(SearchModuleInfo searchInfo, NpgsqlCommand comm, List<SearchConditionInstance> staticConditionInstances)
        {
            var fields = FieldUtils.GetModuleFields(searchInfo.ModuleID, WebCore.CODES.DEFMODFLD.FLDGROUP.PARAMETER);
            if (fields==null || fields.Count==0)
            {
                fields = PostgresqlHelper.ExecuteStoreProcedurePostgreSQL<ModuleFieldInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_FIELD_INFO_BY_MODID, searchInfo.ModuleID);
                if (AllCaches.ModuleFieldsInfo==null)
                {
                    AllCaches.ModuleFieldsInfo = new List<ModuleFieldInfo>();
                }
                AllCaches.ModuleFieldsInfo.AddRange(fields);
            }
            foreach (var field in fields)
            {
                switch (field.FieldName)
                {
                    case CONSTANTS.ORACLE_SESSION_USER:
                        //comm.Parameters.Add(":" + field.ParameterName, Session.Username);
                        break;
                    case CONSTANTS.ORACLE_CURSOR_OUTPUT:
                        //comm.Parameters.Add(new OracleParameter(":" + field.ParameterName, OracleDbType.RefCursor))
                        //    .Direction = ParameterDirection.Output;
                        break;
                }
            }
            var parameter = new NpgsqlParameter();
            fields = FieldUtils.GetModuleFields(searchInfo.ModuleID, WebCore.CODES.DEFMODFLD.FLDGROUP.SEARCH_CONDITION);
            foreach (var condition in staticConditionInstances)
            {
                foreach (var field in fields)
                {
                    if (field.FieldID == condition.ConditionID)
                    {
                        if (string.IsNullOrEmpty(condition.Value))
                        {
                            parameter.ParameterName = field.ParameterName;
                            parameter.NpgsqlDbType = NpgsqlDbType.Text;
                            parameter.Value = DBNull.Value;
                            comm.Parameters.Add(parameter);
                            //comm.Parameters.Add(":" + field.ParameterName, DBNull.Value);
                        }
                        else
                        {
                            parameter.ParameterName = ":" + field.ParameterName;
                            parameter.NpgsqlDbType = NpgsqlDbType.Text;
                            parameter.Value = condition.Value.Decode(field);
                            comm.Parameters.Add(parameter);
                            //comm.Parameters.Add(":" + field.ParameterName, condition.Value.Decode(field));
                        }
                    }
                }
            }
        }

        private void DiscoveryParametersForSearch(NpgsqlCommand command, SearchModuleInfo searchInfo, string queryFormat, SearchConditionInstance conditionIntance, List<SearchConditionInstance> staticConditionInstances)
        {
            //command.BindByName = true;

            var whereExtension = "1 = 1";
            if (!string.IsNullOrEmpty(searchInfo.WhereExtension))
            {
                whereExtension = BuildSearchExtension(searchInfo, conditionIntance);
            }

            BuildStaticConditions(searchInfo, command, staticConditionInstances);

            var whereCondition = "";// ModuleUtils.BuildSearchCondition(searchInfo, ref whereExtension, command, conditionIntance);
            if (string.IsNullOrEmpty(whereCondition)) whereCondition = "1 = 1";

            command.CommandText = string.Format(queryFormat, whereCondition, whereExtension);
            if (searchInfo.ModuleID == STATICMODULE.UPFILE_MODID)
            {
                //command.CommandText = queryFormat + " and sessionskey = '" + Session.SessionKey + "'";
            }

        }

        public static string BuildSearchCondition(ModuleInfo moduleInfo, ref string whereExtension, NpgsqlCommand comm, SearchConditionInstance conditionIntance)
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
                    case WebCore.CODES.SQL_EXPRESSION.SQL_LOGIC.OR:
                        return "(" + string.Join(" OR ", conditions.ToArray()) + ")";
                    default:
                        return "(" + string.Join(" AND ", conditions.ToArray()) + ")";
                }
            }

            var condition = FieldUtils.GetModuleFieldByID(moduleInfo.ModuleID, conditionIntance.ConditionID);

            string conditionFormat;
            switch (conditionIntance.Operator)
            {
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.LIKE:
                    conditionFormat = "{0} LIKE '%' || {1} || '%'";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.NOTLIKE:
                    conditionFormat = "{0} NOT LIKE '%' || {1} || '%'";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.NOTEQUAL:
                    conditionFormat = "{0} <> {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.BEGINWITH:
                    conditionFormat = "{0} LIKE {1} || '%'";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.ENDWITH:
                    conditionFormat = "{0} LIKE '%' || {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.LARGER:
                    conditionFormat = "{0} > {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.LARGEROREQUAL:
                    conditionFormat = "{0} >= {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLER:
                    conditionFormat = "{0} < {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.SMALLEROREQUAL:
                    conditionFormat = "{0} <= {1}";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.INARRAY:
                    conditionFormat = "{0} IN ({1})";
                    break;
                case WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.NOTINARRAY:
                    conditionFormat = "{0} NOT IN ({1})";
                    break;
                default:
                    conditionFormat = "{0} = {1}";
                    break;
            }

            // Build Left Operator
            var leftOperator = condition.ParameterName;
            switch (condition.TextCase)
            {
                case WebCore.CODES.DEFMODFLD.TEXTCASE.UPPER:
                    leftOperator = string.Format("UPPER({0})", condition.ParameterName);
                    break;
                case WebCore.CODES.DEFMODFLD.TEXTCASE.LOWER:
                    leftOperator = string.Format("LOWER({0})", condition.ParameterName);
                    break;
            }

            string rightOperator;
            if (string.IsNullOrEmpty(conditionIntance.Value))
            {
                var parameter = new NpgsqlParameter();
                parameter.ParameterName = ":" + (comm.Parameters.Count + 1);
                parameter.Value = DBNull.Value;
                comm.Parameters.Add(parameter);
                //comm.Parameters.Add(parameterName, DBNull.Value);
                rightOperator = parameter.ParameterName;
            }
            else if (
                conditionIntance.Operator == WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.INARRAY ||
                conditionIntance.Operator == WebCore.CODES.DEFMODFLD.CONDITION_OPERATOR.NOTINARRAY)
            {
                var values = conditionIntance.Value.Split(new[] { "," }, StringSplitOptions.None);
                var parameterNames = new string[values.Length];

                for (var i = 0; i < values.Length; i++)
                {
                    var parameter = new NpgsqlParameter();
                    parameter.ParameterName = ":" + (comm.Parameters.Count + 1);
                    parameterNames[i] = parameter.ParameterName;
                    parameter.Value = values[i].Decode(condition);
                    comm.Parameters.Add(parameter);

                    //var paramName = ":" + (comm.Parameters.Count + 1);
                    //parameterNames[i] = paramName;
                    //comm.Parameters.Add(paramName, values[i].Decode(condition));
                }

                rightOperator = string.Join(",", parameterNames);

            }
            else
            {
                var paramName = ":" + (comm.Parameters.Count + 1);
                var parameter = new NpgsqlParameter();
                parameter.ParameterName = ":" + (comm.Parameters.Count + 1);
                switch (condition.TextCase)
                {
                    case WebCore.CODES.DEFMODFLD.TEXTCASE.UPPER:
                        parameter.Value = conditionIntance.Value.ToUpper();
                        //comm.Parameters.Add(paramName, conditionIntance.Value.ToUpper());
                        break;
                    case WebCore.CODES.DEFMODFLD.TEXTCASE.LOWER:
                        parameter.Value = conditionIntance.Value.ToLower();
                        //comm.Parameters.Add(paramName, conditionIntance.Value.ToLower());
                        break;
                    default:
                        parameter.Value = conditionIntance.Value.Decode(condition);
                        //comm.Parameters.Add(paramName, conditionIntance.Value.Decode(condition));
                        break;
                }
                comm.Parameters.Add(parameter);

                rightOperator = paramName;
            }

            if (!string.IsNullOrEmpty(condition.WhereExtension))
            {
                whereExtension = whereExtension.Replace("{" + condition.ParameterName + ":RIGHT}", string.Format(conditionFormat, "", rightOperator));
                whereExtension = whereExtension.Replace("{" + condition.ParameterName + ":VALUE}", rightOperator);
            }

            if (!string.IsNullOrEmpty(condition.CustomSearchCondition))
            {
                var customSearchInstance = condition.CustomSearchCondition;
                customSearchInstance = customSearchInstance.Replace("{" + condition.ParameterName + ":RIGHT}", string.Format(conditionFormat, "", rightOperator));
                customSearchInstance = customSearchInstance.Replace("{" + condition.ParameterName + ":VALUE}", rightOperator);
                return customSearchInstance;
            }

            return string.Format(conditionFormat, leftOperator, rightOperator);
        }

        private string BuildSearchExtension(SearchModuleInfo searchInfo, SearchConditionInstance conditionIntance)
        {
            var fields = FieldUtils.GetModuleFields(searchInfo.ModuleID);
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                using (var comm = new NpgsqlCommand(searchInfo.WhereExtension, conn))
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    PostgresqlHelper.DiscoveryParameters(comm);

                    foreach (var field in
                        fields.Where(field => field.WhereExtension == WebCore.CODES.DEFMODFLD.WHEREEXTENSION.YES))
                    {
                        comm.Parameters[field.FieldName].Value = DBNull.Value;
                        foreach (var condition in conditionIntance.SubCondition)
                        {
                            if (condition.ConditionID == field.FieldID && string.IsNullOrEmpty(condition.SQLLogic))
                            {
                                comm.Parameters[field.FieldName].Value = condition.Operator;
                            }
                        }
                    }
                    comm.ExecuteNonQuery();
                    return comm.Parameters["RETURN_VALUE"].Value.ToString();
                }
            }
        }
    }
}
