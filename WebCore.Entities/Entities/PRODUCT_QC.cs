using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class PRODUCT_QC
    {
        [DataMember, Column(Name = "Item_Code")]
        public string Item_Code { get; set; }
        [DataMember, Column(Name = "AttrSet_ID")]
        public int AttrSet_ID { get; set; }
        [DataMember, Column(Name = "isQCPass")]
        public string isQCPass { get; set; }
        [DataMember, Column(Name = "QC_Time")]
        public DateTime QC_Time { get; set; }
        [DataMember, Column(Name = "QC_Values")]
        public string QC_Values { get; set; }
        [DataMember, Column(Name = "QC_User_ID")]
        public int QC_User_ID { get; set; }
        [DataMember, Column(Name = "Partition_Code")]

        public string Partition_Code { get; set; }
        [DataMember, Column(Name = "Disabled")]
        public string Disabled { get; set; }
        [DataMember, Column(Name = "GroupId")]
        public int GroupId { get; set; }

    }

}
