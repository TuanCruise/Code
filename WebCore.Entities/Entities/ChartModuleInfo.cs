using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ChartModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "CHARTTYPE")]
        public string ChartType { get; set; }
        [DataMember, Column(Name = "CHARTDATASTORE")]
        public string ChartDataStore { get; set; }
        [DataMember, Column(Name = "SHOWADDBTN")]
        public string ShowAddButton { get; set; }
        [DataMember, Column(Name = "SLEEPTIME")]
        public string SleepTime { get; set; }
        [DataMember, Column(Name = "SIZEPANE")]
        public string SizePane { get; set; }
        [DataMember, Column(Name = "CHARTSERIES")]
        public string ChartSeries { get; set; }
    }
}
