using WebCore.Base;
using System.Runtime.Serialization;

namespace WebCore.Entities
{
    [DataContract]
    public class OracleStore : EntityBase
    {
        [DataMember, Column(Name = "STORENAME")]
        public string StoreName { get; set; }
    }
}
