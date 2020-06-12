using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class TaskInfo: ModuleInfo
    {
        [DataMember, Column(Name = "MODID")]
        public int STT { get; set; }
        [DataMember, Column(Name = "STT")]
        public string ModID { get; set; }
        [DataMember, Column(Name = "TASKNAME")]
        public string TaskName { get; set; }
        [DataMember, Column(Name = "TASKMOD")]
        public string TaskMod { get; set; }
        [DataMember, Column(Name = "TASKSUBMOD")]
        public string TaskSubMod { get; set; }
        [DataMember, Column(Name = "TASKSTATUS")]
        public string TaskStatus { get; set; }
        [DataMember, Column(Name = "TASKDESC")]
        public string TaskDesc { get; set; }
        [DataMember, Column(Name = "TASKORDB")]
        public int TaskOrdb { get; set; }
        [DataMember, Column(Name = "KEYMAP")]
        public string TaskKeyMap { get; set; }
        [DataMember, Column(Name = "IMAGEURI")]
        public string ImageUri { get; set; }

    }
}
