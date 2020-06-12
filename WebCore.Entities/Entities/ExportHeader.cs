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
    public class ExportHeader 
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }

        [DataMember, Column(Name = "STARTROW")]
        public int StartRow { get; set; }

        [DataMember, Column(Name = "STARTCOL")]
        public int StartCol { get; set; }

        [DataMember, Column(Name = "RNUM")]
        public int RowNum { get; set; }

        [DataMember, Column(Name = "CNUM")]
        public int ColNum { get; set; }

        [DataMember, Column(Name = "TEXT")]
        public string Text { get; set; }

        [DataMember, Column(Name = "ORDB")]
        public int Ordb { get; set; }

        [DataMember, Column(Name = "MAXROW")]
        public int MaxRow { get; set; }        
    }
}
