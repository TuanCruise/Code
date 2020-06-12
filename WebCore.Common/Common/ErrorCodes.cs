using WebCore.Base;

namespace WebCore.Common
{
    public static class ERR_SYSTEM
    {
        public const int SUCCESSFUL = 0;
        public const int ERR_SYSTEM_BASE = 100;
        public const int ERR_SYSTEM_UNKNOWN = ERR_SYSTEM_BASE + 1;
        public const int ERR_SYSTEM_GET_CLIENT_RESOURCE_FAIL = ERR_SYSTEM_BASE + 2;
        public const int ERR_SYSTEM_GET_DEFINED_LANGUAGE_FAIL = ERR_SYSTEM_BASE + 3;
        public const int ERR_SYSTEM_FIELD_NOT_FOUND = ERR_SYSTEM_BASE + 4;
        public const int ERR_SYSTEM_FIELD_DUPLICATED = ERR_SYSTEM_BASE + 5;
        public const int ERR_SYSTEM_MODULE_NOT_FOUND = ERR_SYSTEM_BASE + 6;
        public const int ERR_SYSTEM_MODULE_HAVE_TO_CALL_SUB = ERR_SYSTEM_BASE + 7;
        public const int ERR_SYSTEM_SELECT_DATAROW_FIRST = ERR_SYSTEM_BASE + 8;
        public const int ERR_SYSTEM_EXECUTE_SEARCH_FAIL = ERR_SYSTEM_BASE + 9;
        public const int ERR_SYSTEM_MAINTAIN_STORE_NOT_FOUND = ERR_SYSTEM_BASE + 10;
        public const int ERR_SYSTEM_MODULE_FIELD_NOT_FOUND_OR_DUPLICATE = ERR_SYSTEM_BASE + 11;
        public const int ERR_SYSTEM_PARSE_SQL_SCRIPT_ERROR = ERR_SYSTEM_BASE + 12;
        public const int ERR_SYSTEM_CONTROL_TYPE_NOT_FOUND = ERR_SYSTEM_BASE + 13;
        public const int ERR_SYSTEM_CONNECT_TO_SERVER_FAIL = ERR_SYSTEM_BASE + 14;
        public const int ERR_SYSTEM_SELECT_MULTIROWS_NOT_ALLOW = ERR_SYSTEM_BASE + 15;
        public const int ERR_SYSTEM_MODULE_PARAMETER_REQUIRE = ERR_SYSTEM_BASE + 16;
        public const int ERR_SYSTEM_MODULE_NOT_ALLOW_ACCESS = ERR_SYSTEM_BASE + 17;
        public const int ERR_SYSTEM_SESSION_TERMINATED_BY_ADMIN = ERR_SYSTEM_BASE + 18;
        public const int ERR_SYSTEM_SESSION_TIMEOUT = ERR_SYSTEM_BASE + 19;
        public const int ERR_SYSTEM_SESSION_NOT_EXISTS_OR_DUPLICATE = ERR_SYSTEM_BASE + 20;
        public const int ERR_SYSTEM_SESSION_TERMINATED_BY_SELF = ERR_SYSTEM_BASE + 21;
        public const int ERR_SEARCH_RESULT_NOT_FOUND = ERR_SYSTEM_BASE + 23;
        public const int ERR_SYSTEM_ONLY_MODULE_POPUP_CAN_SHOW_AS_DIALOG = ERR_SYSTEM_BASE + 24;
        public const int ERR_CHART_COUNT_SYMBOL = ERR_SYSTEM_BASE + 25;
        public const int ERR_LANGUAGE_NOT_SUPPORTED = ERR_SYSTEM_BASE + 26;
        public const int ERR_SYSTEM_FIELD_NOT_CONFIG_LISTSOURCE = ERR_SYSTEM_BASE + 27;
        public const int ERR_SYSTEM_FUNCTION_ONLY_AVAILABLE_IN_DEBUG_MODE = ERR_SYSTEM_BASE + 28;
        public const int ERR_SYSTEM_SWITCH_MODULE_FAIL = ERR_SYSTEM_BASE + 29;
        public const int ERR_SYSTEM_LOOKUP_EXPRESSION_REQUIRE_ONE_OR_TWO_ARGUMENTS = ERR_SYSTEM_BASE + 30;
        public const int ERR_SYSTEM_LOOKUP_EXPRESSION_CAN_NOT_CONTAIN_NAME = ERR_SYSTEM_BASE + 31;
        public const int ERR_SYSTEM_DEFMODBTNPARAM_ONE_VALUE = ERR_SYSTEM_BASE + 32;
        public const int ERR_SYSTEM_UPDATE_FILE_NOT_EXIST = ERR_SYSTEM_BASE + 33;
        public const int ERR_SYSTEM_MODULE_SINGLE_INSTANCE = ERR_SYSTEM_BASE + 34;
        public const int ERR_SYSTEM_BLOCKED_MODULE = ERR_SYSTEM_BASE + 35;
    }

    public static class ERR_SQL
    {
        public const int ERR_SQL_BASE = 200;
        public const int ERR_SQL_OPEN_CONNECTION_FAIL = ERR_SQL_BASE + 1;
        public const int ERR_SQL_EXECUTE_COMMAND_FAIL = ERR_SQL_BASE + 2;
        public const int ERR_SQL_DISCOVERY_PARAMS_FAIL = ERR_SQL_BASE + 3;
        public const int ERR_SQL_ASSIGN_PARAMS_FAIL = ERR_SQL_BASE + 4;
    }

    [SyncDB("ERR_USER")]
    public static class ERR_USER
    {
        public const int ERR_USER_BASE = 300;
        [SyncDB("USERNAME_NOT_EXIST")]
        public const int ERR_USER_USERNAME_NOT_EXIST = ERR_USER_BASE + 1;
        [SyncDB("NOT_ACTIVATED")]
        public const int ERR_USER_USER_NOT_ACTIVED = ERR_USER_BASE + 2;
        [SyncDB("WRONG_PASSWORD")]
        public const int ERR_USER_USERNAME_AND_PASS_NOT_MATCH = ERR_USER_BASE + 3;
        [SyncDB("USERNAME_EXISTED")]
        public const int ERR_USER_USERNAME_EXISTED = ERR_USER_BASE + 4;
        [SyncDB("USER_ALREADY_LOGINED")]
        public const int ERR_USER_ALREADY_LOGINED = ERR_USER_BASE + 5;
    }

    public static class ERR_IMPORT
    {
        public const int ERR_IMPORT_BASE = 400;
        public const int ERR_CONVERT_IMPORT_DATA_FAIL = ERR_IMPORT_BASE + 1;
        public const int ERR_FILE_OPEN_ERROR = ERR_IMPORT_BASE + 2;
        public const int ERR_COLUMN_NOT_FOUND = ERR_IMPORT_BASE + 5;
    }
    public static class ERR_GROUP
    {
        public const int ERR_GROUP_BASE = 500;
        public const int ERR_GROUP_GROUPNAME_EXISTED = ERR_GROUP_BASE + 1;
    }

    public static class ERR_ROLE
    {
        public const int ERR_ROLE_BASE = 600;
        public const int ERR_ROLE_NEW_ROLE_IS_EXISTED = ERR_ROLE_BASE + 1;
    }
    public static class ERR_FILE
    {
        public const int ERR_FILE_BASE = 700;
        public const int ERR_FILE_IS_NOT_EXIST = ERR_FILE_BASE + 1;
        public const int ERR_FILE_IS_NOT_ATTACKED = ERR_FILE_BASE + 2;

    }
}
