using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Entities;
using WebModelCore.LogicCondition;
using System.Linq;
namespace WebModelCore
{
    public class ParramModuleQuery
    {
        public string Store { get; set; }
        public object[] Parram { get; set; }
        public List<ModuleFieldInfo> Fields { get; set; }
        public ParramModuleQuery()
        {
            Fields = new List<ModuleFieldInfo>();
        }
    }

    public class ParramModuleQueryDynamicQuery
    {
        public List<LogicConditionModel> LogicConditionModels { get; set; }
        public SearchModuleInfo SearchModuleInfo { get; set; }
        public ParramModuleQueryDynamicQuery()
        {
            LogicConditionModels = new List<LogicConditionModel>();
            SearchModuleInfo = new SearchModuleInfo();
        }
        public List<SearchConditionInstance> SearchConditionInstances {
            get
            {
                if (LogicConditionModels != null)
                    return LogicConditionModels.Select(x => x.SearchConditionInstance).ToList();
                return new List<SearchConditionInstance>();
            }
        }
    }
}
