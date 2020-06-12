using System;
using System.Collections.Generic;
using System.Text;

namespace WebModelCore.ControlModel
{
    public class ScheduleControlModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public string className { get; set; }
        public string editable { get { return "true"; } }
        public string modId { get; set; }
    }
}
