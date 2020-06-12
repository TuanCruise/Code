using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract,
        KnownType(typeof(MaintainModuleInfo)),
        KnownType(typeof(ReportModuleInfo)),
        KnownType(typeof(ImportModuleInfo)),
        KnownType(typeof(SearchModuleInfo)),
        KnownType(typeof(SwitchModuleInfo)),
        KnownType(typeof(ExecProcModuleInfo)),
        KnownType(typeof(AlertModuleInfo)),
        KnownType(typeof(StatisticsModuleInfo)),
        KnownType(typeof(ChartModuleInfo)),
        KnownType(typeof(DashboardInfo))]
    public class ModuleInfo
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "SUBMOD")]
        public string SubModule { get; set; }
        [DataMember, Column(Name = "MODNAME")]
        public string ModuleName { get; set; }
        [DataMember, Column(Name = "MODTYPE")]
        public string ModuleType { get; set; }
        [DataMember, Column(Name = "UITYPE")]
        public string UIType { get; set; }
        [DataMember, Column(Name = "USERHIDE")]
        public string UserHide { get; set; }
        [DataMember, Column(Name = "USERMAX")]
        public string UserMax { get; set; }
        [DataMember, Column(Name = "USERCLOSE")]
        public string UserClose { get; set; }
        [DataMember, Column(Name = "EXECMODE")]
        public string ExecuteMode { get; set; }
        [DataMember, Column(Name = "MODTYPENAME")]
        public string ModuleTypeName { get; set; }
        [DataMember, Column(Name = "ROLE")]
        public string RoleID { get; set; }
        [DataMember, Column(Name = "STARTMODE")]
        public string StartMode { get; set; }
        [DataMember, Column(Name = "GRMOD")]
        public string GroupModule { get; set; }
        [DataMember, Column(Name = "EXPANDED")]
        public string Expanded { get; set; }
        [DataMember, Column(Name = "ISRPTSPC")]
        public string IsReportSpecial { get; set; }
        [DataMember, Column(Name = "SENDEMAIL")]
        public string SendEmail { get; set; }
        [DataMember, Column(Name = "ISREALTIME")]
        public string IsRealTime { get; set; }
        public override string ToString()
        {
            return string.Format("{0,4}   {1,-32}", ModuleID, ModuleName);
        }
    }
}
