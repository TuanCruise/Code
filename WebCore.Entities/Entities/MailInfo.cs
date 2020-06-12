using System.Runtime.Serialization;
using WebCore.Base;

namespace CWebCoreore.Entities
{
    [DataContract]
    public class MailInfo : EntityBase
    {
        [DataMember, Column(Name = "MAIL_ID")]
        public string Mail_ID { get; set; }
        [DataMember, Column(Name = "MAIL_SERVER")]
        public string Mail_Server { get; set; }
        [DataMember, Column(Name = "PORT")]
        public string Port { get; set; }
        [DataMember, Column(Name = "MAIL_NAME")]
        public string Mail_Name { get; set; }
        [DataMember, Column(Name = "PASSWORD")]
        public string Password { get; set; }
    }
}
