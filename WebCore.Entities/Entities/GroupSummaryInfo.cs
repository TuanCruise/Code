using WebCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WebCore.Entities
{
    [DataContract]
    [Serializable]
    public class GroupSummaryInfo
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }

        [DataMember, Column(Name = "SUBMOD")]
        public string SubModule { get; set; }

        [DataMember, Column(Name = "GROUPID")]
        public string GroupID { get; set; }

        [DataMember, Column(Name = "GROUPNAME")]
        public string GroupName { get; set; }

        [DataMember, Column(Name = "FLDNAME")]
        public string FieldName { get; set; }

        [DataMember, Column(Name = "SUMMARYTYPE")]
        public string SummaryType { get; set; }

        [DataMember, Column(Name = "SHOWHEADER")]
        public string ShowHeader { get; set; }

        [DataMember, Column(Name = "FOOTERCOLUMN")]
        public string FooterColumn { get; set; }
    }
}