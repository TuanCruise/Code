using System.Collections.Generic;
using WebCore.Entities;

namespace WebModelCore.DefTask
{
    public class DefTaskModel
    {
        public DefTasks DefTasks { get; set; }
        public List<DefTasks> DefTasksChild { get; set; }
        public DefTaskModel()
        {
            DefTasksChild = new List<DefTasks>();
        }
    }
}
