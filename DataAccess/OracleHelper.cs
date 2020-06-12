using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using WebCore.Base;
using WebCore.Common;
using WebCore.Entities;
using WebCore.Utils;

namespace Core.DataAccess
{
    public static class OracleHelper
    {
        internal static Dictionary<string, OracleParameter[]> m_CachedParameters;

        static OracleHelper()
        {
            m_CachedParameters = new Dictionary<string, OracleParameter[]>();
        }
        private static void AssignParameters(OracleCommand comm, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (param.OracleDbType == OracleDbType.NClob)
                        {
                            var lob = new OracleClob(comm.Connection);
                            var buffer = Encoding.Unicode.GetBytes(values[index].ToString());
                            lob.Write(buffer, 0, buffer.Length);

                            param.Value = lob;
                        }
                        else
                        {
                            switch (param.OracleDbType)
                            {
                                case OracleDbType.Date:
                                    param.Value = Convert.ToDateTime(values[index], App.Environment.ServerInfo.Culture);
                                    break;
                                case OracleDbType.Byte:
                                case OracleDbType.Int16:
                                case OracleDbType.Int32:
                                case OracleDbType.Int64:
                                case OracleDbType.Single:
                                case OracleDbType.Double:
                                case OracleDbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index], App.Environment.ServerInfo.Culture);
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
        private static void AssignParameters(OracleCommand comm, Session session, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.ParameterName == CONSTANTS.ORACLE_SESSION_USER)
                    {
                        param.Value = session.Username;
                    }
                    else if (param.ParameterName == CONSTANTS.ORACLE_SESSIONKEY)
                    {
                        param.Value = session.SessionKey;
                    }
                    else if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (param.OracleDbType == OracleDbType.NClob)
                        {
                            var lob = new OracleClob(comm.Connection);
                            var buffer = Encoding.Unicode.GetBytes(values[index].ToString());
                            lob.Write(buffer, 0, buffer.Length);

                            param.Value = lob;
                        }
                        else
                        {
                            switch (param.OracleDbType)
                            {
                                case OracleDbType.Date:
                                    param.Value = Convert.ToDateTime(values[index], App.Environment.ServerInfo.Culture);
                                    break;
                                case OracleDbType.Byte:
                                case OracleDbType.Int16:
                                case OracleDbType.Int32:
                                case OracleDbType.Int64:
                                case OracleDbType.Single:
                                case OracleDbType.Double:
                                case OracleDbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index], App.Environment.ServerInfo.Culture);
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

        private static void AssignParameters(OracleCommand comm, Session session, string moduleID, params object[] values)
        {
            try
            {
                DiscoveryParameters(comm);
                // assign value
                var index = 0;
                foreach (OracleParameter param in comm.Parameters)
                {
                    if (param.ParameterName == CONSTANTS.ORACLE_SESSION_USER)
                    {
                        param.Value = session.Username;
                    }
                    else if (param.ParameterName == CONSTANTS.ORACLE_MODULE_ID)
                    {
                        param.Value = moduleID;
                        index++;

                    }
                    else if (param.OracleDbType == OracleDbType.RefCursor)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                    else if (param.Direction == ParameterDirection.Input || param.Direction == ParameterDirection.InputOutput)
                    {
                        if (values[index] == null || (values[index] is string && (string)values[index] == string.Empty))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (param.OracleDbType == OracleDbType.NClob)
                        {
                            var lob = new OracleClob(comm.Connection);
                            var buffer = Encoding.Unicode.GetBytes(values[index].ToString());
                            lob.Write(buffer, 0, buffer.Length);

                            param.Value = lob;
                        }
                        else
                        {
                            switch (param.OracleDbType)
                            {
                                case OracleDbType.Date:
                                    param.Value = Convert.ToDateTime(values[index], App.Environment.ServerInfo.Culture);
                                    break;
                                case OracleDbType.Byte:
                                case OracleDbType.Int16:
                                case OracleDbType.Int32:
                                case OracleDbType.Int64:
                                case OracleDbType.Single:
                                case OracleDbType.Double:
                                case OracleDbType.Decimal:
                                    param.Value = Convert.ToDecimal(values[index], App.Environment.ServerInfo.Culture);
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

        public static List<T> ExecuteStoreProcedureGeneric<T>(string connectionString, Session session, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static List<T> ExecuteStoreProcedure<T>(string connectionString, Session session, string commandText, params object[] values)
            where T : class, new()
        {
            using (var conn = new OracleConnection(connectionString))
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

                using (var comm = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        comm.CommandType = CommandType.StoredProcedure;
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
                            return dr.ToList<T>();
                        }
                    }
                    catch (OracleException ex)
                    {
                        throw ThrowOracleUserException(ex, commandText);
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
                        if (comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME))
                        {
                            var value = comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value;
                            if (value != DBNull.Value)
                            {
                                throw ErrorUtils.CreateError(int.Parse(value.ToString()));
                            }
                        }

                        conn.Close();
                    }
                }
            }
        }

        public static void DiscoveryParameters(OracleCommand comm)
        {
            try
            {
                // discovery parameter
                var cachedKey = comm.CommandText;
                if (m_CachedParameters.ContainsKey(cachedKey))
                {
                    var source = m_CachedParameters[cachedKey];
                    foreach (var param in source)
                    {
                        comm.Parameters.Add((OracleParameter)param.Clone());
                    }
                }
                else
                {
#if DEBUG
                    comm.CommandText += "--" + (new Random().Next());
#endif
                    OracleCommandBuilder.DeriveParameters(comm);
                    comm.CommandText = cachedKey;
                    var source = new OracleParameter[comm.Parameters.Count];
                    for (var i = 0; i < comm.Parameters.Count; i++)
                    {
                        source[i] = (OracleParameter)comm.Parameters[i].Clone();
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

        public static void ExecuteStoreProcedure(string connectionString, Session session, string commandText, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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

                OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
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
                catch (OracleException ex)
                {
                    txn.Rollback();
                    throw ThrowOracleUserException(ex, commandText);
                }
                catch (FaultException)
                {
                    txn.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ErrorUtils.CreateErrorWithSubMessage(
                        ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message,
                        commandText, values);
                }
                finally
                {
                    txn.Commit();
                    conn.Close();
                }
            }
        }

        public static OracleDataReader[] ExecuteReader(string connectionString, Session session, string commandText, params object[] values)
        {
            var conn = new OracleConnection(connectionString);
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
                var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                AssignParameters(comm, session, values);

                comm.ExecuteNonQuery();

                if (
                    comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                    comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value &&
                    comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != null
                    )
                {
                    var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                    if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText, values);
                }

                var readers = (from OracleParameter param in comm.Parameters
                               where param.OracleDbType == OracleDbType.RefCursor
                               select
                                   (param.Value as OracleDataReader) ?? ((OracleRefCursor)param.Value).GetDataReader()).ToArray();

                return readers;
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex, commandText);
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
        }
        public static void FillDataTable(string connectionString, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static void FillDataTable(string connectionString, Session session, string commandText, out DataTable resultTable, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static void FillDataTable(string connectionString, Session session, string commandText, out DataTable resultTable, out string SecID, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        var adap = new OracleDataAdapter(comm);
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
                        var _secid = comm.Parameters[CONSTANTS.ORACLE_OUT_PARAMETER_SECID].Value.ToString();
                        SecID = null;
                        if (!string.IsNullOrEmpty(_secid)) SecID = _secid;
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static void FillDataSet(string connectionString, Session session, string commandText, string tableName, DataSet ds, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, values);

                        var adap = new OracleDataAdapter(comm);
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static void FillDataSet(string connectionString, Session session, string commandText, out DataSet ds, params object[] values)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, values);
                        ds = new DataSet();
                        var adap = new OracleDataAdapter(comm);
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session, moduleID, values);
                        ds = new DataSet();
                        var adap = new OracleDataAdapter(comm);
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
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        public static void DiscoveryParameters(string connectionString, string commandText, List<OracleParam> @params)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    var comm = new OracleCommand(commandText, conn) { CommandType = CommandType.StoredProcedure };
                    DiscoveryParameters(comm);

                    @params.AddRange(from OracleParameter param in comm.Parameters
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

        public static Exception ThrowOracleUserException(OracleException ex, string commandText)
        {
            if (ex.Number == CONSTANTS.ORACLE_USER_HANDLED_EXCEPTION_CODE)
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

        //duchvm add
        //internal static void BulkInsert(string ConnectionString, Session Session, string tableName, DataTable inputTable, bool result)
        //{
        //    using (var conn = new OracleConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //        }
        //        catch (Exception ex)
        //        {
        //            result = false;
        //            throw ErrorUtils.CreateErrorWithSubMessage(
        //                ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, "Error in bulk insert");
        //        }

        //        try
        //        {
        //            using (var bulkCopy = new OracleBulkCopy(conn))
        //            {
        //                bulkCopy.DestinationTableName = tableName;
        //                bulkCopy.WriteToServer(inputTable);
        //                result = true;
        //            }
        //        }
        //        catch (OracleException ex)
        //        {
        //            result = false;
        //            throw ThrowOracleUserException(ex, "Error in bulk insert");
        //        }
        //        catch (FaultException)
        //        {
        //            result = false;
        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            result = false;
        //            throw ErrorUtils.CreateErrorWithSubMessage(
        //                ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, "Error in bulk insert");
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //}

        //internal static void ExcelDataUpload(string ConnectionString, Session Session, List<DataTable> inputTable, string result, string ExcelTemplateId)
        //{
        //    using (var conn = new OracleConnection(ConnectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //        }
        //        catch (Exception ex)
        //        {
        //            result = "Open Connect Fail:" + ex.Message;
        //            throw ErrorUtils.CreateErrorWithSubMessage(
        //                ERR_SQL.ERR_SQL_OPEN_CONNECTION_FAIL, ex.Message, "Error in bulk insert");
        //        }

        //        try
        //        {
        //            string ReportID = inputTable[0].TableName;
        //            string sqlQuery1 = "PKG_EXCELREPORT.DROP_TEMTTABLE";
        //            string sqlQuery2 = @"CREATE GLOBAL TEMPORARY TABLE TEMP_EXCELREPORT
        //                                (exceltemplateid                VARCHAR2(6 BYTE) NOT NULL,
        //                                exreportname                   VARCHAR2(200 BYTE) NOT NULL,
        //                                md5hash                        VARCHAR2(200 BYTE),
        //                                excelfile                      BFILE,
        //                                createdate                     DATE,
        //                                createuser                     VARCHAR2(200 BYTE),
        //                                modifiedate                    DATE,
        //                                modifieduser                   VARCHAR2(200 BYTE),
        //                                isdeleted                      VARCHAR2(1 BYTE))";
        //            string sqlQuery3 = @"CREATE GLOBAL TEMPORARY TABLE TEMP_EXCELREPORTDATA
        //                                (xposition                      VARCHAR2(100 BYTE),
        //                                yposition                      VARCHAR2(100 BYTE),
        //                                value                          VARCHAR2(4000))";
        //            string sqlQuery4 = "PKG_EXCELREPORT.BATCH_UP_SIZE";
        //            string sqlQuery5 = "PKG_EXCELREPORT.COUNT_SIZE_TEMP";
        //            string sqlQuery6 = "PKG_EXCELREPORT.INSERT_REPORT_FROM_TEMP";

        //            var comm1 = new OracleCommand(sqlQuery1, conn);
        //            comm1.CommandType = CommandType.StoredProcedure;
        //            var comm2 = new OracleCommand(sqlQuery2, conn);
        //            var comm3 = new OracleCommand(sqlQuery3, conn);
        //            var comm4 = new OracleCommand(sqlQuery4, conn);
        //            comm4.CommandType = CommandType.StoredProcedure;
        //            comm4.Parameters.Add("CUR", OracleDbType.RefCursor, ParameterDirection.Output);
        //            var comm5 = new OracleCommand(sqlQuery5, conn);
        //            comm5.CommandType = CommandType.StoredProcedure;
        //            comm5.Parameters.Add("PV_REPORTID", OracleDbType.Varchar2, ReportID, ParameterDirection.Input);
        //            comm5.Parameters.Add("CUR", OracleDbType.RefCursor, ParameterDirection.Output);

        //            var comm6 = new OracleCommand(sqlQuery6, conn);
        //            comm6.CommandType = CommandType.StoredProcedure;
        //            comm6.Parameters.Add("PV_REPORTID", OracleDbType.Varchar2, ReportID, ParameterDirection.Input);
        //            comm6.Parameters.Add("CUR", OracleDbType.RefCursor, ParameterDirection.Output);

        //            //DROP TEMP TABLE AND RE-CREATE
        //            //comm1.ExecuteNonQuery();
        //            //comm2.ExecuteNonQuery();
        //            //comm3.ExecuteNonQuery();

        //            //GET BATCH SIZE 
        //            int batchSize = 5000;
        //            DataSet ds = new DataSet();
        //            var adap = new OracleDataAdapter(comm4);
        //            OracleDataReader objReadercomm4 = comm4.ExecuteReader();
        //            while (objReadercomm4.Read())
        //            {
        //                batchSize = int.Parse(objReadercomm4[0].ToString().Trim());
        //            }
        //            result = "Get Batch size Sucess, Size: " + batchSize;
        //            ////////////////
        //            //int TEMP_EXCELREPORT_SIZE_1 = 0;
        //            //int TEMP_EXCELREPORTDATA_SIZE_1 = 0;
        //            //int i_1 = 0;

        //            //OracleDataReader objReadercomm5_1 = comm5.ExecuteReader();
        //            //while (objReadercomm5_1.Read())
        //            //{
        //            //    if (i_1 == 0)
        //            //        TEMP_EXCELREPORT_SIZE_1 = int.Parse(objReadercomm5_1[0].ToString().Trim());
        //            //    else if (i_1 == 1)
        //            //        TEMP_EXCELREPORTDATA_SIZE_1 = int.Parse(objReadercomm5_1[0].ToString().Trim());
        //            //    else
        //            //        break;
        //            //    i_1++;
        //            //}
        //            ////////////////

        //            //INSERT DATA TO TEMP
        //            using (var bulkCopy = new OracleBulkCopy(conn))
        //            {
        //                bulkCopy.DestinationTableName = "TEMP_EXCELREPORT";
        //                bulkCopy.BatchSize = batchSize;
        //                bulkCopy.WriteToServer(inputTable[0]);
        //            }
        //            result = "Insert TEMP_EXCELREPORT Sucess";
        //            using (var bulkCopy = new OracleBulkCopy(conn))
        //            {
        //                bulkCopy.DestinationTableName = "TEMP_EXCELREPORTDATA";
        //                bulkCopy.BatchSize = batchSize;
        //                bulkCopy.WriteToServer(inputTable[1]);
        //            }

        //            result = "Insert TEMP_EXCELREPORTDATA Sucess";
        //            //VALIDATE SIZE UPLOAD
        //            int TEMP_EXCELREPORT_SIZE = 0;
        //            int TEMP_EXCELREPORTDATA_SIZE = 0;
        //            int i = 0;

        //            OracleDataReader objReadercomm5 = comm5.ExecuteReader();
        //            while (objReadercomm5.Read())
        //            {
        //                if (i == 0)
        //                    TEMP_EXCELREPORT_SIZE = int.Parse(objReadercomm5[0].ToString().Trim());
        //                else if (i == 1)
        //                    TEMP_EXCELREPORTDATA_SIZE = int.Parse(objReadercomm5[0].ToString().Trim());
        //                else
        //                    break;
        //                i++;
        //            }
        //            result = "VALIDATE SIZE UPLOAD Sucess";

        //            //INSERT DATA TO MAIN TABLE
        //            if (TEMP_EXCELREPORT_SIZE == 1 && TEMP_EXCELREPORTDATA_SIZE == inputTable[1].Rows.Count)
        //            {
        //                comm6.ExecuteNonQuery();
        //                result = "Done";
        //            }
        //            else
        //                result = @"Upload Fail, Count Fail: TEMP_EXCELREPORT_SIZE=" + TEMP_EXCELREPORT_SIZE +
        //                            "TEMP_EXCELREPORTDATA_SIZE=" + TEMP_EXCELREPORTDATA_SIZE
        //                            + "  Table0 size:" + inputTable[0].Rows.Count
        //                            + "  Table1 size:" + inputTable[1].Rows.Count
        //                            ;
        //        }
        //        catch (OracleException ex)
        //        {
        //            result = ex.Message;
        //            throw ThrowOracleUserException(ex, "Error in bulk insert");
        //        }
        //        catch (FaultException)
        //        {
        //            throw;
        //        }
        //        catch (Exception ex)
        //        {
        //            result = ex.Message;
        //            throw ErrorUtils.CreateErrorWithSubMessage(
        //                ERR_SQL.ERR_SQL_EXECUTE_COMMAND_FAIL, ex.Message, "Error in bulk insert");
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }
        //    }
        //}

        public static void FillDataSetWithoutPram(string connectionString, Session session, string commandText, out DataSet ds)
        {
            using (var conn = new OracleConnection(connectionString))
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
                    using (var comm = new OracleCommand(commandText, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        AssignParameters(comm, session);
                        ds = new DataSet();
                        var adap = new OracleDataAdapter(comm);
                        adap.Fill(ds);
                        if (
                            comm.Parameters.Contains(CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME) &&
                            comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value != DBNull.Value
                            )
                        {
                            var errCode = int.Parse(comm.Parameters[CONSTANTS.ORACLE_EXCEPTION_PARAMETER_NAME].Value.ToString());
                            if (errCode != 0) throw ErrorUtils.CreateError(errCode, commandText);
                        }
                    }
                }
                catch (OracleException ex)
                {
                    throw ThrowOracleUserException(ex, commandText);
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

        //end duchvm add
    }

    public static class ComponentExtensions
    {
        private static readonly Dictionary<Type, object> CachedMapInfo = new Dictionary<Type, object>();
        public static List<T> ToList<T>(this OracleDataReader reader)
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

        public static T ToObject<T>(this OracleDataReader reader)
            where T : class, new()
        {
            var obj = new T();
            reader.Read();
            MapObject(reader, obj);
            return obj;
        }

        private static void MapObject<T>(this OracleDataReader reader, T obj)
            where T : class, new()
        {
            var mapInfo = GetMapInfo<T>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader[i] != DBNull.Value && mapInfo.ContainsKey(reader.GetName(i)))
                {
                    var prop = mapInfo[reader.GetName(i)];
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