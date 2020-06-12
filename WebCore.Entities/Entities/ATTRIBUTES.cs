using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ATTRIBUTES
    {
        [DataMember, Column(Name = "ATTR_ID")]
        public int ATTR_ID { get; set; }
        [DataMember, Column(Name = "ATTR_CODE")]
        public string ATTR_CODE { get; set; }
        [DataMember, Column(Name = "ATTR_IDX")]
        public int ATTR_IDX { get; set; }
        [DataMember, Column(Name = "ATTR_DESC")]
        public string ATTR_DESC { get; set; }
        [DataMember, Column(Name = "CAPTION_NAME")]
        public string CAPTION_NAME { get; set; }
        [DataMember, Column(Name = "ISQUALIFY")]
        public string ISQUALIFY { get; set; }
        [DataMember, Column(Name = "ISTECHATTR")]
        public string ISTECHATTR { get; set; }
        [DataMember, Column(Name = "ISGENERAL")]
        public string ISGENERAL { get; set; }
        [DataMember, Column(Name = "LBL_FNAME")]
        public string LBL_FNAME { get; set; }
        [DataMember, Column(Name = "DATA_TYPE")]
        public string DATA_TYPE { get; set; }
        [DataMember, Column(Name = "DATA_SIZE")]
        public int DATA_SIZE { get; set; }
        [DataMember, Column(Name = "ISDATABASE")]
        public string ISDATABASE { get; set; }
        [DataMember, Column(Name = "TBL_NAME")]
        public string TBL_NAME { get; set; }
        [DataMember, Column(Name = "TBL_FNAME")]
        public string TBL_FNAME { get; set; }
        [DataMember, Column(Name = "DISABLED")]
        public string DISABLED { get; set; }

    }

}
