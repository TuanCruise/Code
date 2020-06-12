using System.Runtime.Serialization;

namespace WebCore.Entities
{
    [DataContract]
    public class CachedHashInfo
    {
        [DataMember]
        public string LanguageHash { get; set; }
        [DataMember]
        public string ErrorsInfoHash { get; set; }
        [DataMember]
        public string ModuleFieldsInfoHash { get; set; }
        [DataMember]
        public string GroupSummaryInfoHash { get; set; }
        [DataMember]
        public string ModulesInfoHash { get; set; }
        [DataMember]
        public string CodesInfoHash { get; set; }
        [DataMember]
        public string SearchButtonsInfoHash { get; set; }
        [DataMember]
        public string SearchButtonParamsInfoHash { get; set; }
        [DataMember]
        public string OracleParamsInfoHash { get; set; }
        [DataMember]
        public string ValidatesInfoHash { get; set; }
        [DataMember]
        public string ExportHeaderInfoHash { get; set; }
        [DataMember]
        public string SysvarInfoHash { get; set; }
    }
}