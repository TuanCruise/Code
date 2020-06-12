using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class SwitchModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name="SWITCHSTORE")]
        public string SwitchStore { get; set; }
    }
}
