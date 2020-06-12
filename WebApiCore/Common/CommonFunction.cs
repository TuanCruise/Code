using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebCore.Entities;
using WebModelCore.Common;

namespace WebApiCore
{
    public class CommonFunction
    {
        public void GetSQLServerParameterValues(out List<string> oracleValues, string storeName, List<OracleParam> oracleParams, List<ModuleFieldInfo> fields)
        {
            //List<OracleParam> oracleParams = new List<OracleParam>();
            //if (ModuleInfo.ModuleType == Core.CODES.DEFMOD.MODTYPE.MAINTAINPOS) {
            //if (App.Configs.IsPOS == CONSTANTS.Yes) {
            //    oracleParams = obj.BuildOracleParamsInfo(storeName);
            //    GetSQLServerParameterValues(oracleParams);
            //}
            //else {
            //oracleParams = ModuleUtils.GetOracleParams(storeName);
            GetSQLServerParameterValues(oracleParams, fields);
            //}
            oracleValues = oracleParams.ToListString();
        }


        private void GetSQLServerParameterValues(List<OracleParam> @params, List<ModuleFieldInfo> fields)
        {
            //var fields = GetModuleFields();
            foreach (var param in @params)
            {
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && (param.Name.ToUpper() == "@" + field.ParameterName.ToUpper() || param.Name.ToUpper() == "@" + field.FieldName.ToUpper()))
                    {
                        param.Value = field.Value.Trim(',');
                    }
                }
            }
        }

        public void GetNpgsqlParameterValues(List<SqlParameter> prrs, List<ModuleFieldInfo> fields)
        {
            //var fields = GetModuleFields();
            foreach (var param in prrs)
            {
                //if (param.SqlDbType == NpgsqlDbType.Refcursor)
                //{
                //    param.Value = param.ParameterName;
                //    continue;
                //}
                param.Value = "";
                foreach (var field in fields)
                {
                    if (!string.IsNullOrEmpty(field.ParameterName) && (param.ParameterName.ToUpper() == field.ParameterName.ToUpper() || param.ParameterName.ToUpper() == "@" + field.FieldName.ToUpper()))
                    {
                        switch (param.DbType)
                        {
                            case DbType.Date:
                                param.SqlDbType = SqlDbType.Date;
                                param.DbType = DbType.Date;
                                param.Value = (field.Value ?? "").Trim(',').StringToDateTimeByFomat(field.FieldFormat); // Convert.ToDateTime(field.Value.Trim(','));
                                break;
                            case DbType.Boolean:
                                if (field.Value.Trim(',').ToString() == "1" || field.Value.Trim(',').ToString() == "0")
                                {
                                    param.Value = field.Value.Trim(',').ToString() == "1" ? true : false;
                                }
                                else
                                {
                                    param.Value = Convert.ToBoolean(field.Value.Trim(','));
                                }
                                param.DbType = DbType.Boolean;
                                break;
                            case DbType.Byte:
                                param.Value = Convert.ToByte(string.IsNullOrEmpty(field.Value) ? "0" : field.Value.Trim(','));
                                break;
                            case DbType.Int16:
                                param.Value = Convert.ToInt16(string.IsNullOrEmpty(field.Value) ? "0" : field.Value.Trim(','));
                                param.DbType = DbType.Int16;
                                break;
                            case DbType.Int32:
                                param.Value = Convert.ToInt32(string.IsNullOrEmpty(field.Value) ? "0" : field.Value.Trim(','));
                                param.DbType = DbType.Int32;
                                break;
                            case DbType.Int64:
                                param.Value = Convert.ToInt64(string.IsNullOrEmpty(field.Value) ? "0" : field.Value.Trim(','));
                                param.DbType = DbType.Int64;
                                break;
                            case DbType.Single:
                            case DbType.Double:
                            case DbType.Decimal:
                                param.Value = Convert.ToDecimal(string.IsNullOrEmpty(field.Value) ? "0" : field.Value.Trim(','));
                                param.DbType = DbType.Decimal;
                                break;
                            default:
                                param.Value = (field.Value ?? "").Trim(',');
                                break;
                        }
                        //param.Value = field.Value.Trim(',');
                    }
                }
            }
        }


        public List<ModuleFieldInfo> GetModuleFields()
        {
            var moduleFields = new List<ModuleFieldInfo>();
            //// 1. If module implement IParameterFieldSupportedModule
            //var parameterFieldSupportedModule = this as IParameterFieldSupportedModule;
            //if (parameterFieldSupportedModule != null) {
            //    moduleFields.AddRange(ParameterFields);
            //}

            //// 2. If module implement ICommonFieldSupportedModule
            //var commonFieldSupportedModule = this as ICommonFieldSupportedModule;
            //if (commonFieldSupportedModule != null) {
            //    moduleFields.AddRange(CommonFields);
            //}

            //// 3. If module implement IColumnFieldSupportedModule
            //var columnFieldSupportedModule = this as IColumnFieldSupportedModule;
            //if (columnFieldSupportedModule != null) {
            //    moduleFields.AddRange(ColumnFields);
            //}

            return moduleFields;
        }
        //public List<OracleParam> BuildOracleParamsInfo(string storename)
        //{
        //    try {
        //        //var stores = SQLHelper.ExecuteStoreProcedure<OracleStore>(App.Configs.LocalDB, null, SYSTEM_STORE_PROCEDURES.LIST_STOREPROC);
        //        var oracleParams = new List<OracleParam>();
        //        try {
        //            SQLHelper.DiscoveryParameters(connectionstring, storename, oracleParams);
        //        }
        //        catch {
        //        }
        //        return oracleParams;
        //    }
        //    catch (Exception ex) {
        //        throw ErrorUtils.CreateError(ex);
        //    }
        //}
    }
    public interface IParameterFieldSupportedModule
    {
    }
    public interface ICommonFieldSupportedModule
    {
    }
    public interface IColumnFieldSupportedModule
    {
    }

}
