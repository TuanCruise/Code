using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using WB.SYSTEM;
using WB.SystemLibrary;

namespace Host.DataBaseAccessService
{
 
  public sealed class DBManager:  IDBManager, IDisposable
  {
      private IDbConnection idbConnection;
      private IDataReader idataReader;
      private IDbCommand idbCommand;
      private DataProvider providerType;
      private IDbTransaction idbTransaction =null;
      private IDbDataParameter[]idbParameters =null;
      private string strConnection;
      public string result = null;

      public DBManager()
      {
            //string strProviderType = ConfigurationManager.AppSettings["DBConnectionProvider"].ToString();
            //string strConnectionString = ConfigurationManager.AppSettings["DBConnectionString"].ToString();
            //strConnectionString = UtilEncrypt.DePass(strConnectionString);
            string strProviderType = "SqlServer";
            //string strConnectionString = "Server=(local);UID=sa;PWD=sa;database=WFHOST;MultipleActiveResultSets=True";
            string strConnectionString = "data source=113.190.16.55,1433;initial catalog=POSCORE;user id=audit;password=123456a@;MultipleActiveResultSets=true; Min Pool Size=10;Max Pool Size=1000;Pooling=true;";

            if (strProviderType == "SqlServer")
            {
                this.providerType = DataProvider.SqlServer;
                this.strConnection = strConnectionString;
            }
            else if (strProviderType == "Oracle")
            {
                this.providerType = DataProvider.Oracle;
                this.strConnection = strConnectionString;
            }
      }
    
      public DBManager(DataProvider providerType)
      {
          this.providerType = providerType;
      }
 
      public DBManager(DataProvider providerType,string connectionString)
      {
          this.providerType = providerType;
          this.strConnection = connectionString;
      }

      public IDbConnection Connection
      {
          get{return idbConnection;}
      }
 
      public IDataReader DataReader
      {
      
          get{return idataReader;}
          set{idataReader = value;}
      }

      public DataProvider ProviderType
      {
          get{return providerType;}
          set{providerType = value;}
      }

      public string ConnectionString
      {
          get{return strConnection;}
          set{strConnection = value;}
      }

      public IDbCommand Command
      {
          get{return idbCommand;}
      }

      public IDbTransaction Transaction
      {
          get{return idbTransaction;}
      }

      public IDbDataParameter[]Parameters
      {
          get{return idbParameters;}
          set { idbParameters = value; }
      }

      public void Open()
      {
            try
            {
                idbConnection = DBManagerFactory.GetConnection(this.providerType);
                idbConnection.ConnectionString = this.ConnectionString;

                if (idbConnection.State != ConnectionState.Open)
                    idbConnection.Open();

                this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            }
            catch(Exception ex)
            {
                throw ex;
            }
      }

      public void Close()
      {
          if (idbConnection!=null && idbConnection.State != ConnectionState.Closed)
              idbConnection.Close();
      }

      public void Dispose()
      {
          GC.SuppressFinalize(this);
          this.Close();
          this.idbCommand = null;
          this.idbTransaction = null;
          this.idbConnection = null;
      }
   
      public void CreateParameters(int paramsCount)
      {
          idbParameters = new IDbDataParameter[paramsCount];
          idbParameters =DBManagerFactory.GetParameters(this.ProviderType, paramsCount);
      }
      public void CreateParameters(ParamStruct[] param)
      {
          if (param != null)
          {
              int intParamLength = param.Length;
              idbParameters = new IDbDataParameter[intParamLength];
              idbParameters = DBManagerFactory.GetParameters(this.ProviderType, intParamLength);
              for (int i = 0; i <= intParamLength - 1; i++)
              {
                  ParamStruct ps = param[i];
                  IDbDataParameter pm = DBManagerFactory.GetParameter(this.ProviderType);
                  pm.ParameterName = ps.ParameterName;
                  pm.Value = ps.Value;
                  pm.Direction  = ps.Direction;
                  pm.DbType = ps.DbType;
                  pm.SourceColumn = ps.SourceColumn;
                  idbParameters[i] = pm;
              }
          }
      }
      public ParamStruct SetDBType(ParamStruct pmi, string FieldType)
      {

          switch (FieldType)
          {
              case "bigint":
                  pmi.DbType = DbType.UInt64;
                  break;
              case "binary":
                  pmi.DbType = DbType.Binary;
                  break;
              case "char":
                  pmi.DbType = DbType.String;
                  break;
              case "datetime":
                  pmi.DbType = DbType.DateTime;
                  break;
              case "decimal":
                  pmi.DbType = DbType.Decimal;
                  break;
              case "float":
                  pmi.DbType = DbType.Double;
                  break;
              case "int":
                  pmi.DbType = DbType.Int32;
                  break;
              case "nchar":
                  pmi.DbType = DbType.String;
                  break;
              case "ntext":
                  pmi.DbType = DbType.String;
                  break;
              case "nvarchar":
                  pmi.DbType = DbType.String;
                  break;
              case "real":
                  pmi.DbType = DbType.Double;
                  break;
              case "smalldatetime":
                  pmi.DbType = DbType.DateTime;
                  break;
              case "smallint":
                  pmi.DbType = DbType.Int32;
                  break;
              case "text":
                  pmi.DbType = DbType.String;
                  break;
              case "tinyint":
                  pmi.DbType = DbType.Int16;
                  break;
              case "System.varchar":
                  pmi.DbType = DbType.String;
                  break;
              default:
                  pmi.DbType = DbType.String;
                  break;

          }

          return pmi;
      }

      public void AddParameters(int index, string paramName, object objValue)
      {    
          if (index < idbParameters.Length)
          {
            idbParameters[index].ParameterName =paramName;
            idbParameters[index].Value = objValue;
          }
      }

      public void BeginTransaction()
      {
          if (this.idbTransaction == null)
              idbTransaction = DBManagerFactory.GetTransaction(this.ProviderType, this.idbConnection);

          this.idbCommand.Transaction =idbTransaction;
      }

      public void CommitTransaction()
      {
          if (this.idbTransaction != null)
              this.idbTransaction.Commit();

          idbTransaction = null;
      }

      public IDataReader ExecuteReader(CommandType commandType, string commandText)
      {
          try
          {
              this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
              idbCommand.Connection = this.Connection;
              PrepareCommand(idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
              this.DataReader = idbCommand.ExecuteReader();
              idbCommand.Parameters.Clear();
              return this.DataReader;
          }
          catch (Exception ex)
          {
              ErrorHandler.ThrowError(ex);
              return null;
          }
      }
      
      public IDataReader ExecuteReader(string commandText,CommandType commandType)
      {
          try
          {
              this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
              idbCommand.Connection = this.Connection;
              PrepareCommand(idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
              this.DataReader = idbCommand.ExecuteReader();
              idbCommand.Parameters.Clear();
              return this.DataReader;
          }
          catch (Exception ex)
          {
              ErrorHandler.ThrowError(ex);
              return null;
          }
      }
      
      public void CloseReader()
      {
          if (this.DataReader != null)
              this.DataReader.Close();
      }
      
      private void AttachParameters(IDbCommand command, IDbDataParameter[]commandParameters)
      {
          foreach (IDbDataParameter idbParameter in commandParameters)
          {
              if ((idbParameter.Direction == ParameterDirection.InputOutput) && (idbParameter.Value == null))
              {
                  idbParameter.Value = DBNull.Value;
              }
              command.Parameters.Add(idbParameter);
          }
      }
      
      private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[]commandParameters)
      {
          //command.Connection = connection;
          //command.CommandText = commandText;
          //command.CommandType = commandType;

          
          //if (transaction != null)
          //{
          //    command.Transaction = transaction;
          //}
          
          //if (commandParameters != null)
          //{
          //    AttachParameters(command, commandParameters);
          //}

          try
          {
              if (providerType == DataProvider.SqlServer && commandText != null && commandText != "" && commandText.IndexOf("set dateformat dmy") == -1 && commandType != CommandType.StoredProcedure)
              {
                  commandText = "set dateformat dmy " + commandText;
              }


              command.Connection = connection;
              command.CommandText = commandText;
              command.CommandType = commandType;
              command.CommandTimeout = 300;

              if (transaction != null)
              {
                  command.Transaction = transaction;
              }

              if (commandParameters != null)
              {
                  AttachParameters(command, commandParameters);
              }

            }
          catch (Exception ex)
          {               
                ErrorHandler.ThrowError(ex);        
          }

      }


      public int ExecuteNonQuery(CommandType commandType, string commandText)
      {
          try
          {
              this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
              PrepareCommand(idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
              int returnValue = idbCommand.ExecuteNonQuery();
              idbCommand.Parameters.Clear();
              this.Parameters = null;
              return returnValue;
          }
          catch (Exception ex)
          {
              ErrorHandler.ThrowError(ex);
              return -1;
          }
        
      }
      
      public object ExecuteScalar(CommandType commandType, string commandText)
      {
          this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
          PrepareCommand(idbCommand,this.Connection, this.Transaction, commandType, commandText, this.Parameters);
          object returnValue = idbCommand.ExecuteScalar();
          idbCommand.Parameters.Clear();
          return returnValue;
      }
      
      public DataSet ExecuteDataSet(CommandType commandType, string commandText)
      {
          this.idbCommand =DBManagerFactory.GetCommand(this.ProviderType);
          PrepareCommand(idbCommand,this.Connection, this.Transaction, commandType, commandText, this.Parameters);
          IDbDataAdapter dataAdapter =DBManagerFactory.GetDataAdapter(this.ProviderType);
          dataAdapter.SelectCommand = idbCommand;
          DataSet dataSet = new DataSet();
          dataAdapter.Fill(dataSet);
          idbCommand.Parameters.Clear();
          return dataSet;
      }
  }
}