using WebCore.Base;
using System.Runtime.Serialization;

namespace WebCore.Entities
{
    [DataContract]
    public class ButtonInfo : EntityBase
    {
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "BTNID")]
        public string ButtonID { get; set; }
        [DataMember, Column(Name = "BTNNAME")]
        public string ButtonName { get; set; }
        [DataMember, Column(Name = "BTNGROUP")]
        public string ButtonGroup { get; set; }
        [DataMember, Column(Name = "BEGINGROUP")]
        public string BeginGroup { get; set; }
        [DataMember, Column(Name = "CALLMODID")]
        public string CallModuleID { get; set; }
        [DataMember, Column(Name = "CALLSUBMOD")]
        public string CallSubModule { get; set; }
        [DataMember, Column(Name = "SHOWCONFIRM")]
        public string ShowConfirm { get; set; }
        [DataMember, Column(Name = "MULTIEXEC")]
        public string MultiExecute { get; set; }
        [DataMember, Column(Name = "ONTOOLBAR")]
        public string ShowOnToolbar { get; set; }
        [DataMember, Column(Name = "PARAMMODE")]
        public string ParameterMode { get; set; }
        [DataMember, Column(Name = "DBCLICK")]
        public string DBClick { get; set; }          
    }
}
