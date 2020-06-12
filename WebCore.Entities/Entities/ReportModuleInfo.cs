using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ReportModuleInfo : ModuleInfo
    {
        [DataMember, Column( Name = "DESCRIPTION" )]
        public string Description { get; set; }
        [DataMember, Column(Name = "RORDER")]
        public string ReportOrder { get; set; }
        [DataMember, Column(Name = "MODCODE")]
        public string ModCode { get; set; }
        [DataMember, Column(Name = "RPTNAME")]
        public string ReportName { get; set; }
        [DataMember, Column(Name = "STORENAME")]
        public string StoreName { get; set; }
        [DataMember, Column(Name = "REPORTTYPE")]
        public string ReportType { get; set; }
        [DataMember, Column(Name = "VISIBLE")]
        public string Visible { get; set; }
        [DataMember, Column(Name = "ORIENTION")]
        public string Oriention { get; set; }
    }
}
