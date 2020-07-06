using System.Collections.Generic;
using WebCore.Entities;
using System.Runtime.Serialization;
using WebCore.Base;
namespace WebModelCore.ModTreeViewModel
{
    public class ModTreeViewModel
    {
        public DefTasks DefTasks { get; set; }
        public List<DefTasks> DefTasksChild { get; set; }
        public ModTreeViewModel()
        {
            DefTasksChild = new List<DefTasks>();
        }
    }
    [DataContract]
    public class TreeviewInfo
    {
        [DataMember, Column(Name = "ID")]
        public string Id { get; set; }
        [DataMember, Column(Name = "TREENAME")]
        public string TreeName { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModId { get; set; }
        [DataMember, Column(Name = "PARENTID")]
        public string ParentId { get; set; }
    }
}
