using System;
using System.IO;
using System.Web;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace WB.SYSTEM
{
	/// <summary>
	/// Summary description for ErrorHandler.
	/// </summary>
    [Serializable]
	public class ErrorMessage:System.Exception
    {
        #region Prorperties

        //HueMT 05.10 add 2 rows
        public string ErrorSystemCode = "";
        public string ErrorSystemDes = "";
        public string ErrorCode = "";
        public string ErrorDesc = "";
        public string InnerErrorDesc = "";

        //HueMT 05.10 add 1 rows
        public string ErrorDesc_vn = "";
        public string ErrorSource = "";
        public string ErrorType = "";
        public string ErrorTrace = "";
        public bool isBLLError = false;
        //HueMT 05.10 add 1 rows
        public bool isWritelog = false;  
  
		public ErrorMessage()
		{
			ErrorCode="";
			ErrorDesc="";
            InnerErrorDesc = "";
			ErrorSource="";
			ErrorType="SYSTEM_ERROR";
            ErrorTrace = "";
			isBLLError=false;            
        }
        #endregion

        #region IDisposable Members

        private IntPtr handle;
    
        private Component component = new Component();

        private bool disposed = false;

        public ErrorMessage(IntPtr handle)
        {
            this.handle = handle;
        }

        public void Dispose()
        {
            Dispose(true);           
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {               
                if (disposing)
                {
                    component.Dispose();
                }                
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }       
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        ~ErrorMessage()
        {           
            Dispose(false);
        }

        #endregion
	}

	public class ErrorHandler
	{
        //Khai bao loi SB o day
        //HueMT 12.10
        public const string SB_DD_NOTFOUND = "-212";

        //End Khai bao loi SB

		public const string CF_NOTEXITCUSTOMER="CF00001";
		public const string CF_DUPLICATECUSTOMER="CF00002";
		// Common Error 00501 - 01000
		public const string CM_NOTEXITGLGRP="CM00501";
		// DD ErrorCode
		public const string DD_NOTFOUND="DD00001";
		public const string DD_EXISTS="DD00003";
		public const string DD_BALANCENOTENOUGHT="DD00002";
		// FD ErrorCode
		public const string FD_NOTFOUND="FD00001";
		public const string FD_EXISTS="FD00003";
		public const string FD_BALANCENOTENOUGHT="FD00002";
		// GL Error
		public const string GL_DEBITNOTEQUALCREDIT="GL00001";
		// txnum da ton tai trong bang GLTRAN
		public const string GL_TXNUMISEXISTS="GL00002";
		public const string GL_ACCOUNTNOTEXISTS="GL00003";
		public const string GL_ACCOUNTISEXISTS="GL00004";
		// CL Error
		public const string CL_EXISTS="CL00001";	
		public const string TXN_DEF_ERR="TXN00001";

        public const string BUSSINESS_ENTITY_UPDATE = "E00001";
        public const string BUSSINESS_ENTITY_SORT = "E00002";
        public const string BUSSINESS_ENTITY_FETCHALL = "E00003";
        public const string BUSSINESS_ENTITY_FETCHFIELD = "E00004";
        public const string BUSSINESS_ENTITY_FETCH = "E00005";
        public const string BUSSINESS_ENTITY_SBFETCH = "E00006";
        public const string BUSSINESS_ENTITY_ADD = "E00007";
        public const string BUSSINESS_ENTITY_DELETE = "E00008";
        public const string BUSSINESS_ENTITY_GETPROPERTY = "E00009";
        public const string BUSSINESS_ENTITY_GETPK = "E00010";
        public const string BUSSINESS_ENTITY_SETPROPERTY = "E00011";
        public const string BUSSINESS_ENTITY_SETPK = "E00012";
        public const string BUSSINESS_ENTITY_UNLOAD = "E00013";
        public const string BUSSINESS_CONTRACT_CHANGEPASSWORD = "E00014";

        // EBANK Error
        //HueMT 05.10 add 1 row
        public const string EBANK_SYSTEM_ERROR = "EB00000";
        public const string EBANK_LANGNOEXISTS = "EB00001";
        public const string EBANK_LOAD_ACCTYPE_ERR = "EB00002";
        public const string EBANK_CHANGE_PASS_FAIL = "EB00003";
        public const string EBANK_INS_FEEDBACK = "EB00004";
        public const string EBANK_LOAD_LNSCHD_TYPE = "EB00005";
        public const string EBANK_NOUSER_EXISTS = "EB00006";
        public const string EBANK_GEN_KEY = "EB00007";
        public const string EBANK_SEND_SMS_FAIL = "EB00008";
        public const string EBANK_AUTHENTICATE_FAIL = "EB00009";
        public const string EBANK_LOAD_USER_SERVICE = "EB00010";
        public const string EBANK_MENU_ERR = "EB00011";
        public const string EBANK_VIEW_TRANSFER_DETAIL = "EB00012";
        public const string EBANK_VIEW_LNPAYMENT_DETAIL = "EB00013";
        public const string EBANK_PAGING_ERR = "EB00014";
        public const string EBANK_UPDATE_LOG = "EB00015";
        public const string EBANK_DISABLE_ACC = "EB00016";
        public const string EBANK_UPDATE_LAST_LOGGED = "EB00017";
        public const string EBANK_LIST_ACCOUNTS = "EB00018";
        public const string EBANK_VALIDATE_INP_DATA = "EB00019";
        public const string EBANK_ORDER_MESSAGE = "EB00020";
        public const string EBANK_ALL_CODE = "EB00021";        

        public const string EBANK_USERNAME_EXISTS = "EB00022";
        public const string EBANK_OTP_USED = "EB00023";
        public const string EBANK_CUSTID_UNAVAILABLE = "EB00024";
        public const string EBANK_UPDATE_SIGN_GROUP_INVALID = "EB00025";
        public const string EBANK_OTPSERIAL_NOTFOUND = "EB00026";
        public const string EBANK_UNSIGNED_CITAD_MSG_FOUND = "EB00027";
        public const string EBANK_OTPSERIAL_EXISTS = "EB00028";
        public const string EBANK_BRANCH_CODE_NOT_MATCH = "EB00029";
        public const string EBANK_YOUR_VERIFICATION_CODE = "EB00030";
        public const string EBANK_VALIDATE_OPM = "EB00031";
        public const string EBANK_ACCESS_DENIED = "EB00032";
        public const string EBANK_INVALID_RESTRICT_ACC = "EB00033";
        public const string EBANK_NOT_SAME_CUSTID = "EB00034";
        public const string EBANK_ACCTNO_NOT_EXIST = "EB00036";
        public const string EBANK_DEBIT_ACC_CCY_INVALID = "EB00037";
        public const string EBANK_CREDIT_ACC_CCY_INVALID = "EB00038";
        public const string EBANK_INVALID_RES_ACC_NAME = "EB00039";
        public const string EBANK_INVALID_BEN_NAME = "EB00040";
        public const string EBANK_INVALID_SERV_NAME = "EB00041";
        public const string EBANK_INVALID_TELCO_NAME = "EB00042"; 
        public const string EBANK_NO_DATA = "EB00043";

        // Client Caller Error
        public const string CLS_ACC_STATEMENT_DD = "CLS00001";
        public const string CLS_ACC_STATEMENT_FD = "CLS00002";
        public const string CLS_ACC_STATEMENT_LN = "CLS00003";
        public const string CLS_BAL_INQUIRY_DD = "CLS00004";
        public const string CLS_FUND_TRANSFER = "CLS00005";
        public const string CLS_BAL_INQUIRY_FD = "CLS00006";
        public const string CLS_BAL_INQUIRY_LN = "CLS00007";
        public const string CLS_REVERT_FUND_TRANSFER = "CLS0008";
        public const string CLS_FILL_LOAN_PAYMENT = "CLS00009";
        public const string CLS_LOAD_INTLNSCHD = "CLS00010";
        public const string CLS_LOAN_INTCALC = "CLS00011";
        public const string CLS_LOAN_PAYMENT = "CLS00012";
        public const string CLS_LOAD_PRILNSCHD = "CLS00014";
        public const string CLS_LOAD_LNSCHD = "CLS00015";
        public const string CLS_FETCH_BANK_ACC = "CLS00016";
        public const string CLS_INS_LOG = "CLS00017";
        public const string CLS_INS_LNRPT = "CLS00018";
        public const string CLS_ADD_ACC_STATEMENT = "CLS00019";
        public const string CLS_DEL_REPORT = "CLS00020";
        public const string CLS_INS_SLIPRPT = "CLS00021";
        public const string CLS_DEL_BANKST_DD = "CLS00022";
        public const string CLS_DEL_BANKST_FDLN = "CLS00023";        
        public const string CLS_DEL_RPTSLIP = "CLS00024";
        public const string CLS_DEL_LNRPT = "CLS00025";
        public const string CLS_REMITTANCE_DD = "CLS00026";

        // Router Err
        public const string ROUTER_ERR = "RT00001";
        
        // Report Err
        public const string RPT_DEL_TABLE = "RP00001";
        public const string RPT_INS_EXPRPT = "RP00002";        

        // Server Err
        public const string SRV_CONNECT_ERR = "SRV00001";
        public const string SRV_MAINTENANCE_PROCESS_ERR = "SRV00002";
        public const string SRV_SBCOM_ERR = "SRV00003";        
        public const string SRV_GSM_SEND_ERR = "SRV00004";
        public const string SRV_GSM_DEL_ERR = "SRV00005";
        public const string SRV_GSM_RESET_ERR = "SRV00006";
        public const string SRV_RPT_LNSCHD_ERR = "SRV00007";
        public const string SRV_RPT_INS_RESULT_EXP_ERR = "SRV00008";
        public const string SRV_RPT_DEL_RESULT_EXP_ERR = "SRV00009";
        public const string SRV_RPT_INS_BANKST_DD_ERR = "SRV00010";
        public const string SRV_RPT_INS_BANKST_FDLN_ERR = "SRV00011";
        public const string SRV_RPT_DEL_BANKST_DD_ERR = "SRV00012";
        public const string SRV_RPT_DEL_BANKST_FDLN_ERR = "SRV00013";
        public const string SRV_RPT_INS_SLIP_ERR = "SRV00014";
        public const string SRV_RPT_DEL_SLIP_ERR = "SRV00015";
        public const string SRV_RPT_ERR = "SRV00016";        
        public const string PROXY_ERR = "SRV00017";
        public const string CORE_DOTRAN_DD_ERR = "SRV00018";
        public const string CORE_DOTRAN_FD_ERR = "SRV00019";
        public const string CORE_DOTRAN_LNSCHD_ERR = "SRV00020";
        public const string CORE_DOTRAN_LNINTCALC_ERR = "SRV00021";
        public const string CORE_DOTRAN_LNREFILL_ERR = "SRV00022";
        public const string CORE_DOTRAN_LNPAYMENT_ERR = "SRV00023";
        public const string CORE_DOTRAN_LNOTHER_ERR = "SRV00024";
        public const string CORE_LISTACC_ERR = "SRV00025";
        public const string CORE_SBANK_ERR = "SRV00026";
        public const string CORE_FUND_TRANS_ERR = "SRV00027";
        public const string CORE_TRANSACT_ERR = "SRV00028";
        public const string SRV_RBANK_PROCESS_ERR = "SRV00029";
        public const string CORE_AMOUNT_NEGATIVE = "SRV00030";
        public const string SRV_TIME_OUT_SESSION = "SRV00031";
        public const string SRV_NO_ANY_ROWS = "SRV00032";
        public const string SRV_INVALID_FORMAT_DES = "SRV00033";
        public const string SRV_INVALID_FOR_BRCH_NAME = "SRV00034";
        public const string SRV_INVALID_BENEFICIARY_NAME = "SRV00035";
        public const string SRV_NOTIFY_BLOCKACC = "SRV00036";
        public const string SRV_INVALID_PASSWORD = "SRV00037";
        public const string SRV_INVALID_USERNAME = "SRV00038";
        public const string SRV_INVALID_VERIFY_CODE = "SRV00039";
        public const string SRV_INVALID_ACC_STATUS = "SRV00040";
        public const string SRV_INVALID_SM_ACC_STATUS = "SRV00041";
        public const string SRV_NOT_AVAILABLE_BAL = "SRV00042";
        public const string SRV_INVALID_PHONENO = "SRV00043";
        //FROM VNPAY:
        public const string VNP_RESPONSE_ERRCODE = "VNP00094";

        //FOR SMARTLINK GATEWAY:
        public const string SML_DEFAULT_ERRCODE = "54"; 

        public const string ATM_GATEWAY_ERR = "ATM00001";
        public const string BUSSINESS_ENTITY_FETCHNUMBERRECORD = "E00014";
        //Procedure:
        public const string SP_GET_PROVINCE_LIST = "SP00001";
        public const string SP_GET_MSG_ORD_LIST = "SP00002";

        //iSPRINT
        public const string ISP_VERIFY_PASSWORD_ERR = "ISP00001";

        public static String strPath = "C:\\logs\\";


        public const string BOL_BRANCH_CHECKEXIST = "B00001";
        public const string BOL_CONTRACT_RESETPASS = "B00002";
        public const string BOL_CONTRACT_CHECKEXIST = "B00003";
        public const string BOL_CONTRACT_SEARCH = "B00004";
        public const string BOL_SERVICE_CHECKEXIST = "B00005";
        public const string BOL_MODULE_CHECKEXIST = "B00006";
        public const string BOL_PERMISSION_CHECKEXIST = "B00007";
        public const string BOL_ROLE_CHECKEXIST = "B00008";
        public const string BOL_USER_CHECKEXIST = "B00009";
        public const string BOL_USER_SEARCH = "B00010";
        public const string BOL_ENTRUST_ERR = "B00011";

        public const string WEB_ADDCUSTOMER_LOADSERVICEINFO = "W00001";
        public const string WEB_ADDCUSTOMER_LOADAUTHTYPES = "W00002";
        public const string WEB_BINDINGDATA = "W00003";
        public const string WEB_ADDCUSTOMER_SETEBANKNO = "W00004";
        public const string WEB_ISEXIST = "W00005";
        public const string WEB_PAGING = "W00006";
        public const string WEB_GETCOMMENTLIST = "W00007";
        public const string WEB_LOADCUSTOMERLIST = "W00008";
        public const string WEB_LOADROLELIST = "W00009";
        public const string WEB_LOADBRANCHLIST = "W00010";
        public const string WEB_LOADMODULELIST = "W00011";
        public const string WEB_LOADPERMISSIONLIST = "W00012";
        public const string LOADSERVICELIST = "W00013";
        public const string WEB_LOADUSERLIST = "W00014";

        public const string CLSCALLER_LOADACTIVESERVICEINFO = "C00001";
        public const string CLSCALLER_CHECKEXISTCONTRACTINFO = "C00002";
        public const string CLSCALLER_INSERTCONSTRACT = "C00003";
        public const string CLSCALLER_FETCHCONTRACTBYCUSTID = "C00004";
        public const string CLSCALLER_INSERTCONSTRACTDETAIL = "C00005";
        public const string CLSCALLER_INSERTSERVICE = "C00006";
        public const string CLSCALLER_LOADALLCUSTOMERLIST = "C00007";
        public const string CLSCALLER_DELETECONTRACT = "C00008";
        public const string CLSCALLER_DELETECONTRACTDETAIL = "C00009";
        public const string CLSCALLER_FETCHCONTRACTDETAILBYEBNKNO = "C00010";
        public const string CLSCALLER_UPDATECONSTRACT = "C00011";
        public const string CLSCALLER_UPDATEAPPROVECONTRACT = "C00012";
        public const string CLSCALLER_UPDATEBLOCKCONTRACT = "C00013";
        public const string CLSCALLER_UPDATESTATUSCONTRACT = "C00014";
        public const string CLSCALLER_UPDATEPASSWORDSENT = "C00015";
        public const string CLSCALLER_UPDATECONSTRACTDETAIL = "C00016";
        public const string CLSCALLER_RESETPASSWORDCONTRACT = "C00017";
        public const string CLSCALLER_LOADALLSERVICEINFO = "C00018";
        public const string CLSCALLER_DELETESERVICE = "C00019";
        public const string CLSCALLER_FETCHSERVICEBYID = "C00020";
        public const string CLSCALLER_UPDATESERVICE = "C00021";
        public const string CLSCALLER_LOADCOMMENTLIST = "C00022";
        public const string CLSCALLER_GETCOMMENTBYID = "C00023";
        public const string CLSCALLER_UPDATESTATUS = "C00024";
        public const string CLSCALLER_DELETECOMMENT = "C00025";
        public const string CLSCALLER_FETCHNEWCOMMENT = "C00026";
        public const string CLSCALLER_CHECKEXISTSERVICE = "C00027";
        public const string CLSCALLER_DELETEROLE = "C00028";
        public const string CLSCALLER_FETCHROLEBYID = "C00029";
        public const string CLSCALLER_INSERTROLE = "C00030";
        public const string CLSCALLER_UPDATEROLE = "C00031";
        public const string CLSCALLER_CHECKEXISTROLE = "C00032";
        public const string CLSCALLER_LOADALLROLELIST = "C00033";
        public const string CLSCALLER_LOADALLMODULELIST = "C00034";
        public const string CLSCALLER_DELETEMODULE = "C00035";
        public const string CLSCALLER_FETCHMODULEBYID = "C00036";
        public const string CLSCALLER_INSERTMODULE = "C00037";
        public const string CLSCALLER_UPDATEMODULE = "C00038";
        public const string CLSCALLER_CHECKEXISTMODULE = "C00039";
        public const string CLSCALLER_LOADALLPERMISSIONLIST = "C00040";
        public const string CLSCALLER_DELETEPERMISSION = "C00041";
        public const string CLSCALLER_FETCHPERMISSIONBYID = "C00042";
        public const string CLSCALLER_INSERTPERMISSION = "C00043";
        public const string CLSCALLER_UPDATEPERMISSION = "C00044";
        public const string CLSCALLER_CHECKEXISTPERMISSION = "C00045";
        public const string CLSCALLER_LOADACTIVEMODULELIST = "C00046";
        public const string CLSCALLER_LOADACTIVEPERMISSIONLIST = "C00047";
        public const string CLSCALLER_LOADACTIVEROLELIST = "C00048";
        public const string CLSCALLER_CHECKPERMISSONROLEMODULE = "C00049";
        public const string CLSCALLER_DELETEALLROLEINMODULE = "C00050";
        public const string CLSCALLER_INSERTROLEPERMSSIONMODULE = "C00051";
        public const string CLSCALLER_DELETEBRANCH = "C00052";
        public const string CLSCALLER_FETCHBRANCHBYID = "C00053";
        public const string CLSCALLER_INSERTBRANCH = "C00054";
        public const string CLSCALLER_UPDATEBRANCH = "C00055";
        public const string CLSCALLER_CHECKEXISTBRANCH = "C00056";
        public const string CLSCALLER_LOADALLBRANCHLIST = "C00057";
        public const string CLSCALLER_LOADALLUSERLIST = "C00058";
        public const string CLSCALLER_LOADACTIVEBRANCHLIST = "C00059";
        public const string CLSCALLER_DELETEUSER = "C00060";
        public const string CLSCALLER_FETCHUSERBYID = "C00061";
        public const string CLSCALLER_INSERTUSER = "C00062";
        public const string CLSCALLER_UPDATEUSER = "C00063";
        public const string CLSCALLER_CHECKEXISTUSER = "C00064";
        public const string CLSCALLER_FETCHUSERBYUSERNAMEANDPASSWORD = "C00065";
        public const string CLSCALLER_CHANGEPASSWORD = "C00066";
        public const string CLSCALLER_FECTHPERMISSIONMODULEBYROLEID = "C00067";
        public const string CLSCALLER_UPDATEUSERLOGGED = "C00068";
        public const string CLSCALLER_FETCHCONTRACTSEARCH = "C00069";
        public const string CLSCALLER_FETCHUSERSEARCH = "C00070";
        public const string CLSCALLER_LOADAUTHTYPES = "C00071";
        public const string CLSCALLER_INSERTRESTRICTACCOUNT = "C00072";
        public const string CLSCALLER_FETCHRESTRICTTRANSFERBYEBNKNO = "C00073";
        public const string CLSCALLER_DELETERESTRICTTRANSFER = "C00074";
        public const string CLSCALLER_SENDSMSCODE = "C00075";
        public const string CLSCALLER_FETCHCONTRACTBYEBANKNO = "C00076";
        public const string CLSCALLER_FETCHALLEBANKNO = "C00077";
        public const string CLSCALLER_DELETEROLEPERMISSIONMODULE = "C00078"; 

		public ErrorHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void ThrowError(Exception ex)
		{

            StreamWriter myStreamWriter = null;

            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";

                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();
                throw ex;
            }
            
		}
        public static void ThrowError(WB.SYSTEM.ErrorMessage ex)
        {
            StreamWriter myStreamWriter = null;

            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";

                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();
                throw ex;
            }
        }
        public static void ThrowError(System.Reflection.TargetInvocationException Invex)
        {
            WB.SYSTEM.ErrorMessage objErr = new ErrorMessage();
            System.Runtime.InteropServices.COMException comex = (System.Runtime.InteropServices.COMException)Invex.InnerException;
            objErr.ErrorCode = comex.ErrorCode.ToString(); ;
            objErr.ErrorDesc = comex.Message;
            objErr.ErrorSource = comex.Source;
            ErrorHandler.ThrowError(objErr);

        }
		/// <summary>
		/// Output Error message
		/// </summary>
		/// <param name="ex">Error message</param>
		public static void Process(WB.SYSTEM.ErrorMessage ex)
		{
            StreamWriter myStreamWriter = null;
            
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR"  + ".LOG";

                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Source :" + ex.ErrorSource);
                myStreamWriter.WriteLine("Type :" + ex.ErrorType);
                myStreamWriter.WriteLine("Code :" + ex.ErrorCode);                
                myStreamWriter.WriteLine("Description :" + ex.ErrorDesc);
                myStreamWriter.WriteLine("StackTrace :" + ex.StackTrace.ToString());
                myStreamWriter.WriteLine("ORGTrace :" + ex.ErrorTrace);  
                myStreamWriter.WriteLine("----------------------------------------------------------------------------------------------------------------");
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();                
            }
		}

		public static void Process(WB.SYSTEM.ErrorMessage ex,string strSource)
		{
            StreamWriter myStreamWriter = null;            
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";
                ex.ErrorSource = ex.ErrorSource + "\\" + strSource;
                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Source :" + ex.ErrorSource);
                myStreamWriter.WriteLine("Type :" + ex.ErrorType);
                myStreamWriter.WriteLine("Code :" + ex.ErrorCode);                
                myStreamWriter.WriteLine("Description :" + ex.ErrorDesc);
                myStreamWriter.WriteLine("StackTrace :" + ex.StackTrace.ToString());
                myStreamWriter.WriteLine("ORGTrace :" + ex.ErrorTrace);  
                myStreamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();                
            }
		}

        public static void ThrowError(string errorCode, string strCaption)
        {
            string errorMessage_vn = string.Empty;
            string errorMessage_en = string.Empty;

            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\AlertCode.xml";

                DataSet dsErrorCode = new DataSet();
                dsErrorCode.ReadXml(filePath);

                if (errorCode != "")
                {
                    DataTable dataTable = dsErrorCode.Tables["ALERTCODE"];
                    DataRow[] dtRows = dataTable.Select("ALERTCD='" + errorCode + "'");

                    if (dtRows.GetLength(0) > 0)
                    {
                        System.Data.DataRow rowDetail = dtRows[0];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ENCAPTION")
                            {
                                errorMessage_en += " ["  + strCaption  + "] " + rowDetail[i].ToString();
                            }
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "VNCAPTION")
                            {
                                errorMessage_vn += " [" + strCaption + "] " + rowDetail[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage_vn += "Lỗi không xác định!";
                        errorMessage_en += "Undefined error!";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                WB.SYSTEM.ErrorMessage ex = new WB.SYSTEM.ErrorMessage();
                ex.ErrorType = "USER_DEFINED";
                ex.ErrorCode = errorCode;
                ex.ErrorDesc = errorMessage_en;
                ex.ErrorDesc_vn = errorMessage_vn;
                ex.Source = "UNDEFINED";
                ThrowError(ex);
            }
        }

        public static void ThrowError(string errorCode)
        {
            string errorMessage_vn = string.Empty;
            string errorMessage_en = string.Empty;

            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\AlertCode.xml";

                DataSet dsErrorCode = new DataSet();
                dsErrorCode.ReadXml(filePath);

                if (errorCode != "")
                {
                    DataTable dataTable = dsErrorCode.Tables["ALERTCODE"];
                    DataRow[] dtRows = dataTable.Select("ALERTCD='" + errorCode + "'");

                    if (dtRows.GetLength(0) > 0)
                    {
                        System.Data.DataRow rowDetail = dtRows[0];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ENCAPTION")
                            {
                                errorMessage_en += rowDetail[i].ToString();
                            }
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "VNCAPTION")
                            {
                                errorMessage_vn += rowDetail[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage_vn += "Lỗi không xác định!";
                        errorMessage_en += "Undefined error!";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                WB.SYSTEM.ErrorMessage ex = new WB.SYSTEM.ErrorMessage();
                ex.ErrorType = "USER_DEFINED";
                ex.ErrorCode = errorCode;
                ex.ErrorDesc = errorMessage_en;
                ex.ErrorDesc_vn = errorMessage_vn;
                ex.Source = "UNDEFINED";
                ThrowError(ex);
            }
        }

        public static void ThrowException(string errorCode)
        {
            string errorMessage_vn = string.Empty;
            string errorMessage_en = string.Empty;

            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";

                DataSet dsErrorCode = new DataSet();
                dsErrorCode.ReadXml(filePath);

                if (errorCode != "")
                {
                    DataTable dataTable = dsErrorCode.Tables["Ebank"];
                    DataRow[] dtRows = dataTable.Select("ErrorCode='" + errorCode + "'");

                    if (dtRows.GetLength(0) > 0)
                    {
                        System.Data.DataRow rowDetail = dtRows[0];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_EN")
                            {
                                errorMessage_en += rowDetail[i].ToString();
                            }
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_VN")
                            {
                                errorMessage_vn += rowDetail[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage_vn += "Lỗi không xác định!";
                        errorMessage_en += "Undefined error!";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                WB.SYSTEM.ErrorMessage ex = new WB.SYSTEM.ErrorMessage();
                ex.ErrorType = "USER_DEFINED";
                ex.ErrorCode = errorCode;
                ex.ErrorDesc = errorMessage_en;
                ex.ErrorDesc_vn = errorMessage_vn;
                ex.Source = "UNDEFINED";
                ThrowError(ex);
            }
        }

        public static void ThrowException(string errorCode, string strCaption)
        {
            string errorMessage_vn = string.Empty;
            string errorMessage_en = string.Empty;

            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";

                DataSet dsErrorCode = new DataSet();
                dsErrorCode.ReadXml(filePath);

                if (errorCode != "")
                {
                    DataTable dataTable = dsErrorCode.Tables["Ebank"];
                    DataRow[] dtRows = dataTable.Select("ErrorCode='" + errorCode + "'");

                    if (dtRows.GetLength(0) > 0)
                    {
                        System.Data.DataRow rowDetail = dtRows[0];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_EN")
                            {
                                errorMessage_en += " [" + strCaption + "] " + rowDetail[i].ToString();
                            }
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_VN")
                            {
                                errorMessage_vn += " [" + strCaption + "] " + rowDetail[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage_vn += "Lỗi không xác định!";
                        errorMessage_en += "Undefined error!";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                WB.SYSTEM.ErrorMessage ex = new WB.SYSTEM.ErrorMessage();
                ex.ErrorType = "USER_DEFINED";
                ex.ErrorCode = errorCode;
                ex.ErrorDesc = errorMessage_en;
                ex.ErrorDesc_vn = errorMessage_vn;
                ex.Source = "UNDEFINED";
                ThrowError(ex);
            }
        }

        public static void ShowException(string errorCode, string strCaption, ref WB.SYSTEM.ErrorMessage ex)
        {
            string errorMessage_vn = string.Empty;
            string errorMessage_en = string.Empty;

            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";

                DataSet dsErrorCode = new DataSet();
                dsErrorCode.ReadXml(filePath);

                if (errorCode != "")
                {
                    DataTable dataTable = dsErrorCode.Tables["Ebank"];
                    DataRow[] dtRows = dataTable.Select("ErrorCode='" + errorCode + "'");

                    if (dtRows.GetLength(0) > 0)
                    {
                        System.Data.DataRow rowDetail = dtRows[0];
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_EN")
                            {
                                errorMessage_en += " [" + strCaption + "] " + rowDetail[i].ToString();
                            }
                            if (dataTable.Columns[i].ColumnName.ToUpper() == "ERRORMSG_VN")
                            {
                                errorMessage_vn += " [" + strCaption + "] " + rowDetail[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        errorMessage_vn += "Lỗi không xác định!";
                        errorMessage_en += "Undefined error!";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                ex.ErrorType = "USER_DEFINED";
                ex.ErrorCode = errorCode;
                ex.ErrorDesc = errorMessage_en;
                ex.ErrorDesc_vn = errorMessage_vn;
                ex.Source = "UNDEFINED";
            }
        }

        public static void ThrowErrMsg(string errDesc)
        {
            string errorMessage_vn = errDesc;
            string errorMessage_en = errDesc;

            WB.SYSTEM.ErrorMessage ex = new WB.SYSTEM.ErrorMessage();
            ex.ErrorType = "USER_DEFINED";
            ex.ErrorCode = "999";
            ex.ErrorDesc = errorMessage_en;
            ex.ErrorDesc_vn = errorMessage_vn;
            ex.Source = "UNDEFINED";

            ThrowError(ex);
        }

		public static void Process(Exception ex)
		{
            StreamWriter myStreamWriter = null;            
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";
                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Source : Unknown source");
                myStreamWriter.WriteLine("Type : Runtime error");
                myStreamWriter.WriteLine("Code : Undefined");               
                //28-10-2010
                //if (ex.InnerException.Message != null)
                //{
                //    myStreamWriter.WriteLine("Inner description :" + ex.InnerException.Message);
                //}

                myStreamWriter.WriteLine("Description :" + ex.Message);
                myStreamWriter.WriteLine("StackTrace :" + ex.StackTrace.ToString());
                myStreamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();
              
            }
		}

		public static void Process(Exception ex,string strSource)
		{
            StreamWriter myStreamWriter = null;            
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";

                if (Directory.Exists(strPath))
                {
                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(ex.Message + "\n" + ex.StackTrace);

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Source : " + strSource);
                myStreamWriter.WriteLine("Type : Runtime error");
                myStreamWriter.WriteLine("Code : Undefined");
                myStreamWriter.WriteLine("Description :" + ex.Message);
                myStreamWriter.WriteLine("StackTrace :" + ex.StackTrace.ToString());
                myStreamWriter.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();

                //WB.SYSTEM.ErrorMessage wbex = new ErrorMessage();
                //wbex.ErrorDesc = ex.Message;
                //wbex.ErrorSource = strSource;
                //wbex.ErrorType = "SYSTEM_ERROR";
                //throw wbex;
            }
		}

        public static void WirteTrace(string strEx)
        {
            StreamWriter myStreamWriter = null;
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_TRACE" + ".LOG";
                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                myStreamWriter.WriteLine(strEx);
                myStreamWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------------");
                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();

            }
        }

        //HueMT 05.10, from here to the end of file
        public static void WriteLog(ErrorMessage objErr)
        {
            StreamWriter myStreamWriter = null;
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";

                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Type :" + objErr.ErrorType);
                myStreamWriter.WriteLine("User Error Code :" + objErr.ErrorCode);
                myStreamWriter.WriteLine("Description :" + objErr.ErrorDesc);
                //28-10-2010
                myStreamWriter.WriteLine("Inner description :" + objErr.InnerErrorDesc);
                myStreamWriter.WriteLine("Source :" + objErr.ErrorSource);
                myStreamWriter.WriteLine("System Code :" + objErr.ErrorSystemCode);
                myStreamWriter.WriteLine("System Desc :" + objErr.ErrorSystemDes);
                myStreamWriter.WriteLine("Trace :" + objErr.ErrorTrace);
                myStreamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------");

                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();
            }
        }

        public static void WriteLog(string strErrorCode, string strErrorType,string strErrMsg_vn,string strErrMsg_en)
        {
            StreamWriter myStreamWriter = null;
            String strFilename = "";

            try
            {
                strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANKING_ERROR" + ".LOG";

                if (Directory.Exists(strPath))
                {

                }
                else
                {
                    Directory.CreateDirectory(strPath);
                }
                if (!File.Exists(strPath + strFilename))
                {
                    myStreamWriter = File.CreateText(strPath + strFilename);
                }
                else
                {
                    myStreamWriter = File.AppendText(strPath + strFilename);
                }
                myStreamWriter.WriteLine(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                myStreamWriter.Write(myStreamWriter.NewLine);
                myStreamWriter.WriteLine("Type :" + strErrorType);
                myStreamWriter.WriteLine("User Error Code :" + strErrorCode);
                myStreamWriter.WriteLine("Description :" + strErrMsg_vn);
                //28-10-2010
                myStreamWriter.WriteLine("Inner description :" );
                myStreamWriter.WriteLine("Source :" );
                myStreamWriter.WriteLine("System Code :" );
                myStreamWriter.WriteLine("System Desc :" + strErrMsg_en);
                myStreamWriter.WriteLine("Trace :" );
                myStreamWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------");

                myStreamWriter.Write(myStreamWriter.NewLine);
            }
            catch
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                myStreamWriter.Flush();
                myStreamWriter.Close();
            }
        }

        //This function does not throw error, just returns error description according to the error code
        public static void ProcessErr(Exception ex, string strErrType, string strErrCode, ref ErrorMessage objErr)
        {
            try
            {
                string strErrMsg_vn = string.Empty;
                string strErrMsg_en = string.Empty;
                SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                objErr.ErrorSource = ex.Source;
                objErr.ErrorSystemCode = ex.Source;
                objErr.ErrorSystemDes = ex.Message;
                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = ex.StackTrace;
                objErr.ErrorType = strErrType;

                if (!objErr.isWritelog)
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }
            }
            catch
            {
            }
            finally
            {

            }
        }

        public static void ProcessErr(Exception ex, string strErrType, string strErrCode)
        {
            ErrorMessage objErr = new ErrorMessage();
            try
            {
                string strErrMsg_vn = string.Empty;
                string strErrMsg_en = string.Empty;
                SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                objErr.ErrorSource = ex.Source;
                objErr.ErrorSystemCode = ex.Source;
                objErr.ErrorSystemDes = ex.Message;
                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = ex.StackTrace;
                objErr.ErrorType = strErrType;
                //objErr.InnerErrorDesc = ex.InnerException.Message; 

                WriteLog(objErr);
                objErr.isWritelog = true;
                if (!objErr.isWritelog) //unnecessary
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }
            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }

        public static void ProcessErr(ErrorMessage objErr)
        {
            try
            {
                if (!objErr.isWritelog) //unnecessary
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }
            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }

        public static void ProcessErr(string strErrCode, string strErrType)
        {

            ErrorMessage objErr = new ErrorMessage();
            try
            {
                string strErrMsg_vn = string.Empty;
                string strErrMsg_en = string.Empty;

                SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);

                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = new StackFrame(1).GetMethod().ReflectedType.FullName + "." + new StackFrame(1).GetMethod().Name;
                objErr.ErrorType = strErrType;
                if (!objErr.isWritelog) //unnecessary, always write log
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }
            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }

        public static void ProcessErr(string strErrCode, string strErrType, string strErrMsg_vn,string strErrMsg_en, string strDesValue)
        {

            ErrorMessage objErr = new ErrorMessage();
            try
            {
                //string strErrMsg_vn = string.Empty;
                //string strErrMsg_en = string.Empty;
                if (string.IsNullOrEmpty(strErrMsg_vn) & string.IsNullOrEmpty(strErrMsg_en))
                {
                    SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                }              

                objErr.ErrorCode = strErrCode;

                if (strDesValue == null)
                {
                    objErr.ErrorDesc = strErrMsg_en;
                    objErr.ErrorDesc_vn = strErrMsg_vn;
                }
                else
                {
                    //objErr.ErrorDesc = "[" + strDesValue + "] " + strErrMsg_en;
                    //objErr.ErrorDesc_vn = "[" + strDesValue + "] " + strErrMsg_vn;
                    objErr.ErrorDesc =  strErrMsg_en;
                    objErr.ErrorDesc_vn = strErrMsg_vn;
                }

                objErr.ErrorTrace = new StackFrame(1).GetMethod().ReflectedType.FullName + "." + new StackFrame(1).GetMethod().Name;
                objErr.ErrorType = strErrType;
                if (!objErr.isWritelog) //unnecessary, always write log
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }
            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }
       
        public static void ProcessErr(System.Reflection.TargetInvocationException Invex, string strErrType, string strErrCode)
        {

            ErrorMessage objErr = new ErrorMessage();
            try
            {
                string strErrMsg_vn = string.Empty;
                string strErrMsg_en = string.Empty;

                SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                System.Runtime.InteropServices.COMException comex = (System.Runtime.InteropServices.COMException)Invex.InnerException;
                objErr.ErrorSystemCode = comex.ErrorCode.ToString(); ;
                objErr.ErrorSystemDes = Invex.Message;
                objErr.Source = Invex.Source;
                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = Invex.StackTrace;
                objErr.ErrorType = strErrType;
                if (!objErr.isWritelog)//unnecessary, always write log
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }

            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }

        public static void ProcessErr(System.Reflection.TargetInvocationException Invex, string strErrType, string strErrCode, string strErrMsg_vn, string strErrMsg_en)
        {
            ErrorMessage objErr = new ErrorMessage();
            try
            {
                if (string.IsNullOrEmpty(strErrMsg_vn) & string.IsNullOrEmpty(strErrMsg_en))
                {
                    SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                }

                System.Runtime.InteropServices.COMException comex = (System.Runtime.InteropServices.COMException)Invex.InnerException;
                objErr.ErrorSystemCode = comex.ErrorCode.ToString(); ;
                objErr.ErrorSystemDes = Invex.Message;
                objErr.Source = Invex.Source;
                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = Invex.StackTrace;
                objErr.ErrorType = strErrType;
                if (!objErr.isWritelog)//unnecessary, always write log
                {
                    WriteLog(objErr);
                    objErr.isWritelog = true;
                }

            }
            catch
            {
            }
            finally
            {
                throw objErr;
            }
        }

        public static void WriteErr(Exception ex, string strErrType, string strErrCode)
        {
            ErrorMessage objErr = new ErrorMessage();
            try
            {
                string strErrMsg_vn = string.Empty;
                string strErrMsg_en = string.Empty;
                SysUtils.GetErrMsg(strErrCode, ref strErrMsg_en, ref strErrMsg_vn);
                objErr.ErrorSource = ex.Source;
                objErr.ErrorSystemCode = ex.Source;
                objErr.ErrorSystemDes = ex.Message;
                objErr.ErrorCode = strErrCode;
                objErr.ErrorDesc = strErrMsg_en;
                objErr.ErrorDesc_vn = strErrMsg_vn;
                objErr.ErrorTrace = ex.StackTrace;
                objErr.ErrorType = strErrType;
                //objErr.InnerErrorDesc = ex.InnerException.Message; 

                WriteLog(objErr);
                objErr.isWritelog = true;
               
            }
            catch
            {
            }
            finally
            {

            }
        }     

	}
}
