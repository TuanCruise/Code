using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    public class GroupMod
    {
        [DataMember, Column(Name = "GROUPID")]
        public string GroupId { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModId { get; set; }

    }

}
