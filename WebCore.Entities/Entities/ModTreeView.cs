using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ModWorkflow
    {
        [DataMember, Column(Name = "MODID")]
        public string Modid { get; set; }
        [DataMember, Column(Name = "SUBMOD")]
        public string SubMod { get; set; }
        [DataMember, Column(Name = "TATUS")]
        public string Status { get; set; }
        [DataMember, Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember, Column(Name = "NAME")]
        public string Name { get; set; }
        [DataMember, Column(Name = "ORDB")]
        public int Ordb { get; set; }
    }
}

