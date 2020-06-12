using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class LanguageInfo : EntityBase
    {
        [DataMember, Column(Name = "LANGID")]
        public string LanguageID { get; set; }
        [DataMember, Column(Name = "LANGNAME")]
        public string LanguageName { get; set; }
        [DataMember, Column(Name = "LANGVALUE")]
        public string LanguageValue { get; set; }
        [DataMember, Column(Name = "LANGLOBVALUE")]
        public string LargerLanguageValue { get; set; }
        [DataMember, Column(Name = "APPTYPE")]
        public string AppType { get; set; }
        [DataMember, Column(Name = "BRTYPE")]
        public string BrType { get; set; }
    }
}
