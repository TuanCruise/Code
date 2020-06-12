namespace WebCore.Common
{
    public static class PROFILE_VARNAME
    {
        public const string SKIN_NAME = "SKIN";
        public const string MAX_PAGE_SIZE = "MAXPAGESIZE";
    }
    public static class SYSTEM_STORE_PROCEDURES
    {
        #region DEFCODE, DEFERROR, DEFVALIDATE, DEFLANG
        public const string LIST_DEFCODE = "SYSTEM_PROCS.SP_DEFCODE_SEL_ALL";
        public const string LIST_DEFERROR = "SYSTEM_PROCS.SP_DEFERROR_SEL_ALL";
        public const string LIST_DEFVALIDATE = "SYSTEM_PROCS.SP_DEFVALIDATE_SEL_ALL";
        public const string LIST_DEFLANG = "SYSTEM_PROCS.SP_DEFLANG_SEL_ALL";
        public const string LIST_DEFMENU = "SYSTEM_PROCS.SP_DEFMENU_SEL_ALL";
        public const string LIST_DEFRIBBON = "SYSTEM_PROCS.SP_DEFRIBBON_SEL_ALL";
        #endregion

        #region LIST MODULE
        public const string LIST_STATIC_MODULE = "SYSTEM_PROCS.SP_DEFMOD_SEL_STATIC";
        public const string LIST_BATCH_MODULE = "SYSTEM_PROCS.SP_DEFMOD_SEL_BATCH";
        public const string LIST_MAINTAIN_MODULE = "SYSTEM_PROCS.SP_MODMAINTAIN_SEL_ALL";
        public const string LIST_CHART_MODULE = "SYSTEM_PROCS.SP_MODCHART_SEL_ALL";
        public const string LIST_SEARCHMASTER_MODULE = "SYSTEM_PROCS.SP_MODSEARCH_SEL_ALL";
        public const string LIST_SWITCH_MODULE = "SYSTEM_PROCS.SP_MODSWITCH_SEL_ALL";
        public const string LIST_IMPORT_MODULE = "SYSTEM_PROCS.SP_MODIMPORT_SEL_ALL";
        public const string LIST_EXECUTEPROC_MODULE = "SYSTEM_PROCS.SP_MODEXECPROC_SEL_ALL";
        public const string LIST_ALERT_MODULE = "SYSTEM_PROCS.SP_MODALERT_SEL_ALL";
        public const string LIST_STATISTICS_MODULE = "SYSTEM_PROCS.SP_MODSTATISTICS_SEL_ALL";
        public const string LIST_REPORT_MODULE = "SYSTEM_PROCS.SP_MODREPORT_SEL_ALL";
        public const string LIST_TREE_MODULE = "SYSTEM_PROCS.SP_MODTREE_SEL_ALL";
        public const string LIST_EXP_MODULE = "SYSTEM_PROCS.SP_MODEXP_SEL_ALL";
        public const string LIST_WORKFLOW_MODULE = "SYSTEM_PROCS.SP_DEFMOD_SEL_WORKFLOW";
        public const string LIST_DASHBOARD_MODULE = "SYSTEM_PROCS.SP_MODDASHBOARD_SEL_ALL";
        #endregion

        #region LIST MODULE DETAILS
        public const string LIST_FIELD_INFO = "SYSTEM_PROCS.SP_DEFMODFLD_SEL_ALL";
        public const string LIST_BUTTON = "SYSTEM_PROCS.SP_DEFMODBTN_SEL_ALL";
        public const string LIST_BUTTON_PARAM = "SYSTEM_PROCS.SP_DEFMODBTNPARAM_SEL_ALL";
        public const string LIST_BATCHINFO_BY_NAME = "SYSTEM_PROCS.SP_MODBATCH_SEL_BY_BATCHNAME";
        public const string LIST_BATCH_INFO = "SYSTEM_PROCS.SP_MODBATCH_SEL_BY_MODID";
        public const string LIST_STOREPROC = "SYSTEM_PROCS.SP_STOREPROC_SEL_ALL";
        #endregion

        #region TRANSATION
        public const string TRANS_STOREPROC = "SYSTEM_PROCS.SP_TLLOG_SEL";
        public const string GETTXNUM = "SP_GET_TXNUM";
        public const string TRANS_LOGINS = "SP_TLLOG_INS";
        public const string GET_FLDCD = "SP_GET_FLDCD";
        public const string SET_FLDCD = "SP_TLLOGFLD_INS";
        #endregion

        #region MODULE INSTALLER
        public const string MODULE_UNINSTALL = "SYSTEM_PROCS.SP_MODULE_UNINST";
        #endregion

        #region USER, GROUP, ROLE & SESSION
        public const string CREATE_NEW_SESSION = "SYSTEM_PROCS.SP_SESSIONS_CREATE";
        public const string UPDATE_SESSION_INFO = "SYSTEM_PROCS.SP_SESSIONS_UPDATE";
        public const string GET_SESSION_INFO = "SYSTEM_PROCS.SP_SESSIONS_SEL_BY_KEY";
        public const string TERMINAL_SESSION_INFO = "SYSTEM_PROCS.SP_SESSIONS_TERMINAL";
        public const string GET_SESSION_USER_INFO = "SYSTEM_PROCS.SP_USER_SEL_BY_USERNAME";
        public const string CHECK_USER_ROLE = "SYSTEM_PROCS.SP_USERROLE_CHECK";
        public const string LIST_USER_ROLE = "SYSTEM_PROCS.SP_DEFROLE_SEL_BY_USERID";
        public const string LIST_FUNCTION_USER_ROLE = "SYSTEM_PROCS.SP_DEFROLE_SESSIONUSERID";
        public const string LIST_GROUP_ROLE = "SYSTEM_PROCS.SP_DEFROLE_SEL_BY_GROUPID";
        public const string DELETE_GROUP_ROLE = "SYSTEM_PROCS.SP_GROUPROLE_DEL";
        public const string INSERT_GROUP_ROLE = "SYSTEM_PROCS.SP_GROUPROLE_INS";
        public const string DELETE_USER_ROLE = "SYSTEM_PROCS.SP_USERROLE_DEL";
        public const string INSERT_USER_ROLE = "SYSTEM_PROCS.SP_USERROLE_INS";
        public const string PROFILES_SEL = "SYSTEM_PROCS.SP_PROFILES_SEL";
        public const string GROUP_LIST_USERS = "SYSTEM_PROCS.SP_GROUP_LIST_USERS";
        public const string PROFILES_SEL_EXTRA = "SYSTEM_PROCS.SP_PROFILES_SEL_EXTRA";
        public const string PROFILES_UDP_EXTRA = "SYSTEM_PROCS.SP_PROFILES_UDP_EXTRA";
        public const string LIST_GROUP_SUMMARY_INFO = "SYSTEM_PROCS.SP_GROUPSUMMARY_SEL_ALL";
        public const string LIST_HEADER_EXPORT_INFO = "SYSTEM_PROCS.SP_EXPORTHEADER_SEL_ALL";
        public const string LIST_SYSVAR_INFO = "SYSTEM_PROCS.SP_SYSVAR_SEL_ALL";
        //add by TrungTT - 27.12.2012 - UserLog
        public const string USERSLOG_INSERT = "SYSTEM_PROCS.SP_USERSLOG_INS";
        //End TrungTT.
        //TuDQ them
        public const string GET_CHART_INF = "SYSTEM_PROCS.SP_GET_CHART_INF";
        //End 

        public const string GET_USER_BY_USERNAME_PASSWORD = "SP_USER_SEL_BY_USERNAME_PASSWORD";
        public const string DEFERROR_SELECT_ALL = "sp_DEFERROR_SELECT_ALL";
        public const string DEFLANG_SELECT_TEXT_LANG = "sp_DEFLANG_SELECT_TEXT_LANG";
        public const string DEFLANG_SELECT_BTN_LANG = "sp_DEFLANG_SelectAllBtn";
        #endregion

        #region DEVELOPERS
        public const string GET_STATIC_MODULE = "SP_DEFMOD_SEL_STATIC_BY_MODID";
        public const string GET_BATCH_MODULE = "SP_DEFMOD_SEL_BATCH_BY_MODID";
        public const string GET_MAINTAIN_MODULE = "DEV_SP_MODMAINTAIN_SELBY_MODID";
        public const string GET_REPORT_MODULE = "SP_MODREPORT_SEL_BY_MODID";
        public const string GET_CHART_MODULE = "SP_MODCHART_SEL_BY_MODID";
        public const string GET_SEARCHMASTER_MODULE = "SP_MODSEARCH_SEL_BY_MODID";
        public const string GET_SWITCH_MODULE = "SP_MODSWITCH_SEL_BY_MODID";
        public const string GET_IMPORT_MODULE = "SP_MODIMPORT_SEL_BY_MODID";
        public const string GET_EXECUTEPROC_MODULE = "SP_MODEXECPROC_SEL_BY_MODID";
        public const string GET_ALERT_MODULE = "SP_MODALERT_SEL_BY_MODID";
        public const string GET_STATISTICS_MODULE = "SP_MODSTATISTICS_SEL_BY_MODID";
        public const string LIST_FIELD_INFO_BY_MODID = "DEVELOPER_PROCS_SP_DEFMODFLD_SEL_BY_MODID";
        public const string LIST_BUTTON_BY_MODID = "DEVELOPER_PROCS_SP_DEFMODBTN_SEL_BY_MODID";
        public const string LIST_BUTTON_PARAM_BY_MODID = "DEVELOPER_PROCS_SP_DEFMODBTNPARAM_SEL_BY_MODID";
        public const string LIST_LANGUAGE_BY_MODID = "DEVELOPER_PROCS_SP_DEFLANG_SEL_BY_MODID";
        public const string LIST_STOREPROC_BY_MODID = "DEVELOPER_PROCS_SP_STOREPROC_SEL_BY_MODID";
        public const string UPDATE_DEFLANG = "SP_DEFLANG_RPL";
        public const string MODULE_EXPORT = "SP_MODULE_EXPORT";
        public const string GENERATE_PACKGE = "SP_GENERATE_PACKAGE";
        public const string DEFROLE_UDP_PARENT = "SP_DEFROLE_UDP_PARENT";
        public const string UPDATE_LAYOUT = "SP_DEFLANG_UDP_LAYOUT";
        public const string SAVE_FILE = "KERNEL_PROCS.SP_FILE_INS";
        //Tudq them
        public const string GET_TREE_MODULE = "SP_MODTREE_SEL_BY_MODID";
        public const string GET_EXPRESSION_MODULE = "SP_MODEXP_SEL_BY_MODID";
        public const string UPDATE_UBOUND_EXPRESSION = "SP_DEFMODFLD_UPD_UBEXPRESSION";
        //
        // Excel Template
        public const string EXCEL_TEMPLATE_INS = "PKG_TEMPLATE.SP_EXCELTEMPLATE_INS";
        public const string EXCEL_ELEMENTTEMPLATE_INS = "PKG_TEMPLATE.SP_EXCELTEMPELEMENT_INS";
        public const string REPORTBTNAME_INS = "PKG_EXCELREPORT.INS_REPORTBT_NAME";
        public const string UP_ROWFROMTO = "PKG_TEMPLATE.SP_UP_ROW_FROM_TO";

        public const string GET_MAINTAINPOS_MODULE = "DEVELOPER_PROCS_SP_MODMAINTAINPOS_SEL_BY_MODID";
        public const string GET_ORDER_MODULE = "DEVELOPER_PROCS_SP_MODORDER_SEL_BY_MODID";
        public static string DEV_SP_MODMAINTAIN_SELBY_MODID = "DEV_SP_MODMAINTAIN_SELBY_MODID";
        public static string DEV_SP_MODSEARCH_SEL_BY_MODID = "DEV_SP_MODSEARCH_SEL_BY_MODID";
        public static string DEV_MODEXECPROC_SEL_BY_MODID = "DEV_MODEXECPROC_SEL_BY_MODID";
        public const string DEFCODE_SelectByTypeValue = "DEFCODE_SelectByTypeValue";
        public const string SYSVAR_SelectAll = "SYSTEM_PROCS_SP_SYSVAR_SEL_ALL";
        public const string Menu_SelectAll = "SYSTEM_PROCS_SP_DEF_MENU_SEL_ALL";
        public const string DEFLANG_SelectAllIcon_MenuText = "sp_DEFLANG_SelectAllIcon_MenuText";
        public const string DEFLANG_SelectRoleByUserId = "sp_DEF_SELECTROLEBYUSER";

        public const string DEFCODE_SelectAll = "defcode_selectAll";



        #endregion

        #region MODULE TREE
        public static string MODULE_TREE = "";
        public const string GETMODULE_TREE = "GET_MODULETREE_STORE";
        public const string GETMODULE_TREELANG = "GET_MODULETREE_LANG";
        #endregion
    }

    /// <summary>
    /// Các hằng số sử dụng cho module SearchMaster
    /// </summary>
    public static class SEARCHMASTER
    {
        public const string REMOVE_BUTTON_NAME = "btnRemove";
        public const string MODULE_RETURN_ID = "MDRT";
    }

    public static class STATICMODULE
    {
        public const string GENERATE_MODULE_PACKAGE = "GENPK";
        public const string FIELD_MAKER = "FLDMK";
        public const string INSTALL_MODULE_PACKAGE = "INSPK";
        public const string IEMODULE = "INSMD";
        public const string EDITLANG = "ELANG";
        public const string LOGIN_MODULE = "LOGIN";
        public const string USER_ROLE_MODULE = "UROLE";
        public const string GROUP_ROLE_MODULE = "GROLE";
        public const string SYSTEM_LOG_VIEW = "SYLOG";
        public const string VIEW_DATA_FLOW = "DFLOW";
        public const string SQL_MODEL_DESIGNER = "SQLMD";
        public const string MODULE_CONFIG = "EDMOD";
        public const string UPFILE_MODULE = "UFILE";
        public const string READ_EXCEL_MODULE = "REXEL";
        public const string SEND_REPORT = "SERPT";
        public const string UPFILE_MODID = "03907";
        //TUDQ them
        public const string COLUMNEXPORT = "CLEXP";
        //END
        //duchvm
        public const string REPLAY_MARKET_MODULE = "REMAR";
        public const string RELATIONSHIP_MODULE = "RELAT";
        public const string FILE_SYNC_MODULE = "FISYN";
        public const string MONITOR_UPCOM_MARKET = "MOUPC";
        public const string MONITOR_HNX = "MOHNX";
        public const string MONITOR_HSX = "MOHSX";
        public const string STOCKPRICE = "STOCK";
    }

    /// <summary>
    /// Các hằng số sử dụng cho module Maintain
    /// </summary>
    public static class MAINTAIN
    {
        public const int LABEL_TOP_OFFSET = 3;
        public const int OFFSET_WIDTH = 3;
        public const int CONTROL_HEIGHT = 28;
        public const int CONTROL_LABEL_WIDTH = 100;
        public const int TEXTAREA_HEIGHT = 100;
        public const int TEXTAREA_CLIENT_HEIGHT = 96;
    }

    public static class CA
    {
        public const string MSG_REG_SUCCESS = "Bạn đã đăng ký thành công chứng thư số với UBCK NN !";
        public const string MSG_REG_ERROR_GETINFO = "Không lấy được thông tin chứng thư số !";
        public const string TITLE_ALERT = "Thông báo";
        public const string TITLE_ERROR = "Lỗi";
    }

    public static class IMPORTMASTER
    {
        public const string IMPORT_FILE_EXTENSIONS = "Microsoft Excel Worksheets(*.xls, *.xlsx)|*.xls;*.xlsx";
        public const string EXPORT_FILE_EXTENSIONS = "Microsoft Excel Worksheets(*.xls)|*.xls|Microsoft Excel 2007 Worksheets(*.xlsx)|*.xlsx";
        public const string ATTACKED_FILE_EXTENSIONS = "Microsoft Word (*.docx)|*.docx|Microsoft Excel Worksheets(*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        public const string IMP_MSG_SUCCESS = "Import thành công dữ liệu vào hệ thống !";
        public const string IMP_MSG_FAIL = "Import dữ liệu thất bại !";
        public const string IMP_MSG_CANNOT_ROLLBACK_DATA = "Lỗi không rollback được dữ liệu!";
        public const string IMP_MSG_ERR_TYPE = "Lỗi file excel không đúng định dạng!";
    }

    public static class CONSTANTS
    {
        public const int MAX_ROWS_IN_BUFFER = 1000;
        public const int MAX_ROWS_IN_EXPORT_XLS = 65000 - 1;
        public const int MAX_ROWS_IN_EXPORT_XLSX = 300000 - 1;
        public const string SESSION_LOGOUT_DESCRIPTION = "User Self Logout!";
        public const string COLUMNS_VISIBLE_INDEXES = "COLUMN_INDEXES";
        public const string GRIDVIEW_LAYOUT = "GRIDVIEW_LAYOUT";
        public const string MODULE_FULLNAME_FORMAT = "{0}${1}";
        public const string GRIDVIEW_NAME_FORMAT = "gvw{0}";
        public const string TEXTINPUT_NAME_FORMAT = "txt{0}";
        public const string RICHTEXT_NAME_FORMAT = "rt{0}";
        public const string COMBOX_NAME_FORMAT = "txt{0}";
        public const string BAR_BUTTON_ITEM_FORMAT = "{0}";
        public const string BAR_SUB_ITEM_FORMAT = "sub{0}";
        public const string DATETIME_NAME_FORMAT = "dt{0}";
        public const string CHECKEDCOMBOBOX_NAME_FORMAT = "ccb{0}";
        public const string COLUMN_NAME_FORMAT = "col{0}";
        public const string LABEL_NAME_FORMAT = "lb{0}";
        public const string CONFIRM_RESOURCE_NAME = "CONFIRM";
        public const string BUILT_IN_MENU_NAME = "BUILT_IN_MENUS";
        public const string BUILT_IN_MENU_ID = "000000";
        public const string LOOKUPEDIT_NAME_FORMAT = "txt{0}";
        //TuDQ them
        public const string CHKBOX_NAME_FORMAT = "txt{0}";
        public const string RG_NAME_FORMAT = "txt{0}";
        public const string CHECKBOX_CHECKED = "1";
        public const string CHECKBOX_UBCHECK = "0";
        //End
        public const int INCREASE_COLUMN_WIDTH = 15;
        public const string DEFAULT_DATETIME_FORMAT = "d/M/yyyy";
        public const string REGNAME_LANGID = "LanguageID";
        public const string DEFAULT_LANGID = "VN";
        public const string DEFAULT_APPTYPE = "HO";
        public const string DEFAULT_ICON = "ICON_MODULE";
        public const string ORACLE_SESSION_USER = "SESSIONINFO_USERNAME";
        public const string ORACLE_SESSIONKEY = "SESSIONKEY";
        public const string ORACLE_REPORT_ID = "REPORT_ID";
        public const string ORACLE_STARTPOINT = "pv_STARTPOINT";
        public const string ORACLE_REPORT_DETAILID = "DETAILID";
        public const string ORACLE_REPORT_RPTLOGSID = "RPTLOGSID";
        public const string ORACLE_CURSOR_OUTPUT = "STORE_OUTPUT";
        public const string ORACLE_MODULE_ID = "PV_MODID";

        public const string ORACLE_EXCEPTION_PARAMETER_NAME = "PROC_RETURN";
        public const string ORACLE_OUT_PARAMETER_SECID = "SECID";
        public const int ORACLE_USER_HANDLED_EXCEPTION_CODE = 20000;
        public const string CACHED_RESOURCE_FILENAME = "Cache\\IMSS-Resource.cache";
        public const string CACHED_LANG_FILENAME = "Cache\\IMSS-Language.cache";
        public const string PRIVATE_MD5_KEY = "IMSS{0}HASH";
        public const string ROOT_MENUID = "000000";
        public const string IMAGE16_FOLDER = "Theme\\16x16";
        public const string IMAGE24_FOLDER = "Theme\\24x24";
        public const string IMAGE32_FOLDER = "Theme\\32x32";
        public const string IMAGE48_FOLDER = "Theme\\48x48";
        public const int PAGE_VISIBLE_COUNT = 100;

        public const int USER_TYPE_UBCKNN = 1;
        public const int USER_TYPE_MEMBERS = 2;
        public const string USER_ADMIN = "ADMIN";

        public const string Yes = "Y";
        public const string No = "N";

        public const string FLDTYPEDEC = "DEC";
        public const string FLDTYPEDTE = "DTE";
        public const string FLDFORMATDEC = "###,##0.00";
        public const string SUMTEXT = "Tổng";

        public const int INT_CHUNK_SIZE = 512000;

        public const string AppNameSMS = "SCMS";
        public const string AppNameFMS = "FMS";
        public const string AppNameIDMS = "IDMS";
        public const string AutoConfigYes = "Y";
        public const string AutoConfigNo = "N";

        public const string Parameter_SecID = "PV_SECID";

        public const string MENU_NAME_LOGOUT = "MNU_LOGOUT";
        public const string MENU_NAME_NDTNN = "MNU_NDTNN";

        public const string TEXTCASE_N = "N";
        public const string TEXTCASE_U = "U";
        public const string TEXTCASE_L = "L";
        public const string SQL_WSALIAS = "WSALIAS";
        public const int SQL_USER_HANDLED_EXCEPTION_CODE = 50000;

    }

    public static class CHECKMARK
    {
        public const string FIELDID = "CHECKS";
        public const string FIELDVALUE = "1";

    }

    public static class SYSVAR
    {
        public const string GRNAME_SYSTEM = "SYSTEM";
        public const string GRNAME_SYS = "SYS";
        public const string VARNAME_EXPORT = "EXPORTDLG";
        public const string VARNAME_IMPCHECK = "IMPCHK";
        public const string VARNAME_EXPORT_BY_COLUMN = "EXPCOL";
        public const string VARNAME_EXPORT_LOGO = "EXPLOGO";
        public const string VARNAME_APPNAME = "APPNAME";
        public const string VARNAME_WEBFILEPATH = "WEBFILEPATH";
        public const string VARNAME_RPTEXCELFILEPATH = "RPTEXCELFILEPATH";
        public const string VARNAME_LINKNDTNN = "LINKNDTNN";
        public const string VARNAME_DOMAINSRV = "DOMAINSRV";
        public const string VARNAME_DOMAINNAME = "DOMAINNAME";
        public const string VARNAME_DOMAINLOGIN = "DOMAINLOGIN";
    }

    public static class UPLOADEXCELSTATUS
    {
        public const string WaitProcess = "Chờ thực thi";
        public const string UploadError = "Gửi báo cáo không thành công";
        public const string AlreadyUpLoad = "Báo cáo đã được gửi";
        public const string TemplateNotFound = "Không tồn tại biểu mẫu";
        public const string UploadCompeleted = "Gửi báo cáo thành công";
        public const string NamefileErr = "Sai quy tắc đặt tên";
        public const string ValidateError = "Sai dữ liệu";
        public const string OpenError = "Không mở được file";
        public const string ReportSamePeriod = "Thư mục chứa nhiều hơn 1 file báo cáo cùng kỳ";

        public const string stt1 = "Sai quy tắc đặt tên : Sai đơn vị gửi báo cáo";
        public const string stt2 = "Sai quy tắc đặt tên : Không phải tệp dữ liệu báo cáo";
        public const string stt3 = "Sai quy tắc đặt tên : Không phải tệp dữ liệu báo cáo";
        public const string stt4 = "Sai quy tắc đặt tên : Thiếu ngày bắt đầu hoặc ngày kết thúc của báo cáo tuần";
        public const string stt5 = "Sai quy tắc đặt tên : Sai giá trị của kỳ báo cáo";
        public const string stt6 = "Sai quy tắc đặt tên : Sai giá trị của ngày bắt đầu báo cáo - ngày kết thúc báo cáo";
        public const string stt7 = "Sai quy tắc đặt tên : Sai ngày gửi báo cáo";

        public const string faildata = "Dữ liệu báo cáo sai, hoặc để null";

        public const string idrpt_investerins = "T0072";
        public const string idrpt_investerupd = "T0073";
        //WaitProcess, TemplateNotFound, TemplateFound,
        //    ReadCompeleted,
        //    ValidateCompeleted,
        //    UploadCompeleted,
        //    UploadError,
        //    ValidateError,
        //    WriteFileError,
        //    OpenError,
        //    AlreadyUpLoad,
        //    WaitUpload,
        //    ReadError
    }

}

