using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Components;
using WebAppCoreBlazorServer.BUS;
using WebModelCore;

namespace WebAppCoreBlazorServer.Pages
{
    public partial class ModTreeview
    {
        [Parameter]
        public string ModId { get; set; }
        public ModuleInfoViewModel ModuleInfo { get; set; }
        public ModTreeview ModTreeviewInfo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ModId))
            {
                ModuleInfo = await moduleService.GetModule(ModId);
                HomeBus homeBus = new HomeBus(moduleService, iConfiguration,distributedCache);
                homeBus.GetAllModWorkFolow();
            }
        }
    }
}
