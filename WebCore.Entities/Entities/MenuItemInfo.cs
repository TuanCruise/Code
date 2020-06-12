using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class MenuItemInfo : EntityBase
    {
        [DataMember, Column(Name="MENUID")]
        public string MenuID { get; set; }
        [DataMember, Column(Name = "OWNERMENUID")]
        public string OwnerMenuID { get; set; }
        [DataMember, Column(Name = "MENUNAME")]
        public string MenuName { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModID { get; set; }
        [DataMember, Column(Name = "SUBMOD")]
        public string SubMod { get; set; }
        [DataMember, Column(Name = "BEGINGROUP")]
        public string BeginGroup { get; set; }
        [DataMember, Column(Name = "MENUTYPE")]
        public string MenuType { get; set; }
    }
}
