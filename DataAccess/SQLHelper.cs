using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Transactions;
using WebCore.Base;
using WebCore.Common;
using WebCore.Entities;
using WebCore.Utils;

namespace Core.DataAccess
{
    public class SQLHelper
    {
        public static Dictionary<string, SqlParameter[]> m_CachedParameters;
        static SQLHelper()
        {
            m_CachedParameters = new Dictionary<string, SqlParameter[]>();
        }

        public static void ExecuteStoreProcedure(string connectionString, Session session, string commandText, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {

                }

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };

                        AssignParameters(comm, session, values);

                        comm.ExecuteNonQuery();
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                    }

                    catch (Exception ex)
                    {
                        throw ErrorUtils.CreateErrorWithSubMessage(
                            ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                            commandText, values);
                    }
                    finally
                    {
                        scope.Complete();
                        conn.Close();
                    }
                }

            }
        }

        public object Scalar(SqlCommand command)
        {
            try
            {
                DataSet dt = new DataSet();
                try
                {
                    command.Connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    var ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command
            var retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, IEnumerable<SqlParameter> commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        private static void AttachParameters(SqlCommand command, IEnumerable<SqlParameter> commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters == null) return;
            foreach (var p in commandParameters.Where(p => p != null))
            {
                // Check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput ||
                     p.Direction == ParameterDirection.Input) &&
                    (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        public static void FillDataTable(string connectionString, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                resultTable = null;
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
                }

                try
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        var adap = new SqlDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        resultTable = ds.Tables[0];
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public static void FillDataTable(string connectionString, Session session, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
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

                try
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        var adap = new SqlDataAdapter(comm);
                        var ds = new DataSet();
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, values);

                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        resultTable = ds.Tables[0];
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static List<T> ExecuteStoreProcedure<T>(string connectionString, string commandText, params object[] values)
           where T : class, new()
        {
            using (var conn = new SqlConnection(connectionString))
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
                using (var comm = new SqlCommand(commandText, conn))
                {
                    try
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, values);

                        using (var dr = comm.ExecuteReader())
                        {
                            if (
                                comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                                comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                                )
                            {
                                var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                                if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                            }

                            return dr.ToList<T>();
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ThrowSqlUserException(ex, commandText);
                    }
                    catch (FaultException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw ErrorUtils.CreateErrorWithSubMessage(
                            ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }




        private static void AssignParameters(SqlCommand comm, params object[] values)
        {
            try
            {

                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (SqlParameter param in comm.Parameters)
                {
                    if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
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
                                case DbType.Byte:
                                case DbType.Int16:
                                case DbType.Int32:
                                case DbType.Int64:
                                case DbType.Single:
                                case DbType.Double:
                                case DbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index]);
                                    break;
                                default:
                                    param.Value = values[index];
                                    break;
                            }
                        }
                        index++;
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

        private static void AssignParameters(SqlCommand comm, Session session, params object[] values)
        {
            try
            {

                comm.Parameters.Clear();
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (SqlParameter param in comm.Parameters)
                {
                    if (param.ParameterName == "@" + CONSTANTS.ORACLE_SESSION_USER)
                    {
                        param.Value = session.Username;
                    }
                    else if (param.ParameterName == "@" + CONSTANTS.ORACLE_SESSIONKEY)
                    {
                        param.Value = session.SessionKey;
                    }

                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else
                        {
                            switch (param.SqlDbType)
                            {
                                case SqlDbType.Date:
                                    param.Value = Convert.ToDateTime(values[index]);
                                    break;
                                case SqlDbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index]);
                                    break;
                                default:
                                    param.Value = values[index];
                                    break;
                            }
                        }
                        index++;
                    }
                }
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(
                    ERR_SQL.ERR_SQL_ASSIGN_PARAMS_FAIL, ex.Message,
                    comm.CommandText, values);
            }
        }


        public static void DiscoveryParameters(SqlCommand comm)
        {
            try
            {

                //discovery parameter
                var cachedKey = comm.CommandText;
                if (m_CachedParameters.ContainsKey(cachedKey))
                {
                    var source = m_CachedParameters[cachedKey];
                    foreach (var param in source)
                    {
                        comm.Parameters.Add((SqlParameter)((ICloneable)param).Clone());
                    }
                }
                else
                {
#if DEBUG
                    //comm.CommandText += "--" + (new Random().Next());
#endif
                    SqlCommandBuilder.DeriveParameters(comm);
                    comm.CommandText = cachedKey;
                    var source = new SqlParameter[comm.Parameters.Count];
                    for (var i = 0; i < comm.Parameters.Count; i++)
                    {
                        //source[i] = (SqlParameter)comm.Parameters[i];
                        source[i] = (SqlParameter)((ICloneable)comm.Parameters[i]).Clone();
                    }
                    m_CachedParameters.Add(cachedKey, source);
                }

            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateErrorWithSubMessage(
                    ERR_SQL.ERR_SQL_DISCOVERY_PARAMS_FAIL, ex.Message,
                    comm.CommandText);
            }
        }

        public static void DiscoveryParameters(string connectionString, string commandText, List<SqlParameter> parrs)
        {
            using (var conn = new SqlConnection(connectionString))
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
                    var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    DiscoveryParameters(comm);
                    parrs.AddRange(from SqlParameter param in comm.Parameters where param.Direction == ParameterDirection.Input
                                   select new SqlParameter
                                   {
                                       //StoreName = commandText,
                                       DbType = param.DbType,
                                       SqlDbType = param.SqlDbType,
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

        public static List<T> ExecuteStoreProcedureGeneric<T>(string connectionString, Session session, string commandText, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                        commandText, values);
                }

                try
                {
                    var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    AssignParameters(comm, session, values);

                    using (var dr = comm.ExecuteReader())
                    {
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                        var list = new List<T>();
                        while (dr.Read())
                        {
                            list.Add((T)Convert.ChangeType(dr.GetValue(0), typeof(T)));
                        }
                        return list;
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText, values);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void FillDataSet(string connectionString, Session session, string commandText, string tableName, DataSet ds, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                        commandText, values);
                }

                try
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, values);

                        var adap = new SqlDataAdapter(comm);
                        adap.Fill(ds, tableName);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText, values);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //public static SqlDataReader[] ExecuteReader(string connectionString, Session session, string commandText, params object[] values)
        //{
        //    var conn = new SqlConnection(connectionString);
        //    try {
        //        conn.Open();
        //    }
        //    catch (Exception ex) {
        //        throw ErrorUtils.CreateErrorWithSubMessage(
        //            ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
        //    }

        //    try {
        //        var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
        //        AssignParameters(comm, session, values);

        //        comm.ExecuteNonQuery();

        //        if (
        //            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
        //            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value &&
        //            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != null
        //            ) {
        //            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
        //            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
        //        }

        //        var readers = (from SqlParameter param in comm.Parameters
        //                       where param.SqlDbType == SqlDbType.RefCursor
        //                       select
        //                           (param.Value as SqlDataReader) ?? ((OracleRefCursor)param.Value).GetDataReader()).ToArray();

        //        return readers;
        //    }
        //    catch (SqlException ex) {
        //        throw ThrowSqlUserException(ex, commandText);
        //    }
        //    catch (FaultException) {
        //        throw;
        //    }
        //    catch (Exception ex) {
        //        throw ErrorUtils.CreateErrorWithSubMessage(
        //            ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
        //    }
        //}

        public static void FillDataSet(string connectionString, Session session, string commandText, out DataSet ds, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                        commandText, values);
                }

                try
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, values);
                        ds = new DataSet();
                        var adap = new SqlDataAdapter(comm);
                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText, values);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static void FillDataSet(string connectionString, Session session, string moduleID, string commandText, out DataSet ds, params object[] values)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message,
                        commandText, values);
                }

                try
                {
                    using (var comm = new SqlCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, moduleID, values);
                        ds = new DataSet();
                        var adap = new SqlDataAdapter(comm);
                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw ThrowSqlUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText, values);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //public static SqlDataAdapter[] ExecuteReader(string connectionString, Session session, string commandText, params object[] values)
        //{
        //    var conn = new SqlConnection(connectionString);
        //    try
        //    {
        //        conn.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorUtils.CreateErrorWithSubMessage(
        //            ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, commandText);
        //    }

        //    try
        //    {
        //        var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
        //        AssignParameters(comm, session, values);

        //        comm.ExecuteNonQuery();

        //        if (
        //            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
        //            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value &&
        //            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != null
        //            )
        //        {
        //            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
        //            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
        //        }

        //        var readers = (from SqlParameter param in comm.Parameters
        //                       where param.SqlDbType == SqlDbType.RefCursor
        //                       select
        //                           (param.Value as OracleDataReader) ?? ((OracleRefCursor)param.Value).GetDataReader()).ToArray();

        //        return readers;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ThrowSqlUserException(ex, commandText);
        //    }
        //    catch (FaultException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ErrorUtils.CreateErrorWithSubMessage(
        //            ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, commandText);
        //    }
        //}

        public static void DiscoveryParameters(string connectionString, string commandText, List<OracleParam> @params)
        {
            using (var conn = new SqlConnection(connectionString))
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
                    var comm = new SqlCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    DiscoveryParameters(comm);

                    //foreach(SqlParameter p in comm.Parameters) {
                    //    var objparam = new OracleParam();
                    //    objparam.StoreName = commandText;
                    //    objparam.Value = p.ParameterName;
                    //    @params.Add(objparam);
                    //}

                    @params.AddRange(from SqlParameter param in comm.Parameters
                                     where param.Direction == ParameterDirection.Input
                                     select new OracleParam
                                     {
                                         StoreName = commandText,
                                         Name = param.ParameterName
                                     });
                }
                catch (FaultException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #region Private Methods

        private static DataTable Query(String consulta, IList<SqlParameter> parametros)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection connection = new SqlConnection(WebCore.Common.App.Configs.ConnectionString);
                SqlCommand command = new SqlCommand();
                SqlDataAdapter da;
                try
                {
                    command.Connection = connection;
                    command.CommandText = consulta;
                    if (parametros != null)
                    {
                        command.Parameters.AddRange(parametros.ToArray());
                    }
                    da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

        }
        private static int NonQuery(string query, IList<SqlParameter> parametros)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlConnection connection = new SqlConnection(App.Configs.ConnectionString);
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteNonQuery();

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

        private static object Scalar(string query, List<SqlParameter> parametros)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlConnection connection = new SqlConnection(App.Configs.ConnectionString);
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteScalar();

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
        #endregion

        public static Exception ThrowSqlUserException(SqlException ex, string commandText)
        {
            if (ex.Number == CONSTANTS.SQL_USER_HANDLED_EXCEPTION_CODE)
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

    }
    public static class SQLComponentExtensions
    {
        private static readonly Dictionary<Type, object> CachedMapInfo = new Dictionary<Type, object>();
        public static List<T> ToList<T>(this SqlDataReader reader)
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

        public static T ToObject<T>(this SqlDataReader reader)
            where T : class, new()
        {
            var obj = new T();
            reader.Read();
            MapObject(reader, obj);
            return obj;
        }

        private static void MapObject<T>(this SqlDataReader reader, T obj)
            where T : class, new()
        {
            var mapInfo = GetMapInfo<T>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader[i] != DBNull.Value && mapInfo.ContainsKey(reader.GetName(i)))
                {
                    var prop = mapInfo[reader.GetName(i).ToUpper()];
                    //var prop = mapInfo[reader.GetName(i)];
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

