using System;

namespace WB.SYSTEM
{
    public class Constants
    {
        public static int MAX_ROW = 40;
        
        //MESSAGE TYPE
        public static int MSG_MNT_TYPE = 0;
        public static int MSG_TXN_TYPE = 1;
        public static int MSG_MISC_TYPE = 2;
        public static int MSG_SYSTEM_TYPE = 4;
        public static int MSG_EBANK_TYPE = 5;
        public static int MSG_SBANK_TYPE = 6;
        public static int MSG_SMS_TYPE = 7;
        public static int MSG_REPORT_TYPE = 8;
        public static int MSG_iSPRINT_TYPE = 9;
        public static int MSG_EMAIL_TYPE = 10;
        public static int MSG_VNPAY_TYPE = 11;  
	    public static int MSG_VNPAY_PAYMENT_TYPE = 12;
        public static int MSG_MISCELL_TYPE = 13;
        public static int MSG_MBBANK_TYPE = 15;

        //ACTION
        public static string FETCH_ALL = "1";
        public static string FETCH = "2";
        public static string INSERT = "3";
        public static string UPDATE = "4";
        public static string DELETE = "5";
        public static string SORT = "6";

        public static int MSG_ADD_ACTION = 0;
        public static int MSG_UPDATE_ACTION = 1;
        public static int MSG_DELETE_ACTION = 2;
        public static int MSG_JOURNAL_ACTION = 3;
        public static int MSG_POST_ACTION = 4;
        public static int MSG_APPROVE_ACTION = 5;
        public static int MSG_REJECT_ACTION = 6;
        public static int MSG_SEARCH = 8;


        //LIST OBJECT NAME:
        public static string OBJ_MNT_ROLE_PERMISSION = "ROLEPERMISSION";
        public static string OBJ_MNT_USER = "USERPROFILE";
        public static string OBJ_MNT_CUSTOMER = "CUSTOMER";
        public static string OBJ_SEARCH = "OBJ_SEARCH";
        public static string OBJ_SQLQUERY = "SQLQUERY";

        public static string OBJ_PROCEDURE_PAGING = "PROCEDURE_PAGING";
        public static string OBJ_EXECUTESTOREPROCEDURE = "EXECUTESTOREPROCEDURE";

        public static string ERROR_TYPE_EBANK = "EBANK";
        public static string ERROR_TYPE_SMARTBANK = "SMARTBANK";
        public static string ERROR_TYPE_ISPRINT = "ISPRINT";
        public static string ERROR_TYPE_GSM = "GSM";                
        public static string ERROR_TYPE_VNP = "VNP";       
        public static string ERROR_TYPE_eBANKWEBUI = "eBANK_WEBUI";
        

        //ALERT CODE:
        public static string AL_OVER_USER_LIMIT = "AL_OVER_USER_LIMIT";
        public static string AL_INVALID_SIGN_STATUS = "AL_INVALID_SIGN_STATUS";
        public static string AL_RESCTRICT_ACC = "AL_RESCTRICT_ACC";
        public static string AL_SUCCESS_MSG = "AL_SUCCESS_MSG";
        public static string AL_STANDING_MSG = "AL_STANDING_MSG";
        public static string AL_REJECTED_MSG = "AL_REJECTED_MSG";
       
      
        //MODULEID
        public static string MID_LOGIN = "SYS004";
        public static string MID_LST_BRANCH= "SYS005";
        public static string MID_LST_BILL_PROVIDER = "SYS006"; 
     

        private Constants()
        { }
    }

}
