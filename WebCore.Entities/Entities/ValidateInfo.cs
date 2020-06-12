using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ValidateInfo
    {
        [DataMember, Column(Name = "VALIDATENAME")]
        public string ValidateName { get; set; }
        [DataMember, Column(Name = "REGEXMATCH")]
        public string RegularMatch { get; set; }
        [DataMember, Column(Name = "STOREVALIDATE")]
        public string StoreValidate { get; set; }
        [DataMember, Column(Name = "VALUETYPE")]
        public string ValueType { get; set; }
        [DataMember, Column(Name = "NUMISINTEGER")]
        public string NumberIsInteger { get; set; }
        [DataMember, Column(Name = "NUMMINVALUE")]
        public string NumberMinValue { get; set; }
        [DataMember, Column(Name = "NUMMAXVALUE")]
        public string NumberMaxValue { get; set; }

        [Column(Name = "NUMNOTEQUAL")]
        public string NumberNotEqualString
        {
            set 
            { 
                var astrNumbers = value.Split(new [] { ",", " ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                NumberNotEqual = new decimal[astrNumbers.Length];
                for(var i = 0; i < astrNumbers.Length; i++)
                {
                    NumberNotEqual[i] = decimal.Parse(astrNumbers[i]);
                }
            }
        }

        [DataMember]
        public decimal[] NumberNotEqual { get; set; }
    }
}