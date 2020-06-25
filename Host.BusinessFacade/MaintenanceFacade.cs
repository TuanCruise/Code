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
                        else
                        {
                            BusinessEntity ent = new BusinessEntity();
                            ent.dbManager = dbManager;
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

        #region BatchProcess

 

       
      
        #endregion
        
    }
}
