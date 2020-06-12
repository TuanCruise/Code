using WebCore.Base;
using System.Runtime.Serialization;

namespace WebCore.Entities
{
    [DataContract]
    public class Role : EntityBase
    {
        [DataMember, Column(Name = "ROLEID")]
        public string RoleID { get; set; }
        [DataMember, Column(Name = "REQUIREROLEID")]
        public string RequireRoleID { get; set; }
        [DataMember, Column(Name = "ROLENAME")]
        public string RoleName { get; set; }
        [DataMember, Column(Name = "ROLETYPE")]
        public string RoleType { get; set; }
        [DataMember, Column(Name = "CATEGORYID")]
        public string CategoryID{ get; set; }
        [DataMember]
        public string TranslatedRoleName { get; set; }
        [DataMember, Column(Name = "ROLEVALUE")]
        public string RoleValue { get; set; }

        public Role()
        {
            RoleValue = "N";
        }
    }
}
