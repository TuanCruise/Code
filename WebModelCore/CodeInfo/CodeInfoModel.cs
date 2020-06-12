using System.Collections.Generic;
using WebModelCore.ControlModel;

namespace WebModelCore.CodeInfo
{
    public class CodeInfoModel
    {
        public string Name { get; set; }
        public List<WebCore.Entities.CodeInfo> CodeInfos { get; set; }
        public string ControlType { get; set; }
        public string DataCallBack { get; set; }
        public List<ScheduleControlModel> ScheduleControls { get; set; }
        public CodeInfoModel()
        {
            CodeInfos = new List<WebCore.Entities.CodeInfo>();
            ScheduleControls = new List<ScheduleControlModel>();
        }
    }
    public class CodeInfoParram
    {
        public string CtrlType { get; set; }
        public string Name { get; set; }
        public string ListSource { get; set; }
        public string Parrams { get; set; }
    }
}
