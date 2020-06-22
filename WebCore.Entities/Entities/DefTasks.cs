using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class DefTasks
    {
        [DataMember, Column(Name = "MODID")]
        public string Modid { get; set; }
        [DataMember, Column(Name = "TASKNAME")]
        public string TaskName { get; set; }
        [DataMember, Column(Name = "TASKMOD")]
        public string TaskMod { get; set; }
        [DataMember, Column(Name = "TASKSUBMOD")]
        public string TaskSubmod { get; set; }
        [DataMember, Column(Name = "TASKSTATUS")]
        public string TaskStatus { get; set; }
        [DataMember, Column(Name = "TASKSDESC")]
        public string TaskDesc { get; set; }
        [DataMember, Column(Name = "TASKORDB")]
        public int TaskOrdb { get; set; }
        [DataMember, Column(Name = "KEYMAP")]
        public string KeyMap { get; set; }
        [DataMember, Column(Name = "IMAGEURI")]
        public string ImageUri { get; set; }
        [DataMember, Column(Name = "PARENTID")]
        public string ParentId { get; set; }
    }
}

