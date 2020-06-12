using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class BatchInfo : EntityBase
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "BATCHNAME")]
        public string BatchName { get; set; }
        [DataMember, Column(Name = "BATCHSTORE")]
        public string BatchStore { get; set; }
        [DataMember, Column(Name = "AUTO_REPORT")]
        public string Auto_Report { get; set; }
        [DataMember, Column(Name = "REPORT_NAME")]
        public string Report_Name { get; set; }
        [DataMember, Column(Name = "STORE_DATA")]
        public string Store_Data { get; set; }
        [DataMember, Column(Name = "ORIENTION")]
        public string Oriention { get; set; }
        [DataMember, Column(Name = "MOD_REPORT")]
        public string Mod_Report { get; set; }
    }
}
