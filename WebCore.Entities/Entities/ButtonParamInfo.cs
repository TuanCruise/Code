using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ButtonParamInfo
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "BTNNAME")]
        public string ButtonName { get; set; }
        [DataMember, Column(Name = "FLDNAME")]
        public string FieldName { get; set; }
        [DataMember, Column(Name = "COLNAME")]
        public string ColumnName { get; set; }
        [DataMember, Column(Name = "VALUE")]
        public string Value { get; set; }
        [DataMember, Column(Name = "CONDITIONNAME")]
        public string ConditionName { get; set; }
    }
}
