using System.Collections.Generic;
using WebCore.Entities;

namespace WebModelCore.Common
{
    public class RowGridModMaintainModel
    {
        public List<ModuleFieldInfo> Fields { get; set; }
        public ModuleInfoViewModel ModuleInfo { get; set; }

        public RowGridModMaintainModel()
        {
            Fields = new List<ModuleFieldInfo>();
            ModuleInfo = new ModuleInfoViewModel();
        }
    }
}
