using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [Serializable]
    [DataContract]
    public class LOG
    {
        [DataMember, Column(Name = "Id")]
        public int Id { get; set; }
        [DataMember, Column(Name = "ModId")]
        public string ModId { get; set; }
        [DataMember, Column(Name = "Type")]
        public string Type { get; set; }

        [DataMember, Column(Name = "ActionError")]
        public string ActionError { get; set; }
        [DataMember, Column(Name = "Note")]
        public string Note { get; set; }
        [DataMember, Column(Name = "UserName")]
        public string UserName { get; set; }
        [DataMember, Column(Name = "Ip")]
        public string Ip { get; set; }

    }

}
