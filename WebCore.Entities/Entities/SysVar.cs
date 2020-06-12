using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class SysVar : EntityBase
    {
        [DataMember, Column(Name = "Var_Name")]
        public string Var_Name { get; set; }
        [DataMember, Column(Name = "Data_Type")]
        public string Data_Type { get; set; }
        [DataMember, Column(Name = "Var_Value")]
        public string Var_Value { get; set; }
        [DataMember, Column(Name = "Var_Desc")]
        public string Var_Desc { get; set; }
        [DataMember, Column(Name = "Modify")]
        public bool Modify { get; set; }
        [DataMember, Column(Name = "InputMask")]
        public string InputMask { get; set; }
        [DataMember, Column(Name = "Status")]
        public bool Status { get; set; }

    }

}
