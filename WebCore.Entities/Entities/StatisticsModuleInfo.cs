using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class StatisticsModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "STORENAME")]
        public string StoreName { get; set; }
        [DataMember, Column(Name = "FULLWIDTH")]
        public string FullWidth { get; set; }
        [DataMember, Column(Name = "PAGEMODE")]
        public string PageMode { get; set; }
        [DataMember, Column(Name = "STATISTICSTORENAME")]
        public string StatisticQuery { get; set; }
        [DataMember, Column(Name = "AUTOFITWIDTH")]
        public string AutoFitWidthColumns { get; set; }
    }
}
