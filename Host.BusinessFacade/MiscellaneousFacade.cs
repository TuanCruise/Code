using System;
using System.Runtime.InteropServices;
////using  SystemHandler;
using System.Collections;
using System.Data;
using System.Configuration;
//using WB.BusinessBase;

//using WB.HostDAL;
using System.IO;
using System.Collections.Generic;

using WB.SYSTEM;
using Host.DataBaseAccessService;
using Host.BusinessBase;
using WB.MESSAGE;

namespace Host.BusinessFacade
{
    public class MiscellaneousFacade
    {
        private DBManager _dbManager;

        public DBManager dbManager
        {
            get { return _dbManager; }
            set { _dbManager = value; }
        }
       

        public MiscellaneousFacade()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
            }
        }

        public void Process(ref Message msg)
        {
            dbManager = new DBManager();            

            try
            {
                dbManager.Open();

                if (msg.ObjectName.ToUpper() == WB.SYSTEM.Constants.OBJ_SEARCH)
                {
                    GetSearch(ref msg);
                }
                else if (msg.ObjectName.ToUpper() == Constants.OBJ_PROCEDURE_PAGING)
                {
                    msg.Body = this.LoadDataByProcdure(ref msg);
                }
                else
                {
                    msg.Body = GetStoreQuery(msg.Body, msg.ObjectName);
                }
             }
            catch (ErrorMessage ex)
            {
                ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
            }
            finally
            {
            }
        }

        private ArrayList LoadDataByProcdure(ref WB.MESSAGE.Message msg)
        {
            try
            {
                string strProName = SysUtils.CString(SysUtils.getValue(msg.Body, "ENTITY"));
                ArrayList arrParm = (ArrayList)(SysUtils.getValue(msg.Body, "PARMVAL"));

                //1.GET PARAMS
                BusinessEntity ent = new BusinessEntity();
                ent.dbManager = this.dbManager; 
                ent.retrieveDataQuery = "SELECT PARAMETER_NAME ,DATA_TYPE ,CHARACTER_MAXIMUM_LENGTH , PARAMETER_MODE, ORDINAL_POSITION FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = '" + strProName + "'";
                ArrayList arrListParms = ent.LoadByQuery();
                //ArrayList arrListParms = ent.arrProperties;

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

                        string PVal = SysUtils.getProperty(arrParm, PName.Substring(1));

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

                        param[num] = this.dbManager.SetDBType(param[num], Dtype);

                        num++;
                    }

                    // return value
                    //param[num].Direction = ParameterDirection.ReturnValue;
                    //param[num].DbType = DbType.Int32;
                    //param[num].ParameterName = "@Returned";

                    this.dbManager.CreateParameters(param);
                }
              
                DataSet ds = this.dbManager.ExecuteDataSet(CommandType.StoredProcedure, strProName);

                //3. GET RESULT
                arrResult = SysUtils.DataSet2ArrayList(ds, 0);
                msg.effectRows = this.dbManager.totalRows;

                //if have any error throw exception 
                if (arrResult.Count > 1)
                {
                    //if (!string.IsNullOrEmpty(this.dbManager.Err_code))
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
            catch (ErrorMessage ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void GetSearch(ref Message msg)
        {
            int effRows = 0;
            IDataReader rd;
            
            try
            {
                //Get columns to search
                string valType = msg.getValue("SearchObject").ToString();
                string cond = msg.getValue("Condition").ToString();
                int intPage = Convert.ToInt16(msg.getValue("Page").ToString());
                string strSQL = "";
                strSQL = "select tag,[order]  from language where entity='" + valType.ToUpper().Trim() + "' and search='Y' order by seq ";
                rd = dbManager.ExecuteReader(strSQL, CommandType.Text);
                strSQL = "select ";
                //strSQL="select ";
                string strOrder = "";
                while (rd.Read())
                {
                    if (rd.GetValue(0).ToString().ToUpper().Trim() == "CUSTOMERNAME")
                    {
                        strSQL += "b.FULLNAME " + rd.GetValue(0).ToString().Trim() + ",";
                        if (rd.GetValue(1).ToString().ToUpper() == "Y")
                        {
                            strOrder += "b.FULLNAME " + ",";
                        }
                    }
                    else if (rd.GetValue(0).ToString().ToUpper().Trim() == "TELLERNAME")
                    {
                        strSQL += "userprofile.FULLNAME " + rd.GetValue(0).ToString().Trim() + ",";
                        if (rd.GetValue(1).ToString().ToUpper().ToUpper() == "Y")
                        {
                            strOrder += "userprofile.FULLNAME " + ",";
                        }
                    }
                    else
                    {
                        strSQL += "a." + rd.GetValue(0).ToString().ToUpper().Trim() + ",";
                        if (rd.GetValue(1).ToString().ToUpper() == "Y")
                        {
                            strOrder += "a." + rd.GetValue(0).ToString().ToUpper().Trim() + ",";
                        }
                    }
                }

                rd.Close();
                rd.Dispose();

                if (strSQL == "select ")
                {
                    strSQL = "select * ";
                }

                strSQL = strSQL.Substring(0, strSQL.Length - 1) + " from " + valType.ToUpper() + " a ";
                string strCIFJoin = ",LNAPPLICATION,LNACCOUNT,FDACCOUNT,SCACCOUNT,CLACCOUNT,FNACCOUNT,MMACCOUNT,AAFACILITY";

                string strTellerJoin = ",TXNJOURNAL";
                string strTellerHistJoin = ",TXNHIST";
                if (strCIFJoin.IndexOf(valType.ToUpper()) != -1)
                {
                    strSQL += ", customer b ";
                    //cond = cond.Replace("FULLNAME", "b.FULLNAME");
                    cond = cond.Replace("CUSTOMERNAME", "b.FULLNAME");

                    cond = cond.Replace("CIFID", "a.CIFID");
                    cond = cond.Replace("REMARK", "a.REMARK");
                    cond = cond.Replace("UDF1", "a.UDF1");
                    cond = cond.Replace("UDF2", "a.UDF2");
                    cond = cond.Replace("UDF3", "a.UDF3");
                    cond = cond.Replace("UDF4", "a.UDF4");
                    cond = cond.Replace("UDF5", "a.UDF5");
                    strSQL += cond + " and a.cifid=b.cifid ";
                }
                else if (strTellerJoin.IndexOf(valType.ToUpper()) != -1 || strTellerHistJoin.IndexOf(valType.ToUpper()) != -1)
                {
                    strSQL += ", userprofile  ";
                    cond = cond.Replace("FULLNAME", "a.FULLNAME");
                    cond = cond.Replace("TELLERNAME", "userprofile.FULLNAME");

                    strSQL += cond + " and trim(a.tellerid)=trim(userprofile.userid) ";
                }
                else
                {
                    strSQL += cond;
                }
                if (strOrder != "")
                {
                    strOrder = strOrder.Substring(0, strOrder.Length - 1);
                    strSQL += " order by " + strOrder;

                }
                if (valType.ToUpper() == "EXRATE")
                {
                    strSQL = "select distinct rateseq,lastdate,remark from exrate order by rateseq desc";
                }

                //Return search result 
                int fromRow = intPage * WB.SYSTEM.Constants.MAX_ROW;
                int toRow = (intPage + 1) * WB.SYSTEM.Constants.MAX_ROW - 1;
                ArrayList arrValue = new ArrayList();
                rd = dbManager.ExecuteReader(strSQL, CommandType.Text);
                bool firstRow = true;
                int currRow = 0;

                if (rd != null && rd.FieldCount > 0)
                {
                    ArrayList arrHeader = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        arrHeader.Add(rd.GetName(i).ToString().ToUpper());
                    }
                    arrValue.Add(arrHeader);
                    firstRow = false;
                }

                while (rd.Read())
                {
                    effRows += 1;
                    if (currRow >= fromRow && currRow <= toRow)
                    {
                        ArrayList arrRow = new ArrayList();
                        if (firstRow)
                        {
                            for (int i = 0; i < rd.FieldCount; i++)
                            {
                                arrRow.Add(rd.GetName(i).ToString().ToUpper());
                            }
                            arrValue.Add(arrRow);
                            arrRow = new ArrayList();
                            firstRow = false;
                        }
                        for (int i = 0; i < rd.FieldCount; i++)
                        {
                            arrRow.Add(rd.GetValue(i).ToString().Trim());
                        }
                        arrValue.Add(arrRow);
                    }
                    currRow += 1;
                }
                rd.Close();
                rd.Dispose();
                msg.Body = arrValue;
                msg.effectRows = effRows;

            }
            catch (ErrorMessage ex)
            {
                ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {
               ErrorHandler.Process(ex);
            }
        }


        private ArrayList GetStoreQuery(ArrayList arrParm, string strProName)
        {
            try
            {
                //1.GET PARAMS
                ArrayList arrListParms = GetStoreParam(strProName);

                //2.QUIRY  DATA
                int num;
                ArrayList arrData = new ArrayList();

                if (arrListParms.Count > 1)
                {
                    ParamStruct[] param = new ParamStruct[arrListParms.Count - 1];
                    num = 0;
                    ArrayList arrPName = (ArrayList)arrListParms[0];

                    for (int i = 1; i < arrListParms.Count; i++)
                    {
                        ArrayList arrParmName = (ArrayList)arrListParms[i];
                        string PName = SysUtils.getProperty(arrPName, arrParmName, "PARAM");
                        string PVal = SysUtils.CString(SysUtils.getValue(arrParm, PName));
                        param[num].Direction = ParameterDirection.Input;
                        param[num].ParameterName = "@" + PName;
                        param[num].Value = SysUtils.CString(PVal);
                        num++;
                    }
                    this.dbManager.CreateParameters(param);
                }

                IDataReader reader = this.dbManager.ExecuteReader(strProName, CommandType.StoredProcedure);

                //3. GET RESULT
                bool flag = true;
                while (reader.Read())
                {
                    ArrayList arrTemp = new ArrayList();
                    if (flag)
                    {
                        num = 0;
                        while (num < reader.FieldCount)
                        {
                            arrTemp.Add(reader.GetName(num).ToString());
                            num++;
                        }
                        arrData.Add(arrTemp);
                        arrTemp = new ArrayList();
                        flag = false;
                    }
                    for (num = 0; num < reader.FieldCount; num++)
                    {
                        arrTemp.Add(reader.GetValue(num).ToString());
                    }
                    arrData.Add(arrTemp);
                }

                reader.Close();
                reader.Dispose();
                return arrData;
            }
            catch (ErrorMessage message)
            {
                ErrorHandler.Process(message);
                return null;
            }
            catch (Exception exception)
            {
                ErrorHandler.Process(exception);
                return null;
            }
        }

        private ArrayList GetStoreParam(string procedureName)
        {
            try
            {

                ArrayList arrResult = new ArrayList();
                ParamStruct[] param = new ParamStruct[1];
                param[0].Direction = ParameterDirection.Input;
                param[0].ParameterName = "@objname";
                param[0].size = 0x80;
                param[0].Value = procedureName;
                this.dbManager.CreateParameters(param);
                try
                {
                    DataTable table = this.dbManager.ExecuteDataSet(CommandType.StoredProcedure, "sp_help").Tables[1];
                    ArrayList arrData = new ArrayList();
                    arrData.Add("PARAM");
                    arrData.Add("TYPE");
                    arrData.Add("LENGTH");
                    arrData.Add("ORDER");
                    arrData.Add("VALUE");
                    arrResult.Add(arrData);

                    foreach (DataRow row in table.Rows)
                    {
                        ArrayList arrDetail = new ArrayList();
                        arrDetail.Add(row[0].ToString().Substring(1));
                        arrDetail.Add(row[1].ToString());
                        arrDetail.Add(row[2].ToString());
                        arrDetail.Add(row[3].ToString());
                        arrDetail.Add("");
                        arrResult.Add(arrDetail);
                    }
                }
                catch
                {
                }
                return arrResult;
            }
            catch (ErrorMessage ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

    }
}
