using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class DashboardInfo: ModuleInfo
    {
        [DataMember, Column(Name = "ID")]
        public string ID { get; set; }
        [DataMember, Column(Name = "SOURCE")]
        public string Source { get; set; }
        [DataMember, Column(Name = "TITLE")]
        public string Title { get; set; }
        [DataMember, Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember, Column(Name = "AUTOUPDATE")]
        public int Autoupdate { get; set; }

    }
}
