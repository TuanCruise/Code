using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class NewsInfo : EntityBase
    {
        [DataMember, Column(Name = "NEWSID")]
        public string NewsID { get; set; }
        [DataMember, Column(Name = "SYMBOL")]
        public string Symbol { get; set; }
        [DataMember, Column(Name = "TITLE")]
        public string Title { get; set; }
    }
}
