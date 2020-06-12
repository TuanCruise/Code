using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class MaintainModuleInfo : ModuleInfo
    {
        [DataMember, Column(Name = "ADDSELECTSTORE")]
        public string AddSelectStore { get; set; }
        [DataMember, Column(Name = "ADDINSERTSTORE")]
        public string AddInsertStore { get; set; }
        [DataMember, Column(Name = "EDITSELECTSTORE")]
        public string EditSelectStore { get; set; }
        [DataMember, Column(Name = "EDITUPDATESTORE")]
        public string EditUpdateStore { get; set; }
        [DataMember, Column(Name = "VIEWSELECTSTORE")]
        public string ViewSelectStore { get; set; }
        [DataMember, Column(Name = "ADDREPEATINPUT")]
        public string AddRepeatInput { get; set; }
        [DataMember, Column(Name = "EDITREPEATINPUT")]
        public string EditRepeatInput { get; set; }
        [DataMember, Column(Name = "VIEWREPEATINPUT")]
        public string ViewRepeatInput { get; set; }
        [DataMember, Column(Name = "SHOWSUCCESS")]
        public string ShowSuccess { get; set; }
        [DataMember, Column(Name = "APPROVE")]
        public string Approve { get; set; }
        [DataMember, Column(Name = "REPORT")]
        public string Report { get; set; }
        [DataMember, Column(Name = "REPORTSTORE")]
        public string ReportStore { get; set; }
        [DataMember, Column(Name = "REPORTNAME")]
        public string ReportName { get; set; }
        [DataMember, Column(Name = "DATAREPORTSTORE")]
        public string DataReportStore { get; set; }
        //TUDQ them
        [DataMember, Column(Name = "TRANSACTION_MODE")]
        public string TRANSACTION_MODE { get; set; }
        [DataMember, Column(Name = "TRANSATIONSTORE")]
        public string TRANSATIONSTORE { get; set; }
        [DataMember, Column(Name = "EXECTRANSCTIONSTORE")]
        public string EXECTRANSCTIONSTORE { get; set; }
        //
        [DataMember, Column(Name = "ADDINSERTCHECK")]
        public string ADDINSERTCHECK { get; set; }
        [DataMember, Column(Name = "EDITSTORECHECK")]
        public string EDITSTORECHECK { get; set; }
        [DataMember, Column(Name = "BUTTONTEXT")]
        public string ButtonText { get; set; }
    }
}
