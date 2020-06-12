using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCore.Entities;

namespace WebModelCore.LogicCondition
{
    public class LogicConditionModel
    {
        public SearchConditionInstance SearchConditionInstance { get; set; }
        public List<WebCore.Entities.CodeInfo> Conditions { get; set; }
        public LogicConditionModel()
        {
            SearchConditionInstance = new SearchConditionInstance();
            Conditions = new List<WebCore.Entities.CodeInfo>();
        }
        public LogicConditionModel(List<WebCore.Entities.CodeInfo> conditions)
        {
            SearchConditionInstance = new SearchConditionInstance();
            Conditions = conditions.ToList();
        }
    }
}
