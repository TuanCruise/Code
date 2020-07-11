using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;

namespace WebAppCoreBlazorServer.Components
{
    public partial class EditItemGridComponent
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
        public System.Data.DataTable DataSearchSource { get; set; }
        [Parameter]
        public System.Data.DataRow RowEdit { get; set; }
        [Parameter]
        public string ModName { get; set; } = "";
        [Parameter]
        public ModuleInfoModel ModuleInfo { get; set; }
        [Parameter]
        public string keyEdit { get; set; } = "";
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        public async Task CancelField()
        {
            if (RowEdit != null)
            {
                if (RowEdit["CoreSource"].ToString().Length==2)
                {
                    RowEdit["CoreSource"] = RowEdit["CoreSource"].ToString().Replace("4","");
                }
                else
                {
                    RowEdit["CoreSource"] = "0";
                }
            }
        }
        public async Task SaveField()
        {
            if (RowEdit == null && DataSearchSource!=null)
            {
                var row = DataSearchSource.NewRow();
                foreach (var item in Field.FieldChilds)
                {
                    row[item.FieldName.ToLower()] = item.Value;
                }
                row["CoreSource"] = 1;
                DataSearchSource.Rows.Add(row);
                DataSearchSource.AcceptChanges();
            }
            else
            {
                foreach (var item in Field.FieldChilds)
                {
                    RowEdit[item.FieldName.ToLower()] = item.Value;
                }
                RowEdit["CoreSource"] = 2;
            }
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
    }
}
