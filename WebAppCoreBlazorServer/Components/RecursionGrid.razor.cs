using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WebCore.Entities;

namespace WebAppCoreBlazorServer.Components
{
    public partial class RecursionGrid
    {
        [Parameter]
        public List<ModuleFieldInfo> Fields { get; set; }
        public RecursionGrid()
        {
            var fieldParent = Fields.Where(x => x.GroupOnSearch == "Y").OrderBy(x => x.FieldID).ToList();
            RecursionGridModel grids = new RecursionGridModel();
        }
        private void RecursionDataGrid(List<ModuleFieldInfo> fields, RecursionGridModel grids, List<ModuleFieldInfo> fieldAll)
        {            
           
        }
    }
    public class RecursionGridModel
    {
        public List<ModuleFieldInfo> Fields { get; set; }
        public bool IsGroup { get; set; }
        public List<RecursionGridModel> Child { get; set; }
    }
}
