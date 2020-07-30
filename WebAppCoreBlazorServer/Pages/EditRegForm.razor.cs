using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using WebAppCoreBlazorServer.BUS;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using WebModelCore.Common;
using WebAppCoreBlazorServer.Common;
using WebAppCoreBlazorServer.Service;

namespace WebAppCoreBlazorServer.Pages
{
    public partial class EditRegForm 
    {
        [Parameter]
        public string modId { get; set; } = "02501";

        [Parameter]
        public string modSearchId { get; set; } = "03501";
        [Parameter]
        public string fieldNameEdit { get; set; } = "";
        [Parameter]
        public string parram { get; set; } = "";
        [Parameter]
        public string pEdit { get; set; } = "1";
        public bool edit { get; set; }
        public int success { get; set; } = 0;
        //string modId, string modSearchId, string subModId, string fieldNameEdit, string parram, bool edit = true, int success = 0
        public List<string> ErrorValidate { get; set; }
        private ModuleInfoModel moduleInfoModel { get; set; }
        //ViewBag.Title = moduleInfoModel.ModulesInfo == null ? "" : moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo);
        private ModuleInfo moduleInfo { get; set; }
        private List<ModuleFieldInfo> moduleFieldInfo { get; set; }
        private List<CodeInfoModel> codeInfos;
        private List<dynamic> dataControl { get; set; } //= (List<dynamic>)ViewBag.DataControl;
        private string keyEdit { get; set; }// = @ViewBag.KeyEdit;
        private EditContext editContext;
        private bool formInvalid = true;

        public User user { get; set; }

        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            formInvalid = !editContext.Validate();
            StateHasChanged();
        }
        ModuleFieldInfo moduleFieldInfoValidate = new ModuleFieldInfo();
        protected override void OnInitialized()
        {
            try
            {
                user = new User();
            }
            catch (Exception e)
            {

            }
        }
        public async Task Save()
        {
            //Dongpv:FIX TO RUN
            //ErrorValidate = new List<string>();
            //foreach (var item in moduleFieldInfo)
            //{
            //    var validator = new Common.FluentValidation();

            //    var rsErr = validator.Validate(item);
            //    if (!rsErr.IsValid)
            //    {
            //        ErrorValidate.AddRange(rsErr.Errors.Select(x => x.ErrorMessage));
            //    }
            //}
            //if (ErrorValidate.Any())
            //{
            //    return;
            //}
            //Dongpv:FIX TO RUN

            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);

            string validate = "";

            var modMaintain = await homeBus.LoadMaintainModuleInfo(modId);
            var store = "";

            //Dongpv: 
            var excute = new RestOutput<string>();

            if (string.IsNullOrEmpty(keyEdit))
            {
                store = modMaintain.AddInsertStore;                
                excute = (await moduleService.SaveData(modId, store, keyEdit, moduleFieldInfo));
            }
            else
            {
                store = modMaintain.EditUpdateStore;                
                excute = (await moduleService.UpdateData(modId, store, keyEdit, moduleFieldInfo));

            }          

            if (excute.Data != "success" && excute.ResultCode != 1)
            {
                var err = excute.Data.GetError();
                var redrectToSearch = string.IsNullOrEmpty(parram) ? modMaintain.AddRepeatInput : modMaintain.EditRepeatInput;
                JSRuntime.InvokeAsync<string>("bb_alert", excute.Data, DotNetObjectReference.Create(this), "AlertCallBack", redrectToSearch);
            }
            else
            {
                if (modMaintain.ShowSuccess == "Y" || excute.ResultCode == 1)
                {
                    var title = "Lưu dữ liệu thành công";
                    JSRuntime.InvokeAsync<string>("bb_alert", title, DotNetObjectReference.Create(this), "AlertCallBack");                    
                }
            }

        }

        [CascadingParameter] Blazored.Modal.BlazoredModalInstance BlazoredModal { get; set; }

        void Close() => BlazoredModal.Close(Blazored.Modal.Services.ModalResult.Ok(true));
        
        void Cancel()
        {
            if (moduleInfo.UIType == EUITYPE.P.ToString())
                BlazoredModal.Cancel();
            else
                NavManager.NavigateTo(String.Format("/Search/{0}", modSearchId));

        }

        [JSInvokableAttribute("AlertCallBack")]
        public void AlertCallBack(string redrectToSearch)
        {
            if (redrectToSearch == "Y")
            {
                Close();
                //NavManager.NavigateTo(String.Format("/Search/{0}", modSearchId));
            }
        }
    }
}
