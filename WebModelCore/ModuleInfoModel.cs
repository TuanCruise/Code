using System;
using System.Collections.Generic;
using WebCore.Entities;

namespace WebModelCore
{
    [Serializable]
    public class ModuleInfoModel
    {
        public ModuleInfo ModulesInfo { get; set; }
        public List<ModuleFieldInfo> FieldsInfo { get; set; }
        public List<ButtonInfo> ButtonsInfo { get; set; }
        public List<ButtonParamInfo> ButtonParamsInfo { get; set; }
        public List<LanguageInfo> LanguageInfo { get; set; }
        public List<OracleParam> OracleParamsInfo { get; set; }
    }
    [Serializable]
    public class ModuleInfoViewModel
    {
        public List<ModuleInfo> ModulesInfo { get; set; }
        public List<ModuleFieldInfo> FieldsInfo { get; set; }
        public List<ButtonInfo> ButtonsInfo { get; set; }
        public List<ButtonParamInfo> ButtonParamsInfo { get; set; }
        public List<LanguageInfo> LanguageInfo { get; set; }
        public List<OracleParam> OracleParamsInfo { get; set; }
    }

    public class ModuleExcuteViewModel
    {
        public SearchModuleInfo SearchModuleInfo { get; set; }
        public MaintainModuleInfo MaintainModuleInfo { get; set; }
        public ExecProcModuleInfo ExecProcModuleInfo { get; set; }
        public string ModId { get; set; }
    }
}
