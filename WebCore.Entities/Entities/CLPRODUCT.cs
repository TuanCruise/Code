using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLPRODUCT : EntityBase
    {
        [DataMember, Column(Name = "PRODUCTCODE")]
        public string PRODUCTCODE { get; set; }
        [DataMember, Column(Name = "CATEGORY")]
        public string CATEGORY { get; set; }
        [DataMember, Column(Name = "PRODUCTTYPE")]
        public string PRODUCTTYPE { get; set; }
        [DataMember, Column(Name = "PRODUCTNAME")]
        public string PRODUCTNAME { get; set; }
        [DataMember, Column(Name = "EFFECTDATE")]
        public DateTime EFFECTDATE { get; set; }
        [DataMember, Column(Name = "EXPIRYDATE")]
        public DateTime EXPIRYDATE { get; set; }
        [DataMember, Column(Name = "USECHIP")]
        public DateTime USECHIP { get; set; }
    }
    public class CLPRODUCTModel: CLPRODUCT
    {

    }
}
