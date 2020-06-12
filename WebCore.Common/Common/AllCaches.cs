using System;
using System.Collections.Generic;
using WebCore.Entities;

namespace WebCore.Common
{
    public class AllCaches
    {
        public static Dictionary<int, string> ErrorsInfo { get; set; }
        public static Dictionary<string, string> LanguageInfo { get; set; }
        public static Dictionary<string, ValidateInfo> ValidatesInfo { get; set; }
        public static List<ErrorInfo> BaseErrorsInfo;
        public static List<LanguageInfo> BaseLanguageInfo;
        public static List<ValidateInfo> BaseValidatesInfo;
        public static List<CodeInfo> CodesInfo { get; set; }
        public static List<ModuleFieldInfo> ModuleFieldsInfo { get; set; }
        public static List<ButtonInfo> SearchButtonsInfo { get; set; }
        public static List<ButtonParamInfo> SearchButtonParamsInfo { get; set; }
        public static List<ModuleInfo> ModulesInfo { get; set; }
        public static List<OracleParam> OracleParamsInfo { get; set; }
        public static List<GroupSummaryInfo> GroupSummaryInfos { get; set; }
        public static List<ExportHeader> ExportHeaders { get; set; }
        public static List<SysvarInfo> SysvarsInfo { get; set; }

        public static CachedHashInfo CalcHashInfo(string clientLanguageId)
        {
            return new CachedHashInfo();
        }
    }
}
