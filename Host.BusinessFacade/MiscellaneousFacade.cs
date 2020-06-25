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
        private DBManager _dbManagerRead;

        public DBManager dbManagerRead
        {
            get { return _dbManagerRead; }
            set { _dbManagerRead = value; }
        }

        private DBManager _dbManagerWrite;
        public DBManager dbManagerWrite
        {
            get { return _dbManagerWrite; }
            set { _dbManagerWrite = value; }
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
            dbManagerRead = new DBManager();
            dbManagerWrite = new DBManager();

            try
            {
                dbManager.Open();
                dbManagerRead.Open();
                dbManagerWrite.Open();

                if (msg.ObjectName.ToUpper() == WB.SYSTEM.Constants.OBJ_MNT_CUSTOMER)
                {

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
