using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using GridBlazor;
using Microsoft.AspNetCore.Components;
using WebAppCoreBlazorServer.Pages;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;

namespace WebAppCoreBlazorServer.Components
{
    public partial class ModSearchGridGroup
    {
        public SearchModuleInfo modSearch { get; set; }
        [Parameter]
        public List<CodeInfoModel> codeInfos { get; set; } = new List<CodeInfoModel>();
        public IEnumerable<ButtonInfo> checkColAction { get; set; }
        [Parameter]
        public List<ButtonInfo> btnInfos { get; set; } = new List<ButtonInfo>();
        [Parameter]
        public List<dynamic> DataSearch { get; set; } = new List<dynamic>();
        public int totalRow { get; set; }
        public string modId { get; set; }
        [Parameter]
        public Search Search { get; set; }
        [Parameter]
        public List<ModuleFieldInfo> ModuleFieldInfos{ get; set; }
        [Parameter]
        public ModuleInfoModel ModuleInfoModel { get; set; }
        protected override Task OnInitializedAsync()
        {
            modSearch=Search.modSearch;
            return base.OnInitializedAsync();
        }
    }
}
