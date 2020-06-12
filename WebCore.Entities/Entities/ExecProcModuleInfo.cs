using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ExecProcModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "EXECUTESTORE")]
        public string ExecuteStore { get; set; }
    }
}
