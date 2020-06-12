using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class SearchConditionInstance
    {
        [DataMember, Column(Name = "CONDID")]
        public string ConditionID { get; set; }
        [DataMember, Column(Name = "SQLLOGIC")]
        public string SQLLogic { get; set; }
        [DataMember, Column(Name = "OPERATOR")]
        public string Operator { get; set; }
        [DataMember, Column(Name = "Value")]
        public string Value { get; set; }
        [DataMember]
        public SearchConditionInstance[] SubCondition { get; set; }
    }
}
