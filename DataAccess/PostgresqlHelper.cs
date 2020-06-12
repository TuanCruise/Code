using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Npgsql;
using NpgsqlTypes;
using WebCore.Base;
using WebCore.Common;
using WebCore.Utils;

namespace Core.DataAccess
{
    public class PostgresqlHelper
    {

        private NpgsqlConnection connection = null;
        private String connectionString = "dbConnection";
        private NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter();
        public NpgsqlCommand sqlCmd = new NpgsqlCommand();

        // {0} : IP, {1} : DB Name, {2} : userID, {3} : userPW
        private String connectionFormat = "Server={0};Database={1};User Id={2};Password={3};Port={4}";

        public String ConnectionString {
            set { connectionString = value; }
        }
        private static string SchemaStatic = "core";
        private string Schema = "audit";
        #region Constructors

        public PostgresqlHelper()
        {
        }

        public PostgresqlHelper(string pConnectionString)
        {
            if (!string.IsNullOrEmpty(pConnectionString.Trim()))
            {
                connectionString = pConnectionString;
            }
        }

        public PostgresqlHelper(string pIP, string pDbName, string pUserID, string pUserPW, string pPort)
        {
            connectionString = string.Format(this.connectionFormat, pIP, pDbName, pUserID, pUserPW, pPort);
        }

        public PostgresqlHelper(string pIP, string pDbName, string pUserID, string pUserPW)
        {
            connectionString = string.Format(this.connectionFormat, pIP, pDbName, pUserID, pUserPW, "5432");
        }

        #endregion


        /// <summary>
        /// Sql Script / Stored Procedure Name
        /// </summary>
        public string CmdText {
            set { sqlCmd.CommandText = value; }
        }

        /// <summary>
        /// CommandType = CommandType....
        /// </summary>
        public CommandType cmdType {

            set { sqlCmd.CommandType = value; }
        }

        /// <summary>
        /// AddParameter(iSql, "return_value", SqlDbType.Int , "", ParameterDirection.ReturnValue , 5); ---- return문
        /// AddParameter(iSql, "@MSG_TEXT", SqlDbType.VarChar, "", ParameterDirection.Output, 100);
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        /// <param name="Param_io"></param>
        /// <param name="nSize"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values, ParameterDirection Param_io, int nSize)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            if (Param_io == ParameterDirection.Input || Param_io == ParameterDirection.InputOutput)
                param1.Value = values;
            param1.Direction = Param_io;
            param1.Size = nSize;
            sqlCmd.Parameters.Add(param1);
        }

        #region Add Parameter TO Query
        /// <summary>
        /// Store_Param(iSql, "@prc", SqlDbType.VarChar,"C");
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            param1.Value = values;
            sqlCmd.Parameters.Add(param1);
        }

        /// <summary>
        /// Store_Param(iSql, "@MSG_CD", SqlDbType.VarChar, "", ParameterDirection.Output);
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        /// <param name="Param_io"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values, ParameterDirection Param_io)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            if (Param_io == ParameterDirection.Input || Param_io == ParameterDirection.InputOutput)
                param1.Value = values;
            param1.Direction = Param_io;
            sqlCmd.Parameters.Add(param1);
        }

        public object GetParameterValue(NpgsqlCommand command, string parameterName)
        {
            return command.Parameters[parameterName].Value;
        }

        /// <summary>
        /// int(string) return = (int/string)Return_Param("@return");
        /// </summary>
        /// <param name="param">"@param"</param>
        /// <returns>object</returns>
        public object GetParameter(string param)
        {
            return sqlCmd.Parameters[param].Value;
        }

        #endregion

        #region Generating SqlCommand

        private NpgsqlCommand PrepareCommand(CommandType commandType, string commandText)
        {
            if (connection == null)
            {
                connection = new NpgsqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
            command.CommandType = commandType;
            return command;
        }

        public NpgsqlCommand GetStoreProcedureCommand(string spname)
        {
            return PrepareCommand(CommandType.StoredProcedure, spname);
        }

        public NpgsqlCommand GetSqlQueryCommand(string query)
        {
            return PrepareCommand(CommandType.Text, query);
        }
        #endregion

        #region Direct Quer

        public int DirectNonQuery(string query)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            int iResult = ExecuteNonQuery(sc);
            return iResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public DataTable DirectQuery(string query)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, string.Empty);
        }

        public DataTable DirectQuery(string query, string tableName)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, tableName);
        }

        /// <summary>
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns>Return 1(Success) / 0(Fail)</returns>
        public int StringNonQuery(string sQuery)
        {
            return new PostgresqlHelper().DirectNonQuery(sQuery);
        }
        #endregion

        #region Database Related Command

        public int ExecuteNonQuery(NpgsqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExcuteNonQuery()
        {
            return sqlExcuteNonQuery(0);
        }

        /// <summary>
        /// Sql Excute Command
        /// Transaction: 1 begin transaction
        /// Transaction: 0 No transaction
        /// </summary>
        /// <param name="Transaction">Transaction= 1 Or 0</param>
        /// <returns>Return Effective Rows</returns>
        public int sqlExcuteNonQuery(int Transaction)
        {
            try
            {
                sqlData.SelectCommand = sqlCmd;
                sqlCmd.Connection = new NpgsqlConnection(this.connectionString);
                sqlCmd.Connection.Open();
                if (Transaction > 0)
                    sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction(IsolationLevel.ReadCommitted);

                int rtn = sqlCmd.ExecuteNonQuery();

                if (Transaction > 0)
                    sqlCmd.Transaction.Commit();
                sqlCmd.Connection.Close();

                return rtn;
            }
            catch
            {
                if (Transaction > 0)
                    sqlCmd.Transaction.Rollback();

                return -1;
            }
            finally
            {
            }
        }

        public object ExecuteScalar(NpgsqlCommand command)
        {
            try
            {
                command.Connection.Open();
                return command.ExecuteScalar();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public object Scalar(NpgsqlCommand command)
        {
            try
            {
                DataSet dt = new DataSet();
                try
                {
                    command.Connection.Open();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter();
                    da.SelectCommand = command;
                    var ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public void ExecuteStore(List<string> values)
        {
            using (var conn = new NpgsqlConnection(this.connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {

                }
                string str;

                NpgsqlTransaction tran = conn.BeginTransaction();
                try
                {
                    var comm = new NpgsqlCommand("getallmodule", conn) { CommandType = CommandType.StoredProcedure };
                    AssignParameters(comm, values);
                    comm.ExecuteNonQuery();
                    using (var dr = comm.ExecuteReader())
                    {
                        str = dr[0].ToString() + dr[1].ToString();
                    }
                }

                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //private static void AssignParameters(NpgsqlCommand comm, params object[] values)
        //{
        //    try
        //    {
        //        //DiscoveryParameters(comm);
        //        // assign value
        //        var index = 0;
        //        foreach (NpgsqlParameter param in comm.Parameters)
        //        {
        //            param.Value = values[index];
        //            index++;
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //    }
        //}

        public NpgsqlDataReader ExecuteReader(NpgsqlCommand command)
        {
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public NpgsqlDataReader ExecuteReader(NpgsqlCommand command, CommandBehavior commandBehavior)
        {
            return command.ExecuteReader(commandBehavior);
        }

        public DataTable LoadDataTable(NpgsqlCommand command, string tableName)
        {
            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable(tableName))
                {
                    da.Fill(dt);
                    return dt;
                }
            }
        }
        public DataTable LoadDataTable(string connectionString, string commandText, List<NpgsqlParameter> parameters)
        {
            if (commandText.IndexOf("{0}") > 0)
                commandText = string.Format(commandText, Schema);
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                       ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
                }
                var comm = new NpgsqlCommand(commandText, conn);
                NpgsqlTransaction tran = conn.BeginTransaction();
                try
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            comm.Parameters.Add((NpgsqlParameter)((ICloneable)item).Clone());
                        }
                    }

                    if (comm.Parameters.Count() > 0)
                    {
                        var checkRefcursor = comm.Parameters.Where(x => x.NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Refcursor);
                        if (checkRefcursor != null)
                        {
                            if (checkRefcursor.Count() > 0)
                            {
                                if (checkRefcursor.First().Value == null)
                                    checkRefcursor.First().Value = checkRefcursor.First().ParameterName;
                                var execute = comm.ExecuteNonQuery();
                                comm.CommandText = String.Format("fetch all in \"{0}\"", checkRefcursor.First().ParameterName);
                                comm.CommandType = CommandType.Text;
                            }
                        }
                    }

                    using (var dr = comm.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dr);
                        return dataTable;
                    }
                }
                catch (NpgsqlException ex)
                {
                    tran.Rollback();
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    tran.Commit();
                    conn.Close();
                }

            }
        }

        public DataSet LoadDataSet(NpgsqlCommand command, string[] tableNames)
        {
            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(command))
            {
                using (DataSet ds = new DataSet())
                {
                    da.Fill(ds);
                    if (tableNames != null)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            try
                            {
                                ds.Tables[i].TableName = tableNames[i];
                            }
                            catch
                            {
                            }
                        }
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet LoadDataSet()
        {
            //
            DataSet ds = new DataSet();
            try
            {
                sqlData.SelectCommand = sqlCmd;
                sqlCmd.Connection = new NpgsqlConnection(this.connectionString);

                sqlData.Fill(ds);

                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        private NpgsqlTransaction PrepareTransaction(IsolationLevel isolationLevel)
        {
            if (connection == null)
            {
                connection = new NpgsqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            return connection.BeginTransaction(isolationLevel);
        }

        public NpgsqlTransaction BeginTransaction()
        {
            return PrepareTransaction(IsolationLevel.ReadCommitted);
        }

        public NpgsqlTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return PrepareTransaction(isolationLevel);
        }

        public void Commit(NpgsqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Commit();
        }

        public void RollBack(NpgsqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Rollback();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Destructor

        ~PostgresqlHelper()
        {
            Dispose();
        }
        #endregion

        #region Connect PostgreeSql Clone từ Web Api
        public static Dictionary<string, NpgsqlParameter[]> m_CachedNpgParameters = new Dictionary<string, NpgsqlParameter[]>();
        public static void DiscoveryParameters(NpgsqlCommand comm)
        {
            try
            {
                var cachedKey = comm.CommandText;
                if (m_CachedNpgParameters.ContainsKey(cachedKey))
                {
                    var source = m_CachedNpgParameters[cachedKey];
                    foreach (var param in source)
                    {
                        comm.Parameters.Add((NpgsqlParameter)((ICloneable)param).Clone());
                    }
                }
                else
                {
                    NpgsqlCommandBuilder.DeriveParameters(comm);
                    comm.CommandText = cachedKey;
                    var source = new NpgsqlParameter[comm.Parameters.Count];
                    for (var i = 0; i < comm.Parameters.Count; i++)
                    {
                        source[i] = (NpgsqlParameter)((ICloneable)comm.Parameters[i]).Clone();
                    }
                    m_CachedNpgParameters.Add(cachedKey, source);
                }

            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_DISCOVERY_PARAMS_FAIL, ex.Message, comm.CommandText);
            }
        }
        private static void AssignParameters(NpgsqlCommand comm, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                for (int i = 0; i < comm.Parameters.Count(); i++)// NpgsqlParameter param in comm.Parameters)
                {
                    var param = comm.Parameters[i];
                    if (param.Direction == ParameterDirection.Input && param.NpgsqlDbType != NpgsqlTypes.NpgsqlDbType.Refcursor/* || param.Direction == ParameterDirection.InputOutput*/)
                    {
                        if (values.Length == 0)
                        {
                            continue;
                        }
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            switch (param.DbType)
                            {
                                case DbType.Date:
                                    param.Value = Convert.ToDateTime(values[index]);
                                    break;
                                case DbType.Boolean:
                                    if (values[index].ToString() == "1" || values[index].ToString() == "0")
                                    {
                                        param.Value = values[index].ToString() == "1" ? true : false;
                                    }
                                    else
                                    {
                                        param.Value = Convert.ToBoolean(values[index]);
                                    }

                                    break;
                                case DbType.Byte:
                                    param.Value = Convert.ToByte(values[index]);
                                    break;
                                case DbType.Int16:
                                case DbType.Int32:
                                case DbType.Int64:
                                case DbType.Single:
                                case DbType.Double:
                                case DbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index]);
                                    break;
                                default:
                                    var parameter1 = comm.CreateParameter();
                                    parameter1.ParameterName = param.ParameterName;
                                    parameter1.NpgsqlDbType = NpgsqlDbType.Text;
                                    parameter1.Value = values[index];
                                    param = parameter1;
                                    comm.Parameters[i] = parameter1;
                                    break;
                            }
                        }
                        index++;
                    }
                    else if (param.NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Refcursor)
                    {
                        NpgsqlParameter rf = new NpgsqlParameter(param.ParameterName, NpgsqlTypes.NpgsqlDbType.Refcursor);
                        param = rf;
                        param.Value = param.ParameterName;//"ref1";
                        comm.Parameters[i] = param;
                        //index++;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(
                    ERR_SQL.ERR_SQL_ASSIGN_PARAMS_FAIL, ex.Message,
                    comm.CommandText, values);
            }
        }
        public static List<T> ExecuteStoreProcedurePostgreSQL<T>(string connectionString, string commandText, params object[] values)
           where T : class, new()
        {
            commandText = SchemaStatic + "." + commandText;
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                       ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
                }
                var comm = new NpgsqlCommand(commandText, conn);
                NpgsqlTransaction tran = conn.BeginTransaction();
                try
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    AssignParameters(comm, values);
                    if (comm.Parameters.Count() > 0)
                    {
                        var checkRefcursor = comm.Parameters.Where(x => x.NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Refcursor);
                        if (checkRefcursor != null)
                        {
                            if (checkRefcursor.Count() > 0)
                            {
                                var execute = comm.ExecuteNonQuery();
                                comm.CommandText = String.Format("fetch all in \"{0}\"", checkRefcursor.First().ParameterName);
                                comm.CommandType = CommandType.Text;
                            }
                        }
                    }

                    try
                    {
                        using (var dr = comm.ExecuteReader())
                        {
                            if (comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) && comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value)
                            {
                                var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                                if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                            }
                            return dr.ToList<T>();
                        }
                    }
                    catch (Exception e1)
                    {
                        comm.ExecuteNonQuery();
                        var t = 1;
                        return null;
                    }
                }
                catch (NpgsqlException ex)
                {
                    tran.Rollback();
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    tran.Commit();
                    conn.Close();
                }

            }
        }

        public static DataTable ExecuteStoreProcedurePostgreSQLToDataTable(string connectionString, string commandText, params object[] values)
        {
            commandText = SchemaStatic + "." + commandText;
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                       ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
                }
                var comm = new NpgsqlCommand(commandText, conn);
                NpgsqlTransaction tran = conn.BeginTransaction();
                try
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    AssignParameters(comm, values);
                    if (comm.Parameters.Count() > 0)
                    {
                        var checkRefcursor = comm.Parameters.Where(x => x.NpgsqlDbType == NpgsqlTypes.NpgsqlDbType.Refcursor);
                        if (checkRefcursor != null)
                        {
                            if (checkRefcursor.Count() > 0)
                            {
                                var execute = comm.ExecuteNonQuery();
                                comm.CommandText = String.Format("fetch all in \"{0}\"", checkRefcursor.First().ParameterName);
                                comm.CommandType = CommandType.Text;
                            }
                        }
                    }

                    using (var dr = comm.ExecuteReader())
                    {
                        if (comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) && comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value)
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        return dr.ToDataTable();
                    }
                }
                catch (NpgsqlException ex)
                {
                    tran.Rollback();
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    tran.Commit();
                    conn.Close();
                }

            }
        }
        public static Exception ThrowSqlUserException(NpgsqlException ex, string commandText)
        {
            if (ex.ErrorCode == CONSTANTS.SQL_USER_HANDLED_EXCEPTION_CODE)
            {
                var match = Regex.Match(ex.Message, "<ERROR ID=([0-9]+)>([^<]*)</ERROR>");
                if (match.Success)
                {
                    var errCode = int.Parse(match.Groups[1].Value);
                    var errMessage = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        return ErrorUtils.CreateErrorWithSubMessage(errCode, errMessage);
                    }
                    return ErrorUtils.CreateError(errCode);
                }
            }
            return ErrorUtils.CreateErrorWithSubMessage(
                ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
        }
        public static void DiscoveryParameters(string connectionString, string commandText, List<NpgsqlParameter> parrs)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                       ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                       commandText);
                }

                try
                {
                    var comm = new NpgsqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    DiscoveryParameters(comm);
                    parrs.AddRange(from NpgsqlParameter param in comm.Parameters where param.Direction == ParameterDirection.Input
                                   select new NpgsqlParameter
                                   {
                                       //StoreName = commandText,
                                       DbType = param.DbType,
                                       NpgsqlDbType = param.NpgsqlDbType,
                                       ParameterName = param.ParameterName
                                   });
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion
    }

    public static class PostgreComponentExtensions
    {
        private static readonly Dictionary<Type, object> CachedMapInfo = new Dictionary<Type, object>();
        public static List<T> ToList<T>(this NpgsqlDataReader reader)
            where T : class, new()
        {
            var col = new List<T>();
            while (reader.Read())
            {
                var obj = new T();
                reader.MapObject(obj);
                col.Add(obj);
            }
            return col;
        }
        public static DataTable ToDataTable(this NpgsqlDataReader reader)
        {
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public static T ToObject<T>(this NpgsqlDataReader reader)
            where T : class, new()
        {
            var obj = new T();
            reader.Read();
            MapObject(reader, obj);
            return obj;
        }

        private static void MapObject<T>(this NpgsqlDataReader reader, T obj)
            where T : class, new()
        {
            var mapInfo = GetMapInfo<T>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == "fieldsgroup")
                {

                }
                if (reader[i] != DBNull.Value && mapInfo.ContainsKey(reader.GetName(i).ToUpper()))
                {
                    var prop = mapInfo[reader.GetName(i).ToUpper()];
                    prop.SetValue(obj, Convert.ChangeType(reader[i], prop.PropertyType), null);
                }
            }
        }

        private static Dictionary<string, PropertyInfo> GetMapInfo<T>()
        {
            var type = typeof(T);
            if (CachedMapInfo.ContainsKey(type))
            {
                return (Dictionary<string, PropertyInfo>)CachedMapInfo[type];
            }

            var mapInfo = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                foreach (ColumnAttribute attr in attributes)
                {
                    mapInfo.Add(attr.Name, prop);
                }
            }

            CachedMapInfo.Add(type, mapInfo);
            return mapInfo;
        }
    }
}
