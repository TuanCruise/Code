using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ATTRSET
    {
        [DataMember, Column(Name = "ATTRSET_ID")]
        public int ATTRSET_ID { get; set; }
        [DataMember, Column(Name = "ATTRSET_CODE")]
        public string ATTRSET_CODE { get; set; }
        [DataMember, Column(Name = "ATTRSET_DESC")]
        public string ATTRSET_DESC { get; set; }
        [DataMember, Column(Name = "SKU_ID")]
        public int SKU_ID { get; set; }
        [DataMember, Column(Name = "ATTRSET_IMG")]
        public string ATTRSET_IMG { get; set; }
        [DataMember, Column(Name = "FRAME_WIDTH")]
        public int FRAME_WIDTH { get; set; }
        [DataMember, Column(Name = "FRAME_HEIGHT")]
        public int FRAME_HEIGHT { get; set; }
        [DataMember, Column(Name = "ISQUALIFY")]
        public string ISQUALIFY { get; set; }
        [DataMember, Column(Name = "ISTECHATTR")]
        public string ISTECHATTR { get; set; }
        [DataMember, Column(Name = "DISABLED")]
        public string DISABLED { get; set; }

    }
}
