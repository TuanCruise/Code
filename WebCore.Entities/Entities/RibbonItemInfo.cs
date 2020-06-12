using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class RibbonItemInfo : EntityBase
    {
        [DataMember, Column(Name="RIBID")]
        public string RibbonID { get; set; }
        [DataMember, Column(Name = "RIBOWNERID")]
        public string RibbonOwnerID { get; set; }
        [DataMember, Column(Name = "RIBNAME")]
        public string RibbonName { get; set; }
        [DataMember, Column(Name = "RIBTYPE")]
        public string RibbonType { get; set; }
        [DataMember, Column(Name = "BEGINGROUP")]
        public string BeginGroup { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "SUBMOD")]
        public string SubModule { get; set; }
        [DataMember, Column(Name = "DEVELOPER")]
        public string Developer { get; set; }
    }
}
