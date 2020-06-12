using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    // NOTE: ALL PROFILE VARIABLES WILL BE DEFINE IN HERE
    [DataContract]
    public class UserProfile
    {
        [DataMember, Column(Name = "SKINNAME"), SyncDB("Black")]
        public string ApplicationSkinName { get; set; }
        [DataMember, Column(Name = "PAGESIZE"), SyncDB("250")]
        public int MaxPageSize { get; set; }
        public UserProfile()
        {
            ApplicationSkinName = "Black";
            MaxPageSize = 250;
        }
    }
}
