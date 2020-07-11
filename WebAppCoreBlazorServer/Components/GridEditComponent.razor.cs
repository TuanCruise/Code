using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using Blazored.Modal.Services;

namespace WebAppCoreBlazorServer.Components
{
    public partial class GridEditComponent
    {
        [Parameter]
        public ModuleFieldInfo Field { get; set; }
        [Parameter]
        public List<ModuleFieldInfo> Fields { get; set; }
        [Parameter]
        public List<ModuleFieldInfo> FieldParent { get; set; }
        [Parameter]
        public List<CodeInfoModel> DataComboBoxs { get; set; }
        [Parameter]
        public List<LanguageInfo> LanguageInfos { get; set; }
        [Parameter]
        public string SubMod { get; set; }
        [Parameter]
        public List<dynamic> DataSearch { get; set; }
        //[Parameter]
        public System.Data.DataTable DataSearchSource { get; set; }
        [Parameter]
        public string ModName { get; set; } = "";
        [Parameter]
        public ModuleInfoModel ModuleInfo { get; set; }
        [Parameter]
        public string keyEdit { get; set; } = "";
        public int TotalRow { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Field.ListSource))
            {
                DataSearchSource = (await moduleService.Store2DataTable(new ParramModuleQuery { Store = Field.ListSource }));
                var column = new System.Data.DataColumn("CoreSource", typeof(int));
                column.DefaultValue = 0;
                DataSearchSource.Columns.Add(column);
            }
        }
      
        public async Task EditField(DataRow row)
        {
            var data = DataSearchSource.Select("CoreSource > 0");
            if (data.Any())
            {
                foreach (DataRow item in data)
                {
                    if (item["CoreSource"].ToString().Length == 2)
                    {
                        item["CoreSource"] = item["CoreSource"].ToString().Replace("4", "");
                    }
                    else
                    {
                        item["CoreSource"] = "0";
                    }
                }
            }
            row["CoreSource"] = "4"+ row["CoreSource"];
        }
        public async Task DeleteField(DataRow row)
        {
            row["CoreSource"] = 3;
            Field.Value = GetEditFieldJson();
        }
        public string GetEditFieldJson()
        {
            var data = DataSearchSource.Select("CoreSource > 0");
            if (data.Any())
            {
                return JsonConvert.SerializeObject(data.CopyToDataTable());
            }
            return "";
        }
        public async Task ShowModAdd(string callModId)
        {
            ModalParameters parameters = new ModalParameters();
            //{modId}/{modSearchId}
            parameters.Add("modId", callModId);
            parameters.Add("modSearchId", "");
            Modal.Show<Pages.Edit>("Edit", parameters);
        }
    }
}
