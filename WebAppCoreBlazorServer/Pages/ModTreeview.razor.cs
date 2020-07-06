using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.JSInterop;
using WebAppCoreBlazorServer.BUS;
using WebModelCore;
using WebModelCore.ModTreeViewModel;

namespace WebAppCoreBlazorServer.Pages
{
    public partial class ModTreeview
    {
        [Parameter]
        public string ModId { get; set; }
        public string RightModId { get; set; }
        public ModuleInfoViewModel ModuleInfo { get; set; }
        public ModuleInfoViewModel RightModInfo { get; set; }
        public ModTreeview ModTreeviewInfo { get; set; }
        public List<TreeviewInfo> TreeView { get; set; }
        private string RawTreeView { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ModId))
            {
                ModuleInfo = await moduleService.GetModule(ModId);
                HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
                var tree = await homeBus.GetModTreeviewById(ModId);
                if (tree != null)
                {
                    var query = new ParramModuleQuery { Store = tree.ExecuteTreestore };
                    TreeView = await moduleService.GetDataTreeviewInfo(query);
                    //RawTreeView = CreateTreeView();
                }
            }

        }
        public async Task UpdateChangeMod(string modId)
        {
            RightModId = modId;
            RightModInfo = await moduleService.GetModule(RightModId);
            StateHasChanged();
        }
        
        private string CreateTreeView()
        {
            string str = @"<div class='card'>";
            var parent = TreeView.Where(x => String.IsNullOrEmpty(x.ParentId) || x.ParentId == "000000");
            if (parent.Any())
            {
                foreach (var item in parent)
                {
                    str += @"<div class=''><h5 class='mb-0 card-header'><a class='btn btn-link' data-toggle='collapse' href='#item-" + item.Id + "' role='button' aria-expanded='false' aria-controls='collapseExample'>" + item.TreeName + "</a> </h5>";
                    var checkHasParent = TreeView.Where(x => x.ParentId == item.Id);
                    if (checkHasParent.Any())
                    {
                        str += CreateTreeViewChildrend(item.Id);
                    }
                    str += "</div>";
                }
            }
            str += "</div>";
            return str;
        }
        private string CreateTreeViewChildrend(string parentId)
        {
            var str = "";
            var child = TreeView.Where(x => x.ParentId == parentId);
            if (child.Any())
            {
                str += @"<div class='collapse' id='item-" + parentId + "'>";
                foreach (var item in child)
                {
                    //  @onclick="@(()=>Search.CallMod(callModId,modId))"
                    str += "<h5 class='mb-0'><a class='btn btn-link'  role='button' aria-expanded='false' aria-controls='collapseExample' onclick='SetRightMod(" + item.ModId.Trim() + ")'>" + item.TreeName + "</a></h5>";
                    var checkHasParent = TreeView.Where(x => x.ParentId == item.Id);
                    if (checkHasParent.Any())
                    {
                        str += CreateTreeViewChildrend(item.Id);
                    }
                }
                str += "</div>";
            }
            return str;
        }
    }
}
