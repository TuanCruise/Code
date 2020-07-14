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
namespace WebAppCoreBlazorServer.Pages
{
    public partial class Edit
    {
        [Parameter]
        public string modId { get; set; } = "02906";

        [Parameter]
        public string modSearchId { get; set; } = "";
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
                editContext = new EditContext(moduleFieldInfoValidate);
                HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
                edit = pEdit == "1" ? true : false;
                var data = Task.Run(() => homeBus.LoadViewBagEdit(modId, modSearchId, "", fieldNameEdit, parram, true, success)).Result;
                if (data != null)
                {
                    moduleInfoModel = data.ModuleInfo == null ? new ModuleInfoModel() : data.ModuleInfo;
                    moduleInfo = data.ModuleInfo == null ? new ModuleInfo() : data.ModuleInfo.ModulesInfo;
                }
                else
                {
                    moduleInfoModel = new ModuleInfoModel();
                    moduleInfo = new ModuleInfo();
                }
                //ViewBag.Title = moduleInfoModel.ModulesInfo == null ? "" : moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo);
                moduleFieldInfo = moduleInfoModel.FieldsInfo.Where(x => x.FieldGroup == FLDGROUP.COMMON.ToString()).Where(x => x.HideWeb != "Y").OrderBy(x => x.Order).ToList();
                if (moduleFieldInfo == null)
                    moduleFieldInfo = new List<ModuleFieldInfo>();
                codeInfos = data.DataCombobox;
                dataControl = data.DataControl;
                //Set title
                var arr = new string[1];
                arr[0] = moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo); ;
                Task.Run(() => JSRuntime.InvokeVoidAsync("SetTitle", arr));
                //Hết SetTitle
                foreach (var field in moduleFieldInfo)
                {
                    if (dataControl != null)
                    {
                        foreach (var item in dataControl)
                        {
                            var dataRows = ((Newtonsoft.Json.Linq.JContainer)item);

                            foreach (var column in dataRows)
                            {
                                var columnName = ((Newtonsoft.Json.Linq.JProperty)column).Name;
                                if (columnName.ToUpper() == field.FieldName.ToUpper())
                                {
                                    if (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value != null)
                                    {
                                        field.Value += (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value).ToString().Trim() + ",";
                                    }
                                }
                            }
                        }
                    }
                    if (field.Value != null)
                    {
                        field.Value = field.Value.Trim(',');
                    }
                }

                var lstSources = moduleFieldInfo.Where(x => !String.IsNullOrEmpty(x.ListSource) && x.ListSource.IndexOf("(") > 0 && x.ListSource.IndexOf("()") < 0).ToList();//Lấy các list source dạng store có truyền vào tên field
                var fieldChild=moduleFieldInfo.Where(x => x.FieldChilds != null).Select(x => x.FieldChilds);
                if (fieldChild.Any())
                {
                    foreach (var child in fieldChild)
                    {
                        var sourceChild = child.Where(x => !String.IsNullOrEmpty(x.ListSource) && x.ListSource.IndexOf("(") > 0 && x.ListSource.IndexOf("()") < 0);
                        if (sourceChild.Any())
                        {
                            lstSources.AddRange(sourceChild.ToList());
                        }
                        
                    }
                   
                }
                foreach (var item in lstSources)
                {
                    var sourceParr = item.ListSource.Substring(item.ListSource.IndexOf("(") + 1, item.ListSource.IndexOf(")") - item.ListSource.IndexOf("(") - 1).Split(",");
                    foreach (var source in sourceParr)
                    {
                        var checkItems = moduleFieldInfo.Where(x => ":" + x.FieldID.ToUpper() == source.ToUpper());
                        if (checkItems.Any())
                        {
                            item.ListSource = item.ListSource.Replace(source, checkItems.First().Value);
                        }
                    }
                }
                keyEdit = data.KeyEdit;
                //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
                //{
                //    return RedirectToAction("Login", "Home");
                //}
                //await LoadViewBagEdit(modId, modSearchId, subModId, fieldNameEdit, parram, edit, success);
                //codeInfos = DataCombobox;
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
                //Dongpv: 
                excute = (await moduleService.SaveData(modId, store, keyEdit, moduleFieldInfo));
            }
            else
            {
                store = modMaintain.EditUpdateStore;
                //Dongpv: 
                excute = (await moduleService.UpdateData(modId, store, keyEdit, moduleFieldInfo));
            }

            //var excute = (await moduleService.SaveEditModule(modId, store, keyEdit, fieldEdits));
            //Dongpv:   
            //var excute = (await moduleService.SaveEditModule(modId, store, keyEdit, moduleFieldInfo));            
            //Dongpv:

            if (excute.Data != "success")
            {
                var err = excute.Data.GetError();
                var redrectToSearch = string.IsNullOrEmpty(parram) ? modMaintain.AddRepeatInput : modMaintain.EditRepeatInput;
                JSRuntime.InvokeAsync<string>("bb_alert", excute.Data, DotNetObjectReference.Create(this), "AlertCallBack", redrectToSearch);
            }
            else
            {
                if (modMaintain.ShowSuccess == "Y")
                {
                    var title = "Lưu dữ liệu thành công";
                    JSRuntime.InvokeAsync<string>("bb_alert", title, DotNetObjectReference.Create(this), "AlertCallBack");
                    //Modal.Show<Pages.Edit>(moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo), parameters);
                }

            }

        }
        [CascadingParameter] Blazored.Modal.BlazoredModalInstance BlazoredModal { get; set; }

        void Close() => BlazoredModal.Close(Blazored.Modal.Services.ModalResult.Ok(true));
        void Cancel() => BlazoredModal.Cancel();

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
