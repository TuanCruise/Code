using System;
using System.Collections.Generic;
using System.Text;
using WebModelCore.CodeInfo;

namespace WebModelCore
{
    public class DataViewEdit
    {
        public List<dynamic> DataControl { get; set; }
        public string ModId { get; set; }

        public string SubModId { get; set; }
        public string ModSearchId { get; set; }
        public string KeyEdit { get; set; }
        public bool Edit { get; set; }
        public string FieldNameEdit { get; set; }
        public ModuleInfoModel ModuleInfo { get; set; }
        public int Success { get; set; }
        public List<CodeInfoModel> DataCombobox{get;set;}
    }
}
