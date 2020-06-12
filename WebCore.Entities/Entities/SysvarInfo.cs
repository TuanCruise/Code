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
    public class SysvarInfo
    {
        [DataMember, Column(Name = "GRNAME")]
        public string GrName { get; set; }

        [DataMember, Column(Name = "VARNAME")]
        public string VarName { get; set; }

        [DataMember, Column(Name = "VARVALUE")]
        public string VarValue { get; set; }

        [DataMember, Column(Name = "VARDESC")]
        public string VarDesc { get; set; }
    }
}
