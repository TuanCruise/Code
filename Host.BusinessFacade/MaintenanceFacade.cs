using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;

using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using IdentityGuardAuthServiceV3API;
using WB.MESSAGE;
using WB.SYSTEM;
using WB.SystemLibrary;
using Host.BusinessBase;
using Host.DataBaseAccessService;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Host.BusinessFacade
{
	public class MaintenanceFacade
	{
        private DBManager _dbManager;

        public DBManager dbManager
        {
            get { return _dbManager; }
            set { _dbManager = value; }
        }

        public MaintenanceFacade()
		{
            ErrorMessage objErr = new ErrorMessage();
            try
            {              
            }
            catch (Exception ex)
            {                
                objErr.ErrorCode = ErrorHandler.SRV_CONNECT_ERR;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
            }
            finally
            {
                objErr.Dispose();
            }
        }


        public void Process(ref Message msg)
        {
            ErrorMessage objErr = new ErrorMessage();
            dbManager = new DBManager();
            try
            {                
                //var appSettingsSection = IConfiguration.GetSection("AppSettings");
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted; 
                transactionOptions.Timeout = new TimeSpan(10, 0, 0);

                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    try
                    {
                        dbManager.Open();

                        if (msg.ObjectName.ToUpper() == WB.SYSTEM.Constants.OBJ_MNT_ROLE_PERMISSION)
                        {

                        }
                        else if (msg.ObjectName.ToUpper() == WB.SYSTEM.Constants.OBJ_EXECUTESTOREPROCEDURE) //Kiem tra: Goi store hay sinh cau lenh sql
                        {
                            BusinessEntity ent = new BusinessEntity();
                            ent.dbManager = dbManager;

                            //List objects
                            ArrayList arrayLists = new ArrayList();

                            ArrayList arrObj = msg.Body;
                            ArrayList arrDetail = new ArrayList();
                            ArrayList arrHeader = new ArrayList();
                            ArrayList arrData = new ArrayList();

                            string strObjName = "";
                            
                            for (int i = 0; i < arrObj.Count; i += 2)
                            {
                                strObjName = arrObj[i].ToString();
                                arrData = (ArrayList)arrObj[i + 1];

                                //Exec foreach store
                                arrHeader = (ArrayList)arrData[0];
                                for (int j = 1; j < arrData.Count; j++)
                                {
                                    arrDetail = (ArrayList)arrData[i + 1];

                                    ent.arrProperties = SysUtils.Property2Value(arrDetail, arrHeader);
                                    ent.entityName = strObjName;

                                    //1.GET PARAMS of store                                                               
                                    ent.retrieveDataQuery = "SELECT PARAMETER_NAME ,DATA_TYPE ,CHARACTER_MAXIMUM_LENGTH , PARAMETER_MODE, ORDINAL_POSITION FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = '" + strObjName + "'";
                                    ArrayList arrListParms = ent.LoadByQuery();

                                    //Execute
                                    arrDetail = ent.ExecuteStoreProcedure(arrListParms);

                                    if (ent.arrPK == null)
                                        ent.arrPK = arrDetail;

                                    //Reture
                                    arrayLists.Add(strObjName);
                                    arrayLists.Add(arrDetail);
                                }
                            }

                            msg.Body = arrayLists;

                        }
                        else  //defaul: gen sql statement
                        {
                            BusinessEntity ent = new BusinessEntity();
                            ent.dbManager = dbManager;
                            //1.SAVE MASTER                           
                            ent.arrProperties = msg.Body;
                            ent.entityName = msg.ObjectName;

                            if (msg.MsgAction == WB.SYSTEM.Constants.MSG_ADD_ACTION)
                            {
                                ent.Add();
                            }
                            if (msg.MsgAction == WB.SYSTEM.Constants.MSG_UPDATE_ACTION)
                            {
                                ent.Update();
                            }
                            if (msg.MsgAction == WB.SYSTEM.Constants.MSG_DELETE_ACTION)
                            {
                                ent.Delete();
                            }

                            //SAVE PKey for Master to insert properties of Detail
                            ArrayList arrMasterPkey = ent.arrPK;
                            ArrayList arrMasterProperties = ent.arrProperties;

                            //2.UPDATE DETAIL
                            //2.1LOAD ALL CTRLTYPE =GE -->Grid
                            ent.arrProperties = new ArrayList();
                            ent.arrProperties.Add("MODID");
                            ent.arrProperties.Add(msg.ModId);
                            ent.arrProperties.Add("CTRLTYPE");
                            ent.arrProperties.Add("GE");
                            ent.entityName = "DEFMODFLD";
                            ent.arrPK = ent.arrProperties;                            
                            ArrayList arrDefModFld = ent.Fetch("");

                            //2.2 SAVE DETAIL                           
                            if (arrDefModFld.Count > 0)
                            {                                
                                ArrayList arrHeader = (ArrayList)arrDefModFld[0];
                                ArrayList arrDetail = new ArrayList();

                                for (int i = 1; i < arrDefModFld.Count; i++)
                                {
                                    arrDetail = (ArrayList)arrDefModFld[i];
                                    string entity = SysUtils.getProperty(arrHeader, arrDetail, "ENTITY");
                                    ent.entityName = entity;

                                    string fldName = SysUtils.getProperty(arrHeader, arrDetail, "FLDNAME");                                   
                                    string jsonValue = SysUtils.CString(SysUtils.getValue(msg.Body, fldName));
                                    if (string.IsNullOrEmpty(jsonValue)) break;

                                    //2.2.1 Convert jsonValue to Table or List<Model>
                                    //JArray arrBody = JArray.Parse(jsonValue);
                                    var table = JsonConvert.DeserializeObject<DataTable>(jsonValue);
                                    ArrayList arrData = SysUtils.DataTable2ArrayList(table);
                                 
                                    if (msg.MsgAction == WB.SYSTEM.Constants.MSG_ADD_ACTION)
                                    {
                                        //2.2.2 Save
                                        for (int j = 1; j < arrData.Count; j++)
                                        {
                                            ent.arrProperties = SysUtils.Property2Value( (ArrayList)arrData[j], (ArrayList)arrData[0]);
                                            ent.arrPK = new ArrayList();

                                            //2.2.2.1 Update FKey from Pkey of Master
                                            for (int k =0; k < arrMasterPkey.Count; k++ )
                                            {
                                                string strPkey = arrMasterPkey[k].ToString();
                                                ent.setProperty(strPkey, SysUtils.getValue(arrMasterProperties, strPkey));                                                
                                            }

                                            ent.Add();
                                        }
                                    }
                                    if (msg.MsgAction == WB.SYSTEM.Constants.MSG_UPDATE_ACTION)
                                    {
                                        //2.2.2.1 Delete by FKey
                                        ent.arrProperties = arrMasterProperties;
                                        ent.arrPK = arrMasterPkey;
                                        ent.Delete();

                                        //2.2.2.2 Update
                                        for (int j = 1; j < arrData.Count; j++)
                                        {
                                            ent.arrProperties = SysUtils.Property2Value((ArrayList)arrData[0], (ArrayList)arrData[j]);
                                            //2.2.2.1 Update FKey from Pkey of Master
                                            for (int k = 0; k < arrMasterPkey.Count; k++)
                                            {
                                                string strPkey = arrMasterPkey[k].ToString();
                                                ent.setProperty(strPkey, ent.getProperty(strPkey));
                                            }

                                            ent.Update();
                                        }

                                    }
                                    if (msg.MsgAction == WB.SYSTEM.Constants.MSG_DELETE_ACTION)
                                    {
                                        //2.2.2.1 Delete by FKey
                                        ent.arrProperties = arrMasterProperties;
                                        ent.arrPK = arrMasterPkey;
                                        ent.Delete();
                                    }
                                }
                            }


                            msg.Body = ent.getPKProperties();
                        }
                        
                        transaction.Complete();

                    }
                    catch (Exception ex)
                    {                      
                        transaction.Dispose();
                        throw ex;
                    }
                }

            }
            catch (ErrorMessage er)
            {
                               
                ErrorHandler.ProcessErr(er);                
            }
            catch (Exception ex)
            {
                            
                ErrorHandler.ProcessErr(ex, Constants.ERROR_TYPE_EBANK, ErrorHandler.EBANK_SYSTEM_ERROR);
            }
            finally
            {
                try
                {
                    dbManager.Close();
                    dbManager.Dispose();
                }
                catch
                {
                }
                objErr.Dispose();
            }
        }
        

    }
}
