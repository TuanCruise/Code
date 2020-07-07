using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebAppCoreBlazorServer.BUS;
using WebAppCoreBlazorServer.Common;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using WebModelCore.Common;
using WebModelCore.LogicCondition;
using WebModelCore.ModelCheckBox;
using WB.MESSAGE;
using WebAppCoreBlazorServer.Service;
using WB.SYSTEM;

namespace WebAppCoreBlazorServer.Pages
{
    public partial class Search
    {
        private EditContext editContext;
        [Parameter]
        public string modId { get; set; } = "03901";
        //[Parameter]
        public string parramMods { get; set; } = "";
        public string Title { get; set; } = "";
        private string Schema { get; set; } = "audit";
        public List<CheckBoxModel> CheckBoxModels { get; set; }
        public ModuleInfoModel moduleInfoModel { get; set; } = new ModuleInfoModel();
        public ModuleInfoModel ModuleInfo { get; set; } = new ModuleInfoModel();
        public List<ModuleFieldInfo> moduleFieldInfo { get; set; } = new List<ModuleFieldInfo>();
        public List<ModuleFieldInfo> lstControl { get; set; } = new List<ModuleFieldInfo>();
        public List<ModuleFieldInfo> fieldSubmited { get; set; } = new List<ModuleFieldInfo>();
        public List<GroupMod> groupMods { get; set; } = new List<GroupMod>();
        public List<ButtonInfo> btnInfos { get; set; } = new List<ButtonInfo>();
        public List<CodeInfoModel> codeInfos { get; set; } = new List<CodeInfoModel>();

        public List<ButtonInfo> checkBtnViews { get; set; } = new List<ButtonInfo>();
        public List<ButtonInfo> checkColAction { get; set; } = new List<ButtonInfo>();
        public List<ButtonInfo> checkBtnEdits { get; set; } = new List<ButtonInfo>();
        public List<ButtonInfo> checkBtnDels { get; set; } = new List<ButtonInfo>();
        public List<ButtonInfo> checkBtnAssRole { get; set; } = new List<ButtonInfo>();
        public List<ButtonInfo> checkBtnAssUsers { get; set; } = new List<ButtonInfo>();
        //public List<SearchConditionInstance> SearchConditionInstances { get; set; } = new List<SearchConditionInstance>();
        public List<LogicConditionModel> SearchConditionInstances { get; set; } = new List<LogicConditionModel>();
        public List<GroupMod> RoleUser { get; set; } = new List<GroupMod>();

        public SearchModuleInfo modSearch { get; set; }
        public List<CodeInfo> Conditions { get; set; } = new List<CodeInfo>();
        public List<CodeInfo> Logic { get; set; } = new List<CodeInfo>();

        public bool checkBtnDel { get; set; }
        public bool checkBtnView { get; set; }
        public bool checkBtnEdit { get; set; }
        public bool checkBtnAssignRole { get; set; }
        public bool checkBtnAssignUser { get; set; }


        public List<ButtonParamInfo> parramEdit { get; set; } = new List<ButtonParamInfo>();
        public List<ButtonParamInfo> parramView { get; set; } = new List<ButtonParamInfo>();
        public List<ButtonParamInfo> parramDel { get; set; } = new List<ButtonParamInfo>();
        public List<ButtonParamInfo> parramAssignUser { get; set; } = new List<ButtonParamInfo>();
        public List<CodeInfoModel> DataCombobox { get; set; } = new List<CodeInfoModel>();

        public string FieldSelected { get; set; }
        public int currPage { get; set; }
        public int totalRow { get; set; }
        public List<dynamic> DataSearch { get; set; } = new List<dynamic>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")) && modId.ToLower() != ConstMod.ModListHomo.ToLower())
                //{
                //    return RedirectToAction("Login", "Home");
                //    NavigationManager.NavigateTo("PageToRedirect");
                //}
                CheckBoxModels = new List<CheckBoxModel>();
                lstControl = new List<ModuleFieldInfo>();
                moduleFieldInfo = new List<ModuleFieldInfo>();
                HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
                var data = await homeBus.GetModule(modId);

                int userId = int.Parse("0" + ""/*HttpContext.Session.GetString("UserId")*/);
                var groupModUser = await moduleService.GetGroupModByUserId(userId);
                RoleUser = groupModUser;
                moduleInfoModel = homeBus.ConvertFromViewModel(data);
                //Set title
                var arr = new string[1];
                arr[0] = moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo);
                await JSRuntime.InvokeVoidAsync("SetTitle", arr);
                //Hết SetTitle
                Title = moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(await homeBus.LoadAllBtnLanguage());
                var cb = moduleInfoModel.FieldsInfo.Where(x => !String.IsNullOrEmpty(x.ListSource));
                var scdType = moduleInfoModel.FieldsInfo.Select(x => x.ConditionType);
                if (cb.Any())
                {
                    var codeInfoParram = cb.Select(x => new CodeInfoParram
                    {
                        CtrlType = x.ControlType,
                        Name = x.FieldName,
                        ListSource = x.ListSource
                    });
                    //var para = string.Join("", sources);
                    var sourceCodeInfo = cb.Where(x => x.ListSource.Contains(":"));//Lấy những thông tin các ListSource từ DefCode
                    var codeInfoModels = new List<CodeInfoModel>();
                    if (sourceCodeInfo != null && sourceCodeInfo.Any())
                    {
                        var defCodeAll = await homeBus.LoadAllDefCode();
                        var lstSource = sourceCodeInfo.Select(x => x.ListSource).ToList();
                        var cbDefCode = defCodeAll.Where(x => lstSource.Contains(":" + x.CodeType + "." + x.CodeName));
                        foreach (var item in sourceCodeInfo)
                        {
                            codeInfoModels.Add(new CodeInfoModel { Name = item.FieldName, CodeInfos = cbDefCode.Where(x => ":" + x.CodeType + "." + x.CodeName == item.ListSource).ToList() });
                        }
                    }

                    var dataCB = (await moduleService.GetCombobox(codeInfoParram.Where(x => !x.ListSource.Contains(":")).ToList()));//Lấy thông tin các Combobox theo Store
                    codeInfoModels.AddRange(dataCB.Data);
                    DataCombobox = codeInfoModels;
                }

                modSearch = await homeBus.LoadModSearchByModId(modId);
                if (modSearch != null)
                {
                    var parrams = new List<string>();
                    if (!string.IsNullOrEmpty(parramMods))
                    {
                        var btnParramInfo = (List<ButtonParamInfo>)JsonConvert.DeserializeObject<List<ButtonParamInfo>>(parramMods);
                        var temp = btnParramInfo.Select(x => x.FieldName + " = '" + x.Value + "'");
                        parrams.AddRange(temp);
                    }
                    var query = "";
                    if (modSearch.QueryFormat.IndexOf("{0}") > 0)
                    {
                        if (modSearch.QueryFormat.IndexOf("{1}") > 0)
                        {
                            var currPage = 1;
                            var paging = String.Format("  Limit {1} offset {0}", (currPage - 1) * CommonMethod.PageSize, (currPage - 1) * CommonMethod.PageSize + CommonMethod.PageSize);
                            query = string.Format(modSearch.QueryFormat, Schema, parrams.Any() ? String.Join(" AND ", parrams) : " 1=1 ", paging);
                        }
                        else
                        {
                            query = string.Format(modSearch.QueryFormat, Schema, parrams.Any() ? String.Join(" AND ", parrams) : " 1=1 ");
                        }
                    }
                    else
                    {
                        query = String.Format(modSearch.QueryFormat, Schema);
                    }
                    var dataGrid = await moduleService.LoadQueryModule(new ParramModuleQuery { Store = query });

                    //var dataGrid = await _moduleService.LoadQueryModule(new ParramModuleQuery { Store = modSearch.QueryFormat });
                    DataSearch = dataGrid;

                    btnInfos = moduleInfoModel.ButtonsInfo == null ? new List<ButtonInfo>() : moduleInfoModel.ButtonsInfo;
                    var moduleInfo = moduleInfoModel.ModulesInfo == null ? new ModuleInfo() : moduleInfoModel.ModulesInfo;
                    moduleFieldInfo = moduleInfoModel.FieldsInfo == null ? new List<ModuleFieldInfo>() : moduleInfoModel.FieldsInfo;
                    var parrs = moduleInfoModel.ButtonParamsInfo == null ? new List<ButtonParamInfo>() : moduleInfoModel.ButtonParamsInfo;
                    codeInfos = DataCombobox;
                    checkColAction = btnInfos.Where(x => x.ShowOnToolbar == "N").ToList();
                    checkBtnViews = btnInfos.Where(x => x.ButtonName.ToUpper() == EDefModBtn.BTN_VIEW.ToString().ToUpper()).ToList();
                    checkBtnEdits = btnInfos.Where(x => x.ButtonName.ToUpper() == EDefModBtn.BTN_EDIT.ToString().ToUpper()).ToList();
                    checkBtnDels = btnInfos.Where(x => x.ButtonName.ToUpper() == EDefModBtn.BTN_DELETE.ToString().ToUpper()).ToList();
                    checkBtnAssRole = btnInfos.Where(x => x.ButtonName.ToUpper() == EDefModBtn.BTN_ASSIGN_ROLE.ToString().ToUpper()).ToList();
                    checkBtnAssUsers = btnInfos.Where(x => x.ButtonName.ToUpper() == EDefModBtn.BTN_ASSIGN_USER.ToString().ToUpper()).ToList();
                    if (groupMods == null || groupMods.Count == 0)
                    {
                        groupMods = new List<GroupMod>();
                        groupMods.Add(new GroupMod { GroupId = "1", ModId = moduleInfoModel.ModulesInfo.ModuleID });
                        if (checkBtnEdits.Any())
                            groupMods.Add(new GroupMod { GroupId = "1", ModId = checkBtnEdits.First().CallModuleID });
                        if (checkBtnDels.Any())
                            groupMods.Add(new GroupMod { GroupId = "1", ModId = checkBtnDels.First().CallModuleID });

                    }
                    checkBtnDel = checkBtnDels.Any();
                    checkBtnView = checkBtnViews.Any();
                    checkBtnEdit = checkBtnEdits.Any();
                    checkBtnAssignRole = checkBtnAssRole.Any();
                    checkBtnAssignUser = checkBtnAssUsers.Any();
                    parramEdit = new List<ButtonParamInfo>();
                    parramView = new List<ButtonParamInfo>();

                    parramDel = new List<ButtonParamInfo>();
                    parramAssignUser = new List<ButtonParamInfo>();
                    if (checkBtnView)
                    {
                        var parramViews = parrs.Where(x => x.ButtonName == EDefModBtn.BTN_VIEW.ToString());
                        if (parramViews.Any())
                        {
                            parramView = parramViews.ToList();
                        }
                    }
                    if (checkBtnEdit)
                    {
                        var parramEdits = parrs.Where(x => x.ButtonName == EDefModBtn.BTN_EDIT.ToString());
                        if (parramEdits.Any())
                        {
                            parramEdit = parramEdits.ToList();
                        }
                    }
                    if (checkBtnDel)
                    {
                        var parramDels = parrs.Where(x => x.ButtonName == EDefModBtn.BTN_DELETE.ToString());
                        if (parramDels.Any())
                        {
                            parramDel = parramDels.ToList();
                        }
                    }
                    if (checkBtnAssignUser)
                    {
                        var parramAssignUsers = parrs.Where(x => x.ButtonName == EDefModBtn.BTN_ASSIGN_USER.ToString());
                        if (parramAssignUsers.Any())
                        {
                            parramAssignUser = parramAssignUsers.ToList();
                        }
                    }
                }

                //return View("Search", modId);
            }
            catch (Exception ex)
            {

                //return View("Search", modId);
            }
        }

        public async Task SearchModuleFieldInfoOnchange(ChangeEventArgs e)
        {
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            #region Load DropdownList ĐK search. các phép And Or
            var field = moduleFieldInfo.Where(x => x.FieldID == e.Value.ToString());
            if (field != null && field.Any())
            {
                var conditionDefCode = await homeBus.LoadAllDefCode("SCDTYPE", field.First().ConditionType.ToString());
                if (conditionDefCode != null && conditionDefCode.Any())
                {
                    var conditionValues = await homeBus.LoadAllDefCode(conditionDefCode.First().CodeValueName);
                    if (conditionValues != null && conditionValues.Any())
                    {
                        Conditions = conditionValues.ToList();
                    }
                }
            }

            #endregion
        }
        public async Task GoToMod(string modId, string fieldName, string parr = "", string key = "")
        {
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var dataMod = await homeBus.GetModule(modId);
            if (!dataMod.ModulesInfo.Any())
            {
                //return RedirectToAction("Login", "Home");
            }

            var module = homeBus.ConvertFromViewModel(dataMod);
            var btnParram = new List<ButtonParamInfo>();
            btnParram.Add(new ButtonParamInfo { FieldName = fieldName, Value = parr, ModuleID = modId });
            if (module.ModulesInfo.ModuleTypeName == EModuleType.MAINTAIN.ToString())
            {
                //return RedirectToAction("Edit", new { modId = modId, parram = JsonConvert.SerializeObject(btnParram) });
            }
            else
            {
                //return RedirectToAction("Search", new { modId = modId, parramMods = JsonConvert.SerializeObject(btnParram) });
            }
            //return View();
        }

        public async Task BtnSearch(string export = "")
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var data = await homeBus.GetModule(modId);
            var module = homeBus.ConvertFromViewModel(data);
            ModuleInfo = module;
            var cb = module.FieldsInfo.Where(x => !String.IsNullOrEmpty(x.ListSource));
            if (cb.Any())
            {
                var sources = cb.Select(x => x.ListSource).Distinct().ToList();
                var codeInfoParram = cb.Select(x => new CodeInfoParram
                {
                    CtrlType = x.ControlType,
                    Name = x.FieldName,
                    ListSource = x.ListSource
                });
                //var para = string.Join("", sources);
                var dataCB = (await moduleService.GetCombobox(codeInfoParram.ToList()));
                DataCombobox = dataCB.Data;
            }
            modSearch = await homeBus.LoadModSearchByModId(modId);
            if (modSearch != null)
            {
                var fieldEdits = CommonFunction.GetModuleFields(moduleFieldInfo, modId, FLDGROUP.SEARCH_CONDITION);
                List<string> parrams = new List<string>();
                foreach (var field in fieldEdits)
                {
                    if ((field.FieldType == EFieldType.DEC.ToString()) || (field.FieldType == EFieldType.INT.ToString()))
                    {
                        parrams.Add(string.Format("{0}={1}", field.FieldName, (field.Value ?? "").Trim()));
                    }
                    else
                    {
                        parrams.Add(string.Format("{0} LIKE N'%{1}%'", field.FieldName, (field.Value ?? "").Trim()));
                    }

                    var valid = field.ValidateFieldInfo();
                    if (!string.IsNullOrEmpty(valid))
                    {//Nếu validate trường dữ liệu có lỗi.
                        var invalidArr = valid.ToStringArray('.');
                        var fieldName = field.FieldName;
                        string.Join(",", invalidArr.Select(x => fieldName + " " + x));
                    }
                }

                var query = "";
                if (modSearch.QueryFormat.IndexOf("{1}") > 0)
                {
                    if (modSearch.QueryFormat.IndexOf("{2}") > 0)
                    {
                        var paging = String.Format(" Limit {0} offset {1}", (currPage - 1) * CommonMethod.PageSize, (currPage - 1) * CommonMethod.PageSize + CommonMethod.PageSize);
                        query = string.Format(modSearch.QueryFormat, Schema, parrams.Any() ? String.Join(" AND ", parrams) : " 1=1 ", paging);
                    }
                    else
                    {
                        query = string.Format(modSearch.QueryFormat, Schema, parrams.Any() ? String.Join(" AND ", parrams) : " 1=1 ");
                    }
                }
                else
                {
                    query = String.Format(modSearch.QueryFormat, Schema);
                }

                //CurrPage = currPage;
                //FieldSubmited = fieldEdits;
                //Dongpv:Load data to edit
                if (modSearch.GroupModule == "004")
                {
                    Message msg = new Message();
                    msg.ObjectName = Constants.OBJ_SEARCH;
                    msg.MsgType = Constants.MSG_MISC_TYPE;
                    msg.MsgAction = Constants.MSG_SEARCH;

                    msg.Body.Add("SearchObject");
                    msg.Body.Add("CUSTOMER");
                    msg.Body.Add("Condition");
                    msg.Body.Add(" WHERE 1=1");
                    msg.Body.Add("Page");
                    msg.Body.Add(0);

                    var dataGrid = await moduleService.getQuery(msg);
                    DataSearch = dataGrid;
                }
                else
                {
                    var dataGrid = await moduleService.LoadQueryModule(new ParramModuleQuery { Store = query });
                    DataSearch = dataGrid;
                }

                


                //var dataGrid = await moduleService.getQuery(msg);
                //Dongpv:Load data to edit

                //var dataGrid = await moduleService.LoadQueryModule(new ParramModuleQueryDynamicQuery { LogicConditionModels= SearchConditionInstances,SearchModuleInfo= modSearch });
                //if (!string.IsNullOrEmpty(export))
                //{
                //    string pathSaveAs = Path.Combine(_hostingEnvironment.WebRootPath, String.Format("FileTemplate/TemplateExport_{0}.xls", DateTime.Now.ToString("dd-MM-yyyy")));
                //    Data2ExcelFile(dataGrid, module.FieldsInfo, module, pathSaveAs);
                //    return DownloadFile(pathSaveAs);
                //}

                //int userId = int.Parse("0" + HttpContext.Session.GetString("UserId"));
                //var groupModUser = await moduleService.GetGroupModByUserId(userId);
                //ViewBag.RoleUser = groupModUser;
            }
            //return View("Search", modId);
        }
        [JSInvokableAttribute("CallbackConfirm")]
        public void CallbackConfirm(bool result, string modId, List<ButtonParamInfo> keyDels)
        {
            Task.Run(() => Delete(modId, keyDels));
        }
        public async Task ShowConfirmDel(string modId, string keyDels, string title)
        {
            var oKeyDels = JsonConvert.DeserializeObject<List<ButtonParamInfo>>(keyDels);
            var rs = JSRuntime.InvokeAsync<string>("bb_confirm", title, DotNetObjectReference.Create(this), "CallbackConfirm", modId, oKeyDels);
        }
        public async Task DeleteNoConfirm(string modId, string keyDels)
        {
            var oKeyDels = JsonConvert.DeserializeObject<List<ButtonParamInfo>>(keyDels);
            await Delete(modId, oKeyDels);
        }
        public async Task Delete(string modId, List<ButtonParamInfo> keyDels)
        {

            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var modInfo = await homeBus.GetModule(modId);
            var fieldDels = CommonFunction.GetModuleFields(modInfo.FieldsInfo, modId, FLDGROUP.PARAMETER);

            homeBus.AssignParamField(keyDels, fieldDels);

            var dataExcute = await homeBus.LoadExcuteModule(modId);
            if (dataExcute != null)
            {
                //Dongpv: FIX
                //var excute = (await moduleService.DeleteModule(dataExcute.ExecuteStore, fieldDels));
                
                var excute = (await moduleService.DeleteData(modId, dataExcute.ExecuteStore, "", keyDels));
                //Dongpv:

                if (excute.Data != "success")
                {
                    var err = excute.Data.GetError();
                    var errText = await homeBus.GetErrText(err);
                    var textMsg = await homeBus.GetTextLang(errText);
                }
                else
                {
                    await BtnSearch("");
                }
            }
        }
        private bool _CheckAll { get; set; }
        public bool CheckAll {
            get
            {
                return _CheckAll;
            }
            set
            {
                _CheckAll = value;
                foreach (var item in CheckBoxModels)
                {
                    item.Value = _CheckAll;
                }
            }
        }
        private void CheckAllCheckBox()
        {
            foreach (var item in moduleFieldInfo)
            {
                item.IsCheck = CheckAll;
            }
        }

        void CheckManual(ModuleFieldInfo field)
        {
            if (field.IsCheck == false)
            {
                field.IsCheck = true;
            }
            else
            {
                field.IsCheck = false;
                CheckAll = false;
            }
        }
        public async Task CallMod(string callModId, string modSearchId)
        {
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var mod = await homeBus.GetModule(callModId);
            if (mod != null && mod.ModulesInfo.Any())
            {
                if (mod.ModulesInfo.First().UIType == EUITYPE.P.ToString())
                {
                    ModalParameters parameters = new ModalParameters();
                    //{modId}/{modSearchId}
                    parameters.Add("modId", callModId);
                    parameters.Add("modSearchId", modSearchId);
                    Modal.Show<Pages.Edit>(moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo), parameters);
                }
                else
                {
                    NavManager.NavigateTo(String.Format("/Edit/{0}/{1}", callModId, modSearchId));
                }
            }
        }
        public async Task CallMod(string callModId, string modSearchId, List<ButtonParamInfo> parram)
        {
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var mod = await homeBus.GetModule(callModId);
            var checkCheckBoxChecked = CheckBoxModels.Where(x => x.Value);
            if (checkCheckBoxChecked.Count() != 1)
            {
                //JSRuntime.InvokeAsync
                return;
            }
            foreach (var item in parram)
            {
                var value = checkCheckBoxChecked.First().KeyValue;
                var dataRows = ((Newtonsoft.Json.Linq.JContainer)value);
                foreach (var column in dataRows)
                {
                    var columnName = ((Newtonsoft.Json.Linq.JProperty)column).Name;
                    if (columnName.ToUpper() == item.FieldName.ToUpper())
                    {
                        item.Value = (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value ?? "").ToString();
                    }
                }
            }
            if (mod != null && mod.ModulesInfo.Any())
            {
                if (mod.ModulesInfo.First().UIType == EUITYPE.P.ToString())
                {
                    ModalParameters parameters = new ModalParameters();
                    //{modId}/{modSearchId}
                    parameters.Add("modId", callModId);
                    parameters.Add("modSearchId", modSearchId);
                    parameters.Add("fieldNameEdit", "");
                    parameters.Add("parram", JsonConvert.SerializeObject(parram));
                    parameters.Add("pedit", "1");
                    Modal.Show<Pages.Edit>(moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo), parameters);
                }
                else
                {
                    NavManager.NavigateTo(String.Format("/Edit/{0}/{1}/{2}/{3}/{4}", callModId, modSearchId, "", JsonConvert.SerializeObject(parram), "1"));
                }
            }
        }
        public async Task CallMod(string callModId, string modSearchId, string fieldNameEdit, string parram, string pedit)
        {
            HomeBus homeBus = new HomeBus(moduleService, iConfiguration, distributedCache);
            var mod = await homeBus.GetModule(callModId);
            if (mod != null && mod.ModulesInfo.Any())
            {
                if (mod.ModulesInfo.First().UIType == EUITYPE.P.ToString())
                {
                    ModalParameters parameters = new ModalParameters();
                    //{modId}/{modSearchId}
                    parameters.Add("modId", callModId);
                    parameters.Add("modSearchId", modSearchId);
                    parameters.Add("fieldNameEdit", fieldNameEdit);
                    parameters.Add("parram", parram);
                    parameters.Add("pedit", pedit);

                    //Dongpv:Load data to edit
                    Message msg = new Message();
                    msg.ModId = callModId;
                    msg.modSearchId = modSearchId;

                    msg.UserID = "000";
                    msg.BranchID = "000";
                    msg.MsgIP = "000";
                    
                    msg.ObjectName = Constants.OBJ_SEARCH;
                    msg.MsgType = Constants.MSG_MISC_TYPE;
                    msg.MsgAction = Constants.MSG_SEARCH;

                    string strParm = parram.ToString().Replace("[{", "").Replace("}]", "").Replace("\r\n", "").Replace("\"", "");
                    SysUtils.String2ArrayList(ref msg.Body, strParm, ",", ":");

                    //msg.Body.Add("modSearchId");
                    //msg.Body.Add(modSearchId);
                    //msg.Body.Add("fieldNameEdit");
                    //msg.Body.Add(fieldNameEdit);
                    //msg.Body.Add("parram");
                    //msg.Body.Add(parram);
                    //msg.Body.Add("pedit");
                    //msg.Body.Add(pedit);

                    //var dataGrid = await moduleService.getQuery(msg);

                    //Dongpv:Load data to edit

                    Modal.Show<Pages.Edit>(moduleInfoModel.ModulesInfo.ModuleName.GetLanguageTitle(moduleInfoModel.LanguageInfo), parameters);
                }
                else
                {
                    NavManager.NavigateTo(String.Format("/Edit/{0}/{1}/{2}/{3}/{4}", callModId, modSearchId, fieldNameEdit, parram, pedit));
                }
            }
        }
    }
}
