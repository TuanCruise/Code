using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class ErrorInfo : EntityBase
    {
        [DataMember, Column(Name = "ERRCODE")]
        public int ErrorCode { get; set; }
        [DataMember, Column(Name = "ERRNAME")]
        public string ErrorName { get; set; }
    }
}
