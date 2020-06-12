using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class AttrGrp
    {
        [DataMember, Column(Name = "AttrGrp_ID")]
        public int AttrGrp_ID { get; set; }
        [DataMember, Column(Name = "AttrGrp_Code")]
        public string AttrGrp_Code { get; set; }
        [DataMember, Column(Name = "AttrGrp_Desc")]
        public string AttrGrp_Desc { get; set; }
        [DataMember, Column(Name = "AttrGrp_Img")]
        public string AttrGrp_Img { get; set; }
        [DataMember, Column(Name = "Frame_Top")]
        public int Frame_Top { get; set; }
        [DataMember, Column(Name = "Frame_Left")]
        public int Frame_Left { get; set; }
        [DataMember, Column(Name = "Frame_Width")]
        public int Frame_Width { get; set; }
        [DataMember, Column(Name = "Frame_Height")]
        public int Frame_Height { get; set; }
        [DataMember, Column(Name = "isQualify")]
        public string isQualify { get; set; }
        [DataMember, Column(Name = "isTechAttr")]
        public string isTechAttr { get; set; }
        [DataMember, Column(Name = "Disabled")]
        public string Disabled { get; set; }

    }

}
