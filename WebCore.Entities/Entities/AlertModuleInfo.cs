using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class AlertModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "ALERTSTORE")]
        public string AlertStore { get; set; }
        [DataMember, Column(Name = "CLICKSTORE")]
        public string ClickStore { get; set; }
        [DataMember, Column(Name = "SLEEPTIME")]
        public int SleepTime { get; set; }
        [DataMember, Column(Name = "COUNTFIELD")]
        public string CountField { get; set; }
        [DataMember, Column(Name = "CALLMODID")]
        public string CallModuleID { get; set; }
        [DataMember, Column(Name = "CALLSUBMOD")]
        public string CallSubModule { get; set; }
    }
}
