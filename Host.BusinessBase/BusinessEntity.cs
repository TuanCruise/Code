using System;
using System.ComponentModel;
using System.Collections;
using WB.SYSTEM;
using System.Data;
using System.Configuration;
using Host.DataBaseAccessService;
using WB.SystemLibrary;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Host.BusinessBase
{ 

    public class BusinessEntity 
    {
        private string _listTableNameNoLog = "MSGJOURNAL/WSREG/SESSIONLOG/MSGDETAIL/";
        private string _BranchID;
        private string _UserID;
        private ArrayList _arrProperties;
        private ArrayList _arrPK;
        private string _entityName;        
        private string _retrieveDataQuery;
        private DBManager _dbManager;
        public bool isLoadOK = false;

        public BusinessEntity()
        {
        }
        public DBManager dbManager
        {
            get { return _dbManager; }
            set { _dbManager = value; }
        }
        public string entityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }
        public string BranchID
        {
            get { return _BranchID; }
            set { _BranchID = value; }
        }
        public ArrayList arrPK
        {
            get { return _arrPK; }
            set { _arrPK = value; }
        }
        public string retrieveDataQuery
        {
            get { return _retrieveDataQuery; }
            set { _retrieveDataQuery = value; }
        }
        public ArrayList arrProperties
        {
            get { return _arrProperties; }
            set
            {
                _arrProperties = value;
            }
        }

        public virtual object getProperty(string property)
        {
            try
            {
                int i = 0;
                if (_arrProperties != null)
                {
                    while (i < _arrProperties.Count)
                    {
                        if (_arrProperties[i].ToString().ToUpper().Trim() == property.ToUpper())
                        {
                            return _arrProperties[i + 1];
                        }
                        i += 2;
                    }
                }

                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_GETPROPERTY;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }
        public virtual ArrayList getPKProperties()
        {
            try
            {
                ArrayList arrTmp = new ArrayList();
                for (int i = 0; i < _arrPK.Count; i++)
                {
                    arrTmp.Add(_arrPK[i].ToString());
                    arrTmp.Add(this.getProperty(_arrPK[i].ToString()));
                }
                return arrTmp;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_GETPK;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual void setProperty(string property, object newvalue)
        {
            try
            {
                int i = 0;
                bool isFound = false;
                if (_arrProperties != null)
                {
                    while (i < _arrProperties.Count)
                    {
                        if (_arrProperties[i].ToString().ToUpper() == property.ToUpper())
                        {
                            _arrProperties[i + 1] = newvalue;
                            i = _arrProperties.Count;
                            isFound = true;
                        }
                        i += 2;
                    }
                }
                else
                {
                    _arrProperties = new ArrayList();
                }
                if (!isFound)
                {
                    _arrProperties.Add(property);
                    _arrProperties.Add(newvalue);
                }
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                //return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_SETPROPERTY;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
            }
        }

        public virtual void setPK(string property, object newvalue)
        {
            try
            {
                int i = 0;
                bool isFound = false;
                if (_arrPK != null)
                {
                    while (i < _arrPK.Count)
                    {
                        if (_arrPK[i].ToString().ToUpper() == property.ToUpper())
                        {
                            i = _arrPK.Count;
                            isFound = true;
                        }
                        i += 1;
                    }
                }
                else
                {
                    _arrPK = new ArrayList();
                }
                if (!isFound)
                {
                    _arrPK.Add(property);
                }
                if (newvalue != null && newvalue.ToString() != "")
                    this.setProperty(property, newvalue);
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_SETPK;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
            }
        }

        public virtual void UnLoad()
        {
            try
            {
                int i = 0;
                ArrayList arrTmp = new ArrayList();
                while (i < _arrProperties.Count)
                {
                    if (isPKName(_arrProperties[i].ToString().ToUpper()))
                    {
                        arrTmp.Add(_arrProperties[i]);
                        arrTmp.Add(_arrProperties[i + 1]);
                    }
                    i += 2;
                }
                _arrProperties = arrTmp;

            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_UNLOAD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
            }
        }

        public virtual void Load()
        {
            try
            {
                string sqlStr = "SELECT * FROM " + _entityName + " WITH (NOLOCK)  WHERE 1<>0";
                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[arrPK.Count];
                sqlStr = "SELECT * FROM  " + _entityName + " WITH (NOLOCK)  ";
                int i = 0;
                int j = 0;
                while (j < rd.FieldCount)
                {
                    if (this.getProperty(rd.GetName(j)) != null && isPKName(rd.GetName(j).ToUpper()))
                    {
                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));
                        					
                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                //Retrieve default primary key fields
                if (this.arrPK == null || this.arrPK.Count == 0)
                {
                    string strSQL = "SELECT TAG FROM LANGUAGE WHERE PKFIELD='Y' AND ENTITY='" + _entityName + "'";
                    rd = dbManager.ExecuteReader(strSQL, CommandType.Text);
                    while (rd.Read())
                    {
                        if (arrPK == null)
                            arrPK = new ArrayList();
                        arrPK.Add(rd[0]);
                    }
                    rd.Close();
                }
                rd.Dispose();
                sqlStr += " WHERE " + getCommandParam();
                dbManager.CreateParameters(pmi);
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ArrayList arr = new ArrayList();
                while (rd.Read())
                {
                    isLoadOK = true;
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        arr.Add(rd.GetName(i));
                        arr.Add(rd.GetValue(i));
                    }
                }
                rd.Dispose();            
                _arrProperties = arr;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {                
                
                ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {                
                
                ErrorHandler.Process(ex);
            }
        }

        public virtual void ReLoad()
        {
            try
            {
                string sqlStr = "SELECT * FROM " + _entityName + " WITH (NOLOCK)  WHERE 1<>0";
                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[arrPK.Count];
                sqlStr = "SELECT * FROM  " + _entityName + " WITH (NOLOCK)  ";
                int i = 0;
                int j = 0;
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));

                    if (this.getProperty(rd.GetName(j)) != null && isPKName(rd.GetName(j).ToUpper()))
                    {
                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));

                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                //Retrieve default primary key fields
                if (this.arrPK == null || this.arrPK.Count == 0)
                {
                    string strSQL = "SELECT TAG FROM LANGUAGE WHERE PKFIELD='Y' AND ENTITY='" + _entityName + "'";
                    rd = dbManager.ExecuteReader(strSQL, CommandType.Text);
                    while (rd.Read())
                    {
                        if (arrPK == null)
                            arrPK = new ArrayList();
                        arrPK.Add(rd[0]);
                    }
                    rd.Close();
                }
                rd.Dispose();
                sqlStr += " WHERE " + getCommandParam();
                dbManager.CreateParameters(pmi);
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                arrData.Add(arrHeader);

                while (rd.Read())
                {                  
                    isLoadOK = true;
                    ArrayList arrDetail = new ArrayList();
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);

                }
                rd.Dispose();

                _arrProperties = arrData;                
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                
                ErrorHandler.Process(ex);                
            }
            catch (Exception ex)    
            {
                
                ErrorHandler.Process(ex);                
            }
        }

        public virtual ArrayList FetchNumberRecord(string cond, int fromRecord, int numberOfRecord, string orderBy)
        {
            try
            {
                string sqlStr = "SELECT TOP " + numberOfRecord + " FROM [" + _entityName + "] WITH (NOLOCK)";
                sqlStr += " WHERE " + orderBy + " NOT IN ( SELECT TOP " + fromRecord + " " + orderBy + " FROM [" + _entityName + "] WITH (NOLOCK) ORDER BY " + orderBy + " ASC )";
                if (!string.IsNullOrEmpty(cond))
                {
                    sqlStr += " AND " + cond;
                }
                sqlStr += " ORDER BY " + orderBy + " ASC AND 1<>0";

                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Close();
                rd.Dispose();
                //dbManager.Close();
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCHNUMBERRECORD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual int Update()
        {
            string strDebug = "";
            try
            {
                if (ValidateUpdate())
                {                    
                    string sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0";
                    IDataReader rd;
                    rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                    //Count parameter=column
                    int c = 0;
                    for (int k = 0; k < rd.FieldCount; k++)
                    {
                        if (this.getProperty(rd.GetName(k)) != null)
                            c++;
                    }
                    ParamStruct[] pmi = new ParamStruct[c];

                    sqlStr = "UPDATE [" + _entityName + "] SET ";
                    string strVal = "";
                    int i = 0;
                    int j = 0;
                    
                    string strValue = "";
                    string strSqlUpdate = "UPDATE  " + this._entityName + "  SET ";
                    string strSqlWhere = "";

                    while (j < rd.FieldCount)
                    {
                        if (this.getProperty(rd.GetName(j)) != null)
                        {
                            pmi[i].Direction = ParameterDirection.Input;
                            pmi[i].ParameterName = "@" + rd.GetName(j);
                            pmi[i].SourceColumn = rd.GetName(j);

                            if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            {
                                strValue = "";
                                pmi[i].Value = String.Empty;
                            }
                            else
                            {                                
                                strValue = SysUtils.CString(this.getProperty(rd.GetName(j)));
                                pmi[i].Value = strValue;
                            }
                            pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());

                            if (!isPKName(rd.GetName(j)))
                            {
                                strVal += "[" + rd.GetName(j) + "]=@" + rd.GetName(j) + ",";
                            }
                            else
                            {                                
                                strSqlWhere += "[" + pmi[i].ParameterName + "]=''" + strValue + "'' AND ";
                            }
                            strDebug += "<" + pmi[i].ParameterName + "=" + pmi[i].Value.ToString() + ">";
                            
                            strSqlUpdate += rd.GetName(j) + "='" + strValue + "',";

                            i += 1;
                        }
                        j += 1;
                    }
                    rd.Close();
                    sqlStr += strVal.Substring(0, strVal.Length - 1) + " WHERE " + getCommandParam();                               
                    strSqlUpdate = strSqlUpdate.TrimEnd(char.Parse(",")) + " WHERE " + strSqlWhere + "1=1 ";

                    //User log before execute
                    if (_listTableNameNoLog.IndexOf(_entityName + "/") == -1)
                        SystemLog("UPDATE", pmi, strSqlUpdate);

                    dbManager.CreateParameters(pmi);
                    int num_row = dbManager.ExecuteNonQuery(CommandType.Text, sqlStr);
                    
                    return num_row;
                }
                return -1;
            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ThrowError(er);
                return -1;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_UPDATE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return -1;
            }
        }        

        public virtual int Sort(string storeName)
        {
            try
            {
                ParamStruct[] pmi = new ParamStruct[arrProperties.Count / 2];
                for (int i = 0; i < pmi.Length; i++)
                {
                    pmi[i].DbType = DbType.Int16;
                    pmi[i].Direction = ParameterDirection.Input;
                    pmi[i].ParameterName = "@" + arrProperties[i * 2];
                    pmi[i].SourceColumn = arrProperties[i * 2].ToString();
                    pmi[i].Value = this.getProperty(arrProperties[i * 2].ToString());
                }

                dbManager.CreateParameters(pmi);
                int num_row = dbManager.ExecuteNonQuery(CommandType.StoredProcedure, storeName);
                //dbManager.Close();
                return num_row;
            }
            catch (ErrorMessage er)
            {                
                ErrorHandler.ThrowError(er);
                return -1;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_SORT;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return -1;
            }
        }

        public virtual ArrayList FetchAll(bool isOrdered, string orderBy)
        {
            try
            {
                //string ordered = "ORDER BY [" + orderBy + "]";
                string ordered = "ORDER BY " + orderBy ;
                string sqlStr = "";

                if (isOrdered)
                    sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) " + ordered;
                else
                    sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) ";
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Close();
                rd.Dispose();
                //dbManager.Close();
                return arrData;

            }
            catch (ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCHALL;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }     

        public virtual ArrayList FetchByField()
        {
            try
            {
                string strSql = "SELECT ";
                for (int i = 0; i < arrProperties.Count; i++)
                {
                    strSql += "[" + arrProperties[i].ToString() + "], ";
                }
                strSql = strSql.Substring(0, strSql.Length - 2) + " FROM [" + this.entityName + "] WITH (NOLOCK) ";

                ArrayList arrHeader = new ArrayList();
                ArrayList arrData = new ArrayList();
                IDataReader rd;
                rd = dbManager.ExecuteReader(strSql, CommandType.Text);
                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);
                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();
                //dbManager.Close();
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCHFIELD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual ArrayList FetchByFieldWithCondition()
        {
            try
            {
                ArrayList arrHeader = new ArrayList();
                ArrayList arrData = new ArrayList();
                IDataReader rd;

                string strSql = "SELECT ";
                for (int i = 0; i < arrProperties.Count; i++)
                {
                    strSql += "[" + arrProperties[i].ToString() + "], ";
                }
                strSql = strSql.Substring(0, strSql.Length - 2) + " FROM [" + this.entityName + "] WITH (NOLOCK) ";

                if (arrPK.Count > 0)
                {
                    rd = dbManager.ExecuteReader(strSql, CommandType.Text);

                    strSql += "WHERE ";
                    string cond = string.Empty;
                    for (int i = 0; i < arrPK.Count; i += 2)
                    {
                        cond += arrPK[i].ToString() + "=@" + arrPK[i].ToString() + " AND ";
                    }
                    strSql = strSql + cond.Substring(0, cond.Length - 5);

                    ParamStruct[] pmi = new ParamStruct[arrPK.Count / 2];
                    int index = 0;
                    for (int i = 0; i < arrPK.Count; i += 2)
                    {
                        pmi[index].Direction = ParameterDirection.Input;
                        pmi[index].ParameterName = arrPK[i].ToString();
                        pmi[index].Value = arrPK[i + 1].ToString();
                        index++;
                    }

                    while (rd.Read())
                    {
                        for (int i = 0; i < pmi.Length; i++)
                        {
                            for (int k = 0; k < rd.FieldCount; k++)
                            {
                                if (pmi[i].ParameterName.ToLower() == rd.GetName(k))
                                {
                                    pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(k).ToString());
                                }
                            }
                        }
                        break;
                    }
                    rd.Close();
                    dbManager.CreateParameters(pmi);
                }
                rd = dbManager.ExecuteReader(strSql, CommandType.Text);
                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);
                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();
                //dbManager.Close();
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCHFIELD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual ArrayList Fetch(bool isOrdered, string orderBy)
        {
            try
            {
                string ordered = " ORDER BY [" + orderBy + "]";
                string sqlStr = "";

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0 ";

                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[this.arrProperties.Count / 2];

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) ";

                int i = 0;
                int j = 0;
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    if (this.getProperty(rd.GetName(j)) != null)
                    {

                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "@" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));
                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                sqlStr += " WHERE " + getSearchParam();
                if (isOrdered)
                    sqlStr += ordered;

                dbManager.CreateParameters(pmi);
                arrData.Add(arrHeader);

                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                while (rd.Read())
                {
                    isLoadOK = true;
                    ArrayList arrDetail = new ArrayList();
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Close();
                rd.Dispose();
                
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual ArrayList Fetch(string orderBy)
        {
            try
            {
                string ordered = " ORDER BY " + orderBy ;
                string sqlStr = "";

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0 ";

                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[this.arrProperties.Count / 2];

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) ";

                int i = 0;
                int j = 0;
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    if (this.getProperty(rd.GetName(j)) != null)
                    {

                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "@" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));
                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                sqlStr += " WHERE " + getSearchParam();
                if (!string.IsNullOrEmpty(orderBy))
                    sqlStr += ordered;

                dbManager.CreateParameters(pmi);
                arrData.Add(arrHeader);

                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                while (rd.Read())
                {
                    isLoadOK = true;
                    ArrayList arrDetail = new ArrayList();
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Close();
                rd.Dispose();

                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }


        public virtual ArrayList Fetch(bool isOrdered)
        {
            try
            {
                string ordered = " ORDER BY [order]";
                string sqlStr = "";

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0 ";

                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[this.arrProperties.Count / 2];

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) ";

                int i = 0;
                int j = 0;
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    if (this.getProperty(rd.GetName(j)) != null)
                    {

                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "@" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));
                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                sqlStr += " WHERE " + getSearchParam();
                if (isOrdered)
                    sqlStr += ordered;

                dbManager.CreateParameters(pmi);
                arrData.Add(arrHeader);

                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                while (rd.Read())
                {
                    isLoadOK = true;
                    ArrayList arrDetail = new ArrayList();
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Close();
                rd.Dispose();
                //dbManager.Close();
                return arrData;
            }
            catch (ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual ArrayList SBFetch(bool isOrdered)
        {
            try
            {
                string ordered = " ORDER BY [pord]";
                string sqlStr = "";

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0 ";

                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                ParamStruct[] pmi = new ParamStruct[this.arrProperties.Count / 2];

                sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) ";

                int i = 0;
                int j = 0;
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    if (this.getProperty(rd.GetName(j)) != null)
                    {

                        pmi[i].Direction = ParameterDirection.Input;
                        pmi[i].ParameterName = "@" + rd.GetName(j);
                        pmi[i].SourceColumn = rd.GetName(j);

                        if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            pmi[i].Value = String.Empty;
                        else
                            pmi[i].Value = this.getProperty(rd.GetName(j));
                        pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                        i += 1;
                    }
                    j += 1;
                }

                rd.Close();
                sqlStr += " WHERE " + getSearchParam();
                if (isOrdered)
                    sqlStr += ordered;

                dbManager.CreateParameters(pmi);
                arrData.Add(arrHeader);

                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                while (rd.Read())
                {
                    isLoadOK = true;
                    ArrayList arrDetail = new ArrayList();
                    for (i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }
                rd.Dispose();
                //dbManager.Close();
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_SBFETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        #region getArrData

        public virtual ArrayList getArrData(string sqlStr)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();

                IDataReader rd;

                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }

                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();

                return arrData;

            }
            catch (ErrorMessage er)
            {
                
                ErrorHandler.ThrowError(er);
                return null;
            }
            catch (Exception ex)
            {
                
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.SRV_MAINTENANCE_PROCESS_ERR;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        #endregion getArrData

        public virtual ArrayList LoadByQuery()
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();

                IDataReader rd = dbManager.ExecuteReader(_retrieveDataQuery, CommandType.Text);

                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }

                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();

                return arrData;

            }
            catch (ErrorMessage er)
            {
                
                ErrorHandler.ThrowError(er);
                return null;
            }
            catch (Exception ex)
            {
                
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.SRV_MAINTENANCE_PROCESS_ERR;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public virtual int Add()
        {
            string strDebug = "";
            try
            {
                if (ValidateInsert())
                {
                    IDataReader rd;
                    //1. Automatically genarate Primary key field   
                    string sqlStr = "";
                    if (this.arrPK == null)
                    {
                        sqlStr = "SELECT TAG FROM LANGUAGE  WITH (NOLOCK) WHERE PKFIELD='Y' AND ENTITY='" + _entityName + "'";
                        rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                        while (rd.Read())
                        {
                            string key = rd[0].ToString().Trim();
                            string Val = Guid.NewGuid().ToString();
                            setProperty(key, Val);
                            setPK(key, Val);
                        }
                        rd.Close();
                    }

                    //2. Gen sql
                    sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0";
                   
                    rd = dbManager.ExecuteReader(CommandType.Text, sqlStr);            
                    int i = 0;
                    int j = 0;
                    while (j < rd.FieldCount)
                    {
                        if (this.getProperty(rd.GetName(j)) != null)
                        {
                            i += 1;
                        }
                        j += 1;
                    }
                    ParamStruct[] pmi = new ParamStruct[i];

                    sqlStr = "INSERT INTO [" + _entityName + "] (";
                    string strVal = "";
                    i = 0;
                    j = 0;
                    
                    string strValue = "";
                    string strSqlValue = "";

                    while (j < rd.FieldCount)
                    {
                        if (this.getProperty(rd.GetName(j)) != null)
                        {
                            pmi[i].Direction = ParameterDirection.Input;
                            pmi[i].ParameterName = "@" + rd.GetName(j);
                            pmi[i].SourceColumn = rd.GetName(j);

                            if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                            {                                
                                strValue = "";
                                pmi[i].Value = String.Empty;
                            }
                            else
                            {                                
                                strValue = SysUtils.CString(this.getProperty(rd.GetName(j)));
                                pmi[i].Value = strValue;
                            }
                            sqlStr += "[" + rd.GetName(j) + "], ";
                            strVal += "@" + rd.GetName(j) + ", ";                            
                            strSqlValue += "'" + strValue + "',";

                            strDebug += "<" + pmi[i].ParameterName + "=" + pmi[i].Value.ToString() + ">";
                            pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(j).ToString());
                            i += 1;
                        }
                        j += 1;
                    }
                    
                    strSqlValue = sqlStr.Substring(0, sqlStr.Length - 1) + ") VALUES (" + strSqlValue.Substring(0, strSqlValue.Length - 1) + ")";                    
                    sqlStr = sqlStr.Substring(0, sqlStr.Length - 2) + ") VALUES(" + strVal.Substring(0, strVal.Length - 2) + ")";
                    rd.Close();
                    rd.Dispose();

                    //User log before execute
                    if (_listTableNameNoLog.IndexOf(_entityName + "/") == -1)
                        SystemLog("INSERT", pmi, strSqlValue);

                    dbManager.CreateParameters(pmi);
                    int num_rows = (int)dbManager.ExecuteNonQuery(CommandType.Text, sqlStr);

                    return num_rows;
                }
                return -1;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return -1;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_ADD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return -1;
            }
        }

        private void SystemLog(string strAction, ParamStruct[] parm, string strSqlStatement)
        {
            try
            {
                //1. LOG SQL STATEMENT
                DateTime dtLog = DateTime.Now;
                string strINSERT = "INSERT INTO USERLOG (USERID,LOGTIME,SQLSTATEMENT)";
                strINSERT = strINSERT + " VALUES ('" + this._UserID + "','" + dtLog + "','" + strSqlStatement + "')";
                dbManager.ExecuteNonQuery(CommandType.Text, strINSERT);

                //3. LOG FIELDS DETAIL
                //SystemLog(strAction, parm);

                //2.Dongpv:22/04/2018
                //strSqlStatement = strSqlStatement.Replace("''", "'");
                //LogMessage.Log(strSqlStatement + "\n", "TRANCE_SQL");

            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
            }
            finally
            {
            }
        }

        public virtual int Delete()
        {
            try
            {
                if (ValidateDelete())
                {                    
                    string sqlStr = "SELECT * FROM [" + _entityName + "] WITH (NOLOCK) WHERE 1=0";
                    IDataReader rd;
                    rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);
                    ParamStruct[] pmi = new ParamStruct[arrPK.Count];
                    sqlStr = "DELETE FROM [" + _entityName + "] ";
                    int i = 0;
                    int j = 0;
                    while (j < rd.FieldCount)
                    {
                        if (this.getProperty(rd.GetName(j)) != null && isPKName(rd.GetName(j).ToUpper()))
                        {
                            pmi[i].Direction = ParameterDirection.Input;
                            pmi[i].ParameterName = "@" + rd.GetName(j);
                            pmi[i].SourceColumn = rd.GetName(j);

                            if ((this.getProperty(rd.GetName(j)) == null) && (pmi[j].DbType == DbType.String))
                                pmi[i].Value = String.Empty;
                            else
                                pmi[i].Value = this.getProperty(rd.GetName(j));                            					
                            i += 1;
                        }
                        j += 1;
                    }
                    rd.Close();
                    rd.Dispose();
                    sqlStr += " WHERE " + getCommandParam();

                    dbManager.CreateParameters(pmi);
                    int num_rows = (int)dbManager.ExecuteNonQuery(CommandType.Text, sqlStr);
                    
                    return num_rows;
                }
                return -1;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 
                ErrorHandler.ThrowError(ex);
                return -1;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_DELETE;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return -1;
            }
        }

        public bool isPKName(string property)
        {
            for (int i = 0; i < _arrPK.Count; i++)
            {
                if (_arrPK[i].ToString().ToUpper() == property.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }
        public string getCommandParam()
        {
            string st = "";
            for (int i = 0; i < _arrPK.Count; i++)
            {
                st += _arrPK[i].ToString().ToUpper() + "=@" + _arrPK[i].ToString().ToUpper() + " AND ";
            }
            st += "1=1 ";
            return st;
        }

        public string getSearchParam()
        {
            string st = "";
            for (int i = 0; i < this._arrProperties.Count; i += 2)
            {
                st += _arrProperties[i].ToString().ToUpper() + "=@" + _arrProperties[i].ToString().ToUpper() + " AND ";
            }
            st += "1=1 ";
            return st;
        }       


        public string getSearchLikeParam()
        {
            string st = "";
            for (int i = 0; i < this._arrProperties.Count; i += 2)
            {
                st += _arrProperties[i].ToString().ToUpper() + "LIKE '@" + _arrProperties[i].ToString().ToUpper() + "' AND ";
            }
            st += "1=1 ";
            return st;
        }

      
        public static ParamStruct SetDBType(ParamStruct pmi, string FieldType)
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
        public virtual bool ValidateUpdate()
        {
            try
            {
                return true;
            }
            catch (ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return false;
            }
        }
        public virtual bool ValidateInsert()
        {
            try
            {
                return true;
            }
            catch (ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return false;
            }
        }
        public virtual bool ValidateDelete()
        {
            try
            {
                return true;
            }
            catch (ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCH;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return false;
            }
        }

        //public static List<T> ExecuteProcedure<T>(ref WB.MESSAGE.Message msg)       
        public ArrayList ExecuteStoreProcedure(ArrayList arrListParms)
        {
            try
            {                            
                //2.QUIRY  DATA
                int num;
                ArrayList arrResult = new ArrayList();

                if (arrListParms.Count > 1)
                {
                    ParamStruct[] param = new ParamStruct[arrListParms.Count - 1];
                    //ParamStruct[] param = new ParamStruct[arrListParms.Count];
                    num = 0;
                    ArrayList arrPName = (ArrayList)arrListParms[0];

                    for (int i = 1; i < arrListParms.Count; i++)
                    {
                        ArrayList arrParmName = (ArrayList)arrListParms[i];
                        string PName = SysUtils.getProperty(arrPName, arrParmName, "PARAMETER_NAME");
                        string Dtype = SysUtils.getProperty(arrPName, arrParmName, "DATA_TYPE");
                        string MaxLen = SysUtils.getProperty(arrPName, arrParmName, "CHARACTER_MAXIMUM_LENGTH");
                        string ParmMode = SysUtils.getProperty(arrPName, arrParmName, "PARAMETER_MODE");

                        object PVal = this.getProperty(PName.Substring(1));

                        param[num].ParameterName = PName; //"@" + 
                        param[num].size = 1024;

                        string strVal = SysUtils.CString(PVal);
                        if (!string.IsNullOrEmpty(strVal))
                            param[num].Value = strVal;
                        else
                            param[num].Value = null;

                        switch (ParmMode)
                        {
                            case "OUTPUT":
                                param[num].Direction = ParameterDirection.Output;
                                break;
                            case "INOUT":
                                param[num].Direction = ParameterDirection.Output;
                                break;
                            case "RETURNVALUE":
                                param[num].Direction = ParameterDirection.ReturnValue;
                                break;
                            default:
                                param[num].Direction = ParameterDirection.Input;
                                break;
                        }

                        //switch (Dtype)
                        //{
                        //    case "datetime":
                        //        param[num].DbType = DbType.DateTime;
                        //        break;
                        //}

                        param[num] = dbManager.SetDBType(param[num], Dtype);
                        num++;
                    }

                    // return value
                    //param[num].Direction = ParameterDirection.ReturnValue;
                    //param[num].DbType = DbType.Int32;
                    //param[num].ParameterName = "@Returned";
                    dbManager.CreateParameters(param);
                }

                //Case:DataSet
                DataSet ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, this.entityName);
                arrResult = SysUtils.DataSet2ArrayList(ds, 0);
                            
                //3. GET RESULT             
                //msg.effectRows = dbManager.result;

                //if have any error throw exception 
                if (arrResult.Count > 1)
                {                    
                    string strErr_code = SysUtils.getProperty(arrResult, "ERR_CODE");
                    if (!string.IsNullOrEmpty(strErr_code))
                    {
                        ErrorMessage ex = new ErrorMessage();
                        ex.ErrorSource = SysUtils.getProperty(arrResult, "ENTITY");
                        ex.ErrorCode = strErr_code;
                        ex.ErrorDesc_vn = SysUtils.getProperty(arrResult, "ERR_CAPTION");
                        ex.ErrorDesc = SysUtils.getProperty(arrResult, "ERR_CAPTION");
                        ErrorHandler.ThrowError(ex);
                    }
                }

                return arrResult;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (ErrorMessage ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {                
            }
        }


        /// <summary>
        /// chay store tra ra ket qua la arraylist
        /// </summary>
        /// <param name="_arrPamameter"></param>
        /// <param name="_StoreName"></param>
        /// <returns></returns>
        public ArrayList ExecStoreProcedure(ArrayList _arrPamameter, string _StoreName)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();

                ParamStruct[] pmi = new ParamStruct[_arrPamameter.Count / 2];// /2 vi vd arrparam="fromdate","2018-08-22","todate","2018-087-23" co 4 item nen phai /2
                for (int i = 0; i < pmi.Length; i++)
                {
                    pmi[i].DbType = DbType.String;
                    pmi[i].Direction = ParameterDirection.Input;
                    pmi[i].ParameterName = "@" + _arrPamameter[i * 2];
                    pmi[i].SourceColumn = "@" + _arrPamameter[i * 2].ToString();
                    pmi[i].Value = SysUtils.CString(SysUtils.getValue(_arrPamameter, _arrPamameter[i * 2].ToString()));
                }

                dbManager.CreateParameters(pmi);

                IDataReader rd;
                string strStoreName = _StoreName;
                rd = dbManager.ExecuteReader(CommandType.StoredProcedure, strStoreName);

                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }

                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();

                return arrData;

            }
            catch (ErrorMessage er)
            {
                
                ErrorHandler.ThrowError(er);
                return null;
            }
            catch (Exception ex)
            {
                
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = "ExeStoreProcedure";
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        /// <summary>
        /// chuyen result tu store thanh arraylist
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public virtual ArrayList getArrDataStoreProcedure(string strStoreName, string SearchCondition, string SearchValue)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();

                ParamStruct[] pms = new ParamStruct[2];

                pms[0].DbType = System.Data.DbType.String;
                pms[0].Direction = System.Data.ParameterDirection.Input;
                pms[0].ParameterName = "@SearchCondition";
                pms[0].SourceColumn = "@SearchCondition";
                pms[0].Value = SearchCondition;

                pms[1].DbType = System.Data.DbType.String;
                pms[1].Direction = System.Data.ParameterDirection.Input;
                pms[1].ParameterName = "@SearchValue";
                pms[1].SourceColumn = "@SearchValue";
                pms[1].Value = SearchValue;

                dbManager.CreateParameters(pms);

                IDataReader rd;

                rd = dbManager.ExecuteReader(CommandType.StoredProcedure, strStoreName);

                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }

                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();

                return arrData;

            }
            catch (ErrorMessage er)
            {
                
                ErrorHandler.ThrowError(er);
                return null;
            }
            catch (Exception ex)
            {
                
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.SRV_MAINTENANCE_PROCESS_ERR;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        /// <summary>
        /// Tim theo dieu kien xem record co ton tai hay ko
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList FetchByFieldWithConditionExists()
        {
            try
            {
                ArrayList arrHeader = new ArrayList();
                ArrayList arrData = new ArrayList();
                IDataReader rd;

                string strSql = "SELECT * FROM [" + this.entityName + "] WITH (NOLOCK) ";

                if (arrPK.Count > 0)
                {
                    rd = dbManager.ExecuteReader(strSql, CommandType.Text);

                    strSql += "WHERE ";
                    string cond = string.Empty;
                    for (int i = 0; i < arrPK.Count; i += 2)
                    {
                        cond += arrPK[i].ToString() + "=@" + arrPK[i].ToString() + " AND ";
                    }
                    strSql = strSql + cond.Substring(0, cond.Length - 5);

                    ParamStruct[] pmi = new ParamStruct[arrPK.Count / 2];
                    int index = 0;
                    for (int i = 0; i < arrPK.Count; i += 2)
                    {
                        pmi[index].Direction = ParameterDirection.Input;
                        pmi[index].ParameterName = arrPK[i].ToString();
                        pmi[index].Value = arrPK[i + 1].ToString();
                        index++;
                    }

                    while (rd.Read())
                    {
                        for (int i = 0; i < pmi.Length; i++)
                        {
                            for (int k = 0; k < rd.FieldCount; k++)
                            {
                                if (pmi[i].ParameterName.ToLower() == rd.GetName(k))
                                {
                                    pmi[i] = dbManager.SetDBType(pmi[i], rd.GetDataTypeName(k).ToString());
                                }
                            }
                        }
                        break;
                    }
                    rd.Close();
                    dbManager.CreateParameters(pmi);
                }
                strSql = $"select case when exists ({strSql}) then 1 else 0 end";
                int iRowCount =Convert.ToInt32(dbManager.ExecuteScalar(CommandType.Text,strSql));
                
                arrData.Add("isExist");
                arrData.Add(iRowCount);
               
                //dbManager.Close();
                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                 ErrorHandler.ThrowError(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.BUSSINESS_ENTITY_FETCHFIELD;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                 ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

    }
}
