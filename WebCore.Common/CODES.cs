using WebCore.Base;
namespace WebCore
{
    namespace CODES
    {
        namespace EXPORT
        {
            public static class EXPORTTYPE
            {
                public const string EXCEL_XLS = "XLS";
                public const string EXCEL_XLSX = "XLX";
                public const string XML = "XML";
                public const string TXT = "TXT";
            }
            public static class PAGESIZE
            {
                public const string LETTER = "1";
                public const string A4 = "9";
                public const string A3 = "8";
                public const string A2 = "66";
            }
            public static class LAYOUT
            {
                public const string PORTRAIT = "P";
                public const string LANDSCAPE = "L";
            }
        }


        namespace SESSIONS
        {
            public static class SESSIONSTATUS
            {
                public const string SESSION_ACTIVATED = "A";
                public const string SESSION_TERMINATED = "T";
                public const string SESSION_TIMEOUT = "O";
            }
        }
        namespace DEFROLE
        {
            public static class ROLETYPE
            {
                public const string CATEGORY = "C";
                public const string HIGH_ROLE = "H";
                public const string NORMAL_ROLE = "N";
            }
            public static class EXECTYPE
            {
                public const string VIEW = "00";
                public const string ADD = "01";
                public const string EDIT = "02";
                public const string DELETE = "03";   
                public const string EXPORT_EXCEL = "04";
                public const string EXPORT_PDF = "05";
                public const string EXPORT_WORD = "06";
            }
        }
        namespace DEFFORMAT
        {
            public static class FILTER
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }
        namespace DEFMENU
        {
            public static class MENUTYPE
            {
                public const string BUTTON16 = "B16";
                public const string BUTTON32 = "B32";
            }
            public static class BEGINGROUP
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }

        namespace DEFRIBBON
        {
            public static class RIBTYPE
            {
                public const string RIBBON_PAGE = "RPG";
                public const string RIBBON_GROUP = "RGP";
                public const string BUTTON48 = "B48";
                public const string BUTTON32 = "B32";
                public const string BUTTON16 = "B16";
                public const string SUB_BUTTON16 = "S16";
            }
            public static class BEGINGROUP
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }

        namespace MODCHART
        {
            public static class CHARTTYPE
            {
                public const string YIELD_CURVE_WITH_FIT_OPTIONS = "YCF";
                public const string YIELD_CURVE_NO_FIT_OPTIONS = "YCN";
                public const string PIE_CHART = "PIE";
                public const string LINE_CHART = "LNE";  // Đồ thị giam sát 1 Mã chứng khoán với HNX-INDEX nhiều ngày theo URD
                public const string BAR_CHART = "BAR";
                public const string BAR_LINE_CHART = "BLC";

                public const string LINE_LINE_BAR_CHART = "LLB"; // Đồ thị giống mã URD dưới nhưng chỉ cho chọn 1 Mã CK
                public const string LINE_BAR_BAR_CHART = "LBB"; //Đồ thị gồm 2 panel - 1.Giá trị GD và HNX-INDEX - 2.Khối lượng GD
                public const string LINE_BAR_BAR_URD_CHART = "URD"; //Đồ thị so sánh Các Mã CK và HNX - INDEX nhiều ngày theo URD

                public const string LINE_LINE_BAR_DAY_CHART = "LLC";//đỒ THỊ CHỈ SỐ THỊ TRƯỜNG TRONG NGÀY
                public const string LINE_BAR_BAR_DAY_CHART = "LBC"; //Đồ thị so sánh Các Mã CK và HNX - INDEX trong ngày theo URD
                //TUDQ them
                public const string STOCK_CHART = "STK";
                //END
            }
            public static class YC_CURVETYPE
            {
                public const string FORWARD_RATES_CURVE = "FWR";
                public const string ZERO_RATES_CURVE = "ZRR";
                public const string ALL_CURVE = "ALL";
            }
        }
        namespace DEFMOD
        {
            public static class MODSTORESTATUS
            {
                public const string VALID = "OK";
                public const string INVALID = "ERR";
            }
            [SyncDB("CODE$EXECMODE")]
            public static class EXECMODE
            {
                public const string BLOCKED = "B";
                public const string SINGLE_INSTANCE = "S";
                public const string MULTI_INSTANCE = "M";
                public const string FEEDBACK = "F"; // Feedback execute Export
                public const string CUSTOMRESULT = "C"; // Custom Mod Statistic
                public const string TEMPLATE = "T"; // Import template
                public const string REPORT_BT = "X"; // Import bao cao bat thuong
            }
            public static class PROBLEMTYPE
            {
                public const string NOT_SETUP_ROLE = "001";
                public const string SWITCH_MODULE_UITYPE = "002";
                public const string CBCONDITION_OPERATOR_NOT_IS_EQC = "003";
                public const string MODID_NOT_CORRECT = "004";
            }
            [SyncDB("CODE$STARTMODE")]
            public static class STARTMODE
            {
                public const string AUTOMATIC = "A";
                public const string MANUAL = "M";
            }
            [SyncDB("CODE$MODTYPE")]
            public static class MODTYPE
            {
                public const string STATICMODULE = "00";
                public const string STOREEXECUTE = "01";
                public const string MAINTAIN = "02";
                public const string SEARCHMASTER = "03";
                public const string SWITCHMODULE = "04";
                public const string MODCHART = "05";
                public const string IMPORTMASTER = "06";
                public const string ALERTMASTER = "07";
                public const string BATCHPROCESS = "08";
                public const string STATISTICSMASTER = "09";                
                public const string REPORTMASTER = "10";                                
                public const string TREEVIEW = "11";
                public const string EXPESSION = "12";
                public const string WORKFLOW = "13";
                public const string TRANS = "14";
                public const string DASHBOARD = "69";
            }
            [SyncDB("CODE$SUBMOD")]
            public static class SUBMOD
            {
                public const string MAINTAIN_ADD = "MAD";
                public const string MAINTAIN_EDIT = "MED";
                public const string MAINTAIN_VIEW = "MVW";
                //TUDQ them
                public const string TRANSACTION_VIEW = "MVT";
                //
                public const string MODULE_MAIN = "MMN";
                public const string SEARCH_EXPORT = "EXP";
                //ADD BY TRUNGTT - 12.3.2012 - SEND MAIL
                public const string SEND_MAIL = "SML";
                //END TRUNGTT
            }
            [SyncDB("CODE$UITYPE")]
            public static class UITYPE
            {
                public const string NOWINDOW = "N";
                public const string POPUP = "P";
                public const string TABPAGE = "T";
            }
            [SyncDB("CODE$USERCLOSE")]
            public static class USERCLOSE
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            [SyncDB("CODE$USERHIDE")]
            public static class USERHIDE
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            [SyncDB("CODE$USERMAX")]
            public static class USERMAX
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class SENDMAIL
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class CONFIGBCC
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }
        namespace DEFGROUPSUMMARY
        {
            public static class SUMMARYTYPE
            {
                public const string COUNT = "COUNT";
                public const string SUM = "SUM";
                public const string MIN = "MIN";
                public const string MAX = "MAX";
                public const string AVERAGE = "AVG";
            }
        }
        namespace DEFMODFLD
        {
            public static class COLUMNSORT
            {
                public const string ASCENDING = "A";
                public const string DESCENDING = "D";
                public const string NO_SORT = "N";
            }
            public static class TEXTCASE
            {
                public const string DEFAULT = "D";
                public const string UPPER = "U";
                public const string LOWER = "L";
            }
            public static class MERGECELL
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class HOLDCOLUMN
            {
                public const string LEFT = "L";
                public const string RIGHT = "R";
                public const string DEFAULT = "D";
            }
            public static class WHEREEXTENSION
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class STATISTICS
            {
                public const string NO = "NO";
                public const string SUM = "NO";
            }
            public static class SCDTYPE
            {
                public const string TEXT_COMPARE = "TXC";
                public const string NUMBERIC_COMPARE = "NBC";
                public const string EQUAL_COMPARE = "EQC";
                public const string DATETIME_COMPARE = "DTC";
                public const string IN_STRING_ARRAY = "IAR";
                public const string EQUAL_ONLY = "EQO";
                public const string TEXT_LIKE_ONLY = "TLO";
            }
            public static class CONDITION_OPERATOR
            {
                public const string LIKE = "001";
                public const string NOTLIKE = "002";
                public const string EQUAL = "003";
                public const string NOTEQUAL = "004";
                public const string BEGINWITH = "005";
                public const string ENDWITH = "006";
                public const string LARGER = "007";
                public const string SMALLER = "008";
                public const string LARGEROREQUAL = "009";
                public const string SMALLEROREQUAL = "010";
                public const string INARRAY = "011";
                public const string NOTINARRAY = "012";
            }
            public static class EQUAL_ONLY
            {
                public const string EQUAL = CONDITION_OPERATOR.EQUAL;
            }
            public static class TEXT_LIKE_ONLY
            {
                public const string LIKE = CONDITION_OPERATOR.LIKE;
            }
            public static class TEXT_COMPARE
            {
                public const string LIKE = CONDITION_OPERATOR.LIKE;
                public const string NOTLIKE = CONDITION_OPERATOR.NOTLIKE;
                public const string EQUAL = CONDITION_OPERATOR.EQUAL;
                public const string NOTEQUAL = CONDITION_OPERATOR.NOTEQUAL;
                public const string BEGINWITH = CONDITION_OPERATOR.BEGINWITH;
                public const string ENDWITH = CONDITION_OPERATOR.ENDWITH;
            }
            public static class NUMBERIC_COMPARE
            {
                public const string EQUAL = CONDITION_OPERATOR.EQUAL;
                public const string NOTEQUAL = CONDITION_OPERATOR.NOTEQUAL;
                public const string LARGER = CONDITION_OPERATOR.LARGER;
                public const string SMALLER = CONDITION_OPERATOR.SMALLER;
                public const string LARGEROREQUAL = CONDITION_OPERATOR.LARGEROREQUAL;
                public const string SMALLEROREQUAL = CONDITION_OPERATOR.SMALLEROREQUAL;
            }
            public static class EQUAL_COMPARE
            {
                public const string EQUAL = CONDITION_OPERATOR.EQUAL;
                public const string NOTEQUAL = CONDITION_OPERATOR.NOTEQUAL;
            }
            public static class DATETIME_COMPARE
            {
                public const string EQUAL = CONDITION_OPERATOR.EQUAL;
                public const string NOTEQUAL = CONDITION_OPERATOR.NOTEQUAL;
                public const string LARGEROREQUAL = CONDITION_OPERATOR.LARGEROREQUAL;
                public const string SMALLEROREQUAL = CONDITION_OPERATOR.SMALLEROREQUAL;
            }
            public static class INARRAY_COMPARE
            {
                public const string INARRAY = CONDITION_OPERATOR.INARRAY;
                public const string NOTINARRAY = CONDITION_OPERATOR.NOTINARRAY;
            }
            public static class NULLABLE
            {
                public const string NO = "N";
                public const string YES = "Y";
                public const string NULL_HORIZONTAL = "H";
            }
            public static class TEXTALIGN
            {
                public const string DEFAULT = "D";
                public const string LEFT = "L";
                public const string RIGHT = "R";
                public const string CENTER = "C";
            }
            public static class ICONONLY
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class CTRLTYPE
            {
                public const string CHECKBOX = "CK";
                public const string SPINEDITOR = "SP";
                public const string COMBOBOX = "CB";
                public const string COMBOBOXCHECK = "CBC";
                public const string FILETEXTBOX = "FT";
                public const string MASKEDIT = "MK";
                public const string PASSWORD = "PW";
                public const string TEXTBOX = "TB";
                public const string LABEL = "LB";
                public const string FILESAVE = "FS";
                public const string TEXTAREA = "TA";
                public const string DATETIME = "DT";
                public const string SUGGESTIONTEXTBOX = "SB";
                public const string CHECKEDCOMBOBOX = "CC";
                public const string DEFINEDGROUP = "DG";
                public const string LOOKUPVALUES = "LV";
                public const string LOOKUPEDIT = "LE";
                public const string UPLOADFILE = "UF";
                public const string RICHTEXTEDITOR = "RT";
                public const string GRIDVIEW = "GV";                
                //TuDQ them
                public const string RADIOGROUP = "RG";
                //End
            }
            public static class FLDGROUP
            {
                public const string PARAMETER = "PRM";//P
                public const string SEARCH_COLUMN = "SCL";//L
                public const string SEARCH_GROUP = "SGR";//G
                public const string SEARCH_CONDITION = "SCD";//D
                public const string COMMON = "CMN";//C
                public const string SQL_EXPRESSION = "SEP";
                public const string SQL_OPERATOR = "SOP";
                public const string IMPORT_COLUMN = "IMC";//I
                public const string SEARCH_EXPORT = "EXP";//E
                //ADD BY TRUNGTT - 12.3.2012 - SEND MAIL
                public const string SEND_MAIL = "SML";//S
                //END TRUNGTT
                //TuDQ them
                public const string CHART = "CHT";
                //End

            }
            public static class FLDTYPE
            {
                public const string DATE = "DTE";
                public const string DATETIME = "DTI";
                public const string DECIMAL = "DEC";
                public const string DOUBLE = "DBL";
                public const string FLOAT = "FLT";
                public const string INT32 = "INT";
                public const string INT64 = "LNG";
                public const string STRING = "STR";
                //TuDQ them
                public const string CHART = "CHT";
                //End
            }
            public static class TXCHECK
            {
                public const string CHECKRYEAR = "<=[RRRR]";
            }
            public static class POPUPMODE
            {
                public const string MANUAL = "M";
                public const string ONFOCUS = "F";
            }
            [SyncDB("CODE$READONLYMODE")]
            public static class READONLYMODE
            {
                public const string READWRITE = "RW";
                public const string READONLY = "RO";
            }
            public static class FOS
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class GOS
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class TABSTOP
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class VISIBLE
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }
        namespace DEFMODBTN
        {
            public static class ONTOOLBAR
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class BEGINGROUP
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class SHOWCONFIRM
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
            public static class MULTIEXEC
            {
                public const string NO = "N";
                public const string YES = "Y";
                public const string MULTI_ROWS = "MR";
            }
           
        }
        namespace DEFMODBTNPARAM
        {
            public static class USED
            {
                public const string NO = "N";
                public const string YES = "Y";
            }
        }
        namespace SQL_EXPRESSION
        {
            public static class SQL_LOGIC
            {
                public const string AND = "A";
                public const string OR = "O";
            }
        }
        namespace SQL_OPERATOR
        {
            public static class CODE_VALUE
            {
                public const string EQUAL = "EQL";
                public const string NOTEQUAL = "NEQ";
                public const string ALLVALUES = "ALL";
            }
            public static class STRING
            {
                public const string BEGINWITH = "BGW";
                public const string ENDWITH = "ENW";
                public const string EQUAL = "EQL";
                public const string LIKE = "LKE";
                public const string NOTEQUAL = "NEQ";
                public const string NOTLIKE = "NLK";
            }
        }
        namespace MODMAINTAIN
        {
            public static class REPEATINPUT
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class SHOWSUCCESS
            {
                public const string YES = "Y";
                public const string NO = "N";                
            }
            public static class APROVE
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class REPORT
            {
                public const string FILE = "F";
                public const string NO = "N";
                public const string YES = "Y";
            }
        }
        namespace MODSEARCH
        {
            public static class AUTOFITWIDTH
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class PAGEMODE
            {
                public const string PAGE_FROM_DATASET = "PGS";
                public const string PAGE_FROM_READER = "PGR";
                public const string ALL_FROM_DATASET = "ALS";
                public const string APPEND_FROM_READER = "APR";
            }
            public static class FULLWIDTH
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class GROUPBOX
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class SHOWGRPCOL
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class SHOWCHECKBOX
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
            public static class AUTOREFRESH
            {
                public const string YES = "Y";
                public const string NO = "N";
            }
        }
        namespace USERS
        {
            public static class STATUS
            {
                public const string ACTIVATED = "A";
                public const string LOCKED = "L";
            }
        }
        namespace SYSTEM
        {
            public static class GRMOD
            {
                public const string SYSTEM = "001";
                public const string DEVELOPER = "002";
                public const string USER_AND_GROUP = "003";
                public const string LOOKUP_MODULES = "004";
            }
        }

    }
}