using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ModTreeView
    {
        [DataMember, Column(Name = "MODID")]
        public string Modid { get; set; }
        [DataMember, Column(Name = "EXECUTETREESTORE")]
        public string ExecuteTreestore { get; set; }
    }
}

