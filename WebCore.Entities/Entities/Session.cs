using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class Session
    {
        [DataMember, Column(Name = "SESSIONKEY")]
        public string SessionKey { get; set; }
        [DataMember, Column(Name = "SESSIONID")]
        public int SessionID { get; set; }
        [DataMember, Column(Name = "USERID")]
        public int UserID { get; set; }
        [DataMember, Column(Name = "USERNAME")]
        public string Username { get; set; }
        [DataMember, Column(Name = "CREATEDATE")]
        public DateTime CreateDate { get; set; }
        [DataMember, Column(Name = "LASTACCESS")]
        public DateTime LastAccess { get; set; }
        [DataMember, Column(Name = "CLIENTIP")]
        public string ClientIP { get; set; }
        [DataMember, Column(Name = "CLIENTMACADDRESS")]
        public string ClientMacAddress { get; set; }
        [DataMember, Column(Name = "DNSNAME")]
        public string DNSName { get; set; }
        [DataMember, Column(Name = "SESSIONSTATUS")]
        public string SessionStatus { get; set; }
        [DataMember, Column(Name = "TERMINATEDUSERNAME")]
        public string TerminatedUsername { get; set; }
        [DataMember, Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember, Column(Name = "SYSTEMDATE")]
        public DateTime SystemDate { get; set; }
        [DataMember, Column(Name = "TYPE")]
        public int Type { get; set; }
        [DataMember, Column(Name = "CHKLOG")]
        public int ChkLog { get; set; }

        [DataMember, Column(Name = "WSALIAS")]
        public string WsAlias { get; set; }

    }
}
