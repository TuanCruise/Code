using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebAppCoreBlazorServer.BUS;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;

namespace WebAppCoreBlazorServer.Components
{
    public partial class GenControl
    {
        [Parameter]
        public ModuleFieldInfo field { get; set; }
        [Parameter]
        public List<ModuleFieldInfo> fields { get; set; }
        [Parameter]
        public List<ModuleFieldInfo> FieldParent { get; set; }
        [Parameter]
        public List<CodeInfoModel> dataComboBoxs { get; set; }
        [Parameter]
        public List<LanguageInfo> languageInfos { get; set; }
        [Parameter]
        public ModuleInfoModel ModuleInfo { get; set; }
        [Parameter]
        public string subMod { get; set; }
        [Parameter]
        public List<dynamic> dataEdit { get; set; }
        [Parameter]
        public string modName { get; set; } = "";
        [Parameter]
        public string keyEdit { get; set; } = "";
        [Parameter]
        public int pageSlide { get; set; } = 0;
        [Parameter]
        public List<ModuleFieldInfo> fieldSubmited { get; set; } = null;
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        [JSInvokable()]
        public Task invokeFromJS()
        {

            //field.Value = DateTime.Now.ToString("dd/MM/yyyy");
            //field.Value = "New value";
            //StateHasChanged();
            //return CompletedTask;
            return null;
        }
        public async Task ControlOnchange(ChangeEventArgs e)
        {
            field.Value = (e.Value ?? "").ToString();
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var codeInfoParrams = new List<CodeInfoParram>();
            if (!string.IsNullOrEmpty(field.Callback))
            {
                var codeInfoParram = new CodeInfoParram
                {
                    Name = field.FieldName,
                    CtrlType = field.ControlType,
                    ListSource = field.Callback,
                    Parrams = (e.Value ?? "").ToString()
                };
                codeInfoParrams.Add(codeInfoParram);
                var loadCallBacks = await homeBus.LoadDataListSourceControl(codeInfoParrams);
                foreach (var item in loadCallBacks)
                {
                    if (!String.IsNullOrEmpty(item.DataCallBack))
                    {
                        var dataCallBackControls = JsonConvert.DeserializeObject<List<dynamic>>(item.DataCallBack);
                        foreach (var controlCallBack in dataCallBackControls)
                        {
                            var name = ((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)controlCallBack).First).Name;
                            var value = ((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)controlCallBack).First).Value;
                            var arr = new string[2];
                            arr[0] = name;
                            arr[1] = value == null ? "" : value.ToString();
                            await JSRuntime.InvokeVoidAsync("SetValueControl", arr);
                        }
                    }
                }
            }
        }
    }
}
