using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class CodeInfo : EntityBase
    {
        [DataMember, Column(Name = "CDTYPE")]
        public string CodeType { get; set; }
        [DataMember, Column(Name = "CDNAME")]
        public string CodeName { get; set; }
        [DataMember, Column(Name = "CDVALUE")]
        public string CodeValue { get; set; }
        [DataMember, Column(Name="CDVALUENAME")]
        public string CodeValueName { get; set; }
        [DataMember, Column(Name = "CDPOSITION")]
        public int CodePosition { get; set; }

        public override string ToString()
        {
            return string.Format("{0,4}   {1,-32}", CodeValue, CodeValueName);
        }
    }
}
