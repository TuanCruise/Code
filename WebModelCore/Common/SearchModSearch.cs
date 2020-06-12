using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Entities;

namespace WebModelCore.Common
{
    public class SearchModSearch
    {
        public ModuleInfo ModInfo { get; set; }
        public SearchModuleInfo SearchInfo { get; set; }
        public List<SearchConditionInstance> StaticConditionInstances { get; set; }
        public SearchModSearch()
        {
            SearchInfo = new SearchModuleInfo();
            StaticConditionInstances = new List<SearchConditionInstance>();
            ModInfo = new ModuleInfo();
        }
    }
}
