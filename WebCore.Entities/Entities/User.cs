using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class User
    {

        [DataMember, Column(Name = "USERID")]
        public int UserID { get; set; }
        [DataMember, Column(Name = "USERNAME")]
        public string Username { get; set; }
        [DataMember, Column(Name = "DISPLAYNAME")]
        public string DisplayName { get; set; }
        [DataMember, Column(Name = "PASSWORD")]
        public string Password { get; set; }
        [DataMember, Column(Name = "STATUS")]
        public string Status { get; set; }
        [DataMember, Column(Name = "DATECREATED")]
        public DateTime DateCreated { get; set; }
        [DataMember, Column(Name = "DATEMODIFIED")]
        public DateTime DateModified { get; set; }
        [DataMember, Column(Name = "DATELOGINED")]
        public DateTime DateLogined { get; set; }
        [DataMember, Column(Name = "TYPE")]
        public int Type { get; set; }
        [DataMember, Column(Name = "JOBID")]
        public string JobId { get; set; }
        [DataMember, Column(Name = "ORGPASSWORD")]
        public string OrgPassword { get; set; }
        [DataMember, Column(Name = "MODIFYBY")]
        public string ModifyBy { get; set; }
        [DataMember, Column(Name = "DELETED")]
        public string Deleted { get; set; }
        [DataMember, Column(Name = "CREATEDBY")]
        public string CreatedBy { get; set; }
        [DataMember, Column(Name = "EMAIL")]
        public string Email { get; set; }
        [DataMember, Column(Name = "PHONE")]
        public string Phone { get; set; }
        [DataMember, Column(Name = "LASTCHANGEPASS")]
        public DateTime LastChangePass { get; set; }
        [DataMember, Column(Name = "ATHBRANCH_ID")]
        public string AthBranch_Id{ get; set; }
        [DataMember, Column(Name = "CLEVEL")]
        public int CLevel { get; set; }
        [DataMember, Column(Name = "EMAILRECEIVE")]
        public string EmailReceive { get; set; }
        [DataMember, Column(Name = "AVATARNAME")]
        public string AvatarName { get; set; }


    }
    //public class User
    //{
    //    [DataMember, Column(Name = "USERID")]
    //    public int UserID { get; set; }
    //    [DataMember, Column(Name = "GROUPID")]
    //    public string GroupID { get; set; }
    //    [DataMember, Column(Name = "USERNAME")]
    //    public string Username { get; set; }
    //    [DataMember, Column(Name = "DISPLAYNAME")]
    //    public string DisplayName { get; set; }
    //    [DataMember, Column(Name = "USERSTATUS")]
    //    public string UserStatus { get; set; }
    //    [DataMember, Column(Name = "CREATEDATE")]
    //    public DateTime CreateDate { get; set; }
    //    [DataMember, Column(Name = "LASTMODIFY")]
    //    public DateTime LastModify { get; set; }
    //    [DataMember, Column(Name = "LASTLOGIN")]
    //    public DateTime LastLogin { get; set; }

    //    [DataMember, Column(Name = "TYPE")]
    //    public int Type { get; set; }

    //    public override string ToString()
    //    {
    //        return Username + " - " + DisplayName;
    //    }
    //}
}
