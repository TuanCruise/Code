using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using WebModelCore;
using Newtonsoft.Json;
using WebAppCoreBlazorServer.Common.Utils;
using WebAppCoreBlazorServer.Service;
using Microsoft.Extensions.Configuration;
using WebCore.Entities;
using WebModelCore.CodeInfo;

namespace WebAppCoreBlazorServer.BUS
{
    public class HomeBus
    {
        private IConfiguration _Configuration;
        private IModuleService _moduleService;
        private IDistributedCache _distributedCache;
        public HomeBus(IModuleService moduleService, IConfiguration Configuration, IDistributedCache distributedCache)
        {
            _moduleService = moduleService;
            _Configuration = Configuration;
            _distributedCache = distributedCache;
        }
        public async Task<ExecProcModuleInfo> LoadExcuteModule(string modId)
        {
            string key = ECacheKey.ExecProcModuleInfo.ToString() + modId;
            var cachedData = _distributedCache.GetString(key);
            if (cachedData != null && cachedData != "[]")
            {
                var exec = JsonConvert.DeserializeObject<List<ExecProcModuleInfo>>(cachedData);
                return exec.First();
            }
            else
            {
                var exec = await _moduleService.LoadExcuteModule(modId);
                RedisUtils.SetCacheData(_distributedCache, _Configuration, exec, key);
                return exec.First();
            }

        }
        public async Task<ModuleInfoViewModel> GetModule(string modId)
        {
            try
            {
                string key = ECacheKey.ModuleInfo.ToString() + modId;
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var module = JsonConvert.DeserializeObject<ModuleInfoViewModel>(cachedData);
                    return module;
                }
                else
                {
                    var model = await _moduleService.GetModule(modId);
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, model, key);
                    return model;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<LanguageInfo>> LoadAllBtnLanguage()
        {
            string key = ECacheKey.BtnLanguageInfo.ToString();
            var cachedData = _distributedCache.GetString(key);
            if (cachedData == null)
            {
                await LoadBtnLanguage();
            }
            cachedData = _distributedCache.GetString(key);
            var languages = JsonConvert.DeserializeObject<List<LanguageInfo>>(cachedData);
            return languages;
        }
        public async Task LoadBtnLanguage()
        {
            try
            {
                string key = ECacheKey.BtnLanguageInfo.ToString();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var module = JsonConvert.DeserializeObject<List<LanguageInfo>>(cachedData);
                }
                else
                {
                    var model = await _moduleService.GetAllBtnLanguageText();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, model, key);
                }
            }
            catch (Exception e)
            {
            }
        }
        public ModuleInfoModel ConvertFromViewModel(ModuleInfoViewModel viewModel)
        {
            if (viewModel == null)
                return new ModuleInfoModel();
            var module = new ModuleInfoModel();
            var check = viewModel.ModulesInfo.Where(x => x.SubModule == "MMN");
            if (check.Any())
            {
                module.ModulesInfo = check.First();
            }
            else
            {
                if (viewModel.ModulesInfo != null && viewModel.ModulesInfo.Any())
                {
                    module.ModulesInfo = viewModel.ModulesInfo.First();
                }
            }
            module.FieldsInfo = viewModel.FieldsInfo.Where(x => x.ModuleID == module.ModulesInfo.ModuleID).ToList();
            module.ButtonsInfo = viewModel.ButtonsInfo;
            module.ButtonParamsInfo = viewModel.ButtonParamsInfo;
            var lstLanguage = Task.Run(() => LoadAllBtnLanguage()).Result;
            if (lstLanguage != null)
                viewModel.LanguageInfo.AddRange(lstLanguage);
            module.LanguageInfo = viewModel.LanguageInfo;
            var btnLang = Task.Run(() => LoadAllBtnLanguage()).Result;
            if (btnLang != null)
            {
                module.LanguageInfo.AddRange(btnLang);
            }

            return module;
        }
        /// <summary>
        /// Load Tất cả defcode vào cache
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeInfo>> LoadAllDefCode()
        {
            try
            {
                string key = ECacheKey.DefCode.ToString();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var defcodes = JsonConvert.DeserializeObject<List<CodeInfo>>(cachedData);
                    return defcodes;
                }
                else
                {
                    var defcodes = await _moduleService.GetAllDefCode();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defcodes, key);
                    return defcodes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// Lấy Defcode từ All cache theo code.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<CodeInfo>> LoadAllDefCode(string code)
        {
            try
            {
                string key = ECacheKey.DefCode.ToString();
                var cachedData = _distributedCache.GetString(key);
                var defcodes = new List<CodeInfo>();
                if (cachedData != null)
                {
                    defcodes = JsonConvert.DeserializeObject<List<CodeInfo>>(cachedData);
                }
                else
                {
                    defcodes = await _moduleService.GetAllDefCode();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defcodes, key);
                }
                return defcodes.Where(x => x.CodeName == code).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy Defcode từ All cache theo code.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<CodeInfo>> LoadAllDefCode(string code, string value)
        {
            try
            {
                string key = ECacheKey.DefCode.ToString();
                var cachedData = _distributedCache.GetString(key);
                var defcodes = new List<CodeInfo>();
                if (cachedData != null)
                {
                    defcodes = JsonConvert.DeserializeObject<List<CodeInfo>>(cachedData);
                }
                else
                {
                    defcodes = await _moduleService.GetAllDefCode();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defcodes, key);
                }
                return defcodes.Where(x => x.CodeName == code && x.CodeValue == value).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<CodeInfo>> LoadAllDefCode(List<string> codes)
        {
            try
            {
                string key = ECacheKey.DefCode.ToString();
                var cachedData = _distributedCache.GetString(key);
                var defcodes = new List<CodeInfo>();
                if (cachedData != null)
                {
                    defcodes = JsonConvert.DeserializeObject<List<CodeInfo>>(cachedData);
                }
                else
                {
                    defcodes = await _moduleService.GetAllDefCode();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defcodes, key);
                }
                return defcodes.Where(x => codes.Contains(x.CodeName)).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<SearchModuleInfo> LoadModSearchByModId(string modId)
        {
            try
            {
                string key = ECacheKey.ModuleSearchInfo.ToString() + modId;
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var module = JsonConvert.DeserializeObject<List<SearchModuleInfo>>(cachedData);
                    return module.FirstOrDefault();
                }
                else
                {
                    var modSearchs = await _moduleService.LoadModSearchByModId(modId);
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, modSearchs, key);
                    return modSearchs.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<DataViewEdit> LoadViewBagEdit(string modId, string modSearchId, string subModId, string fieldNameEdit, string parram, bool edit, int success)
        {
            try
            {
                var result = new DataViewEdit();
                var dataModule = await GetModule(modId);
                var fields = CommonFunction.GetModuleFields(dataModule.FieldsInfo, modId, FLDGROUP.PARAMETER);
                foreach (var item in fields)
                {//Set lại giá trị null cho field
                    item.Value = "";
                }
                AssignParamField(parram, fields);
                var dataMaintainInfo = (await _moduleService.LoadDataMainTainModule(modId, subModId, parram, fields));
                result.DataControl = dataMaintainInfo;
                result.ModId = modId;
                result.SubModId = subModId;
                result.ModSearchId = modSearchId;
                result.KeyEdit = parram;
                result.Edit = edit;
                result.FieldNameEdit = fieldNameEdit;
                var data = await GetModule(modId);
                var module = ConvertFromViewModel(data);
                result.ModuleInfo = module;
                result.Success = success;
                var cb = module.FieldsInfo.Where(x => !String.IsNullOrEmpty(x.ListSource));
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
                        var defCodeAll = await LoadAllDefCode();
                        var lstSource = sourceCodeInfo.Select(x => x.ListSource).ToList();
                        var cbDefCode = defCodeAll.Where(x => lstSource.Contains(":" + x.CodeType + "." + x.CodeName));
                        foreach (var item in sourceCodeInfo)
                        {
                            codeInfoModels.Add(new CodeInfoModel { Name = item.FieldName, CodeInfos = cbDefCode.Where(x => ":" + x.CodeType + "." + x.CodeName == item.ListSource).ToList() });
                        }
                    }
                    var dataCB = (await _moduleService.GetCombobox(codeInfoParram.Where(x => !x.ListSource.Contains(":")).ToList()));//Lấy thông tin các Combobox theo Store
                    if (dataCB.Data != null)
                    {
                        codeInfoModels.AddRange(dataCB.Data);
                    }
                    result.DataCombobox = codeInfoModels;
                }
                return result;
            }
            catch (Exception e)
            {

                return new DataViewEdit();
            }
        }


        public async Task<List<CodeInfoModel>> LoadDataListSourceControl(List<CodeInfoParram> sourceCodeInfo)
        {
            try
            {
                var codeInfoModels = new List<CodeInfoModel>();
                if (sourceCodeInfo != null && sourceCodeInfo.Any())
                {
                    var defCodeAll = await LoadAllDefCode();
                    var lstSource = sourceCodeInfo.Select(x => x.ListSource).ToList();
                    var cbDefCode = defCodeAll.Where(x => lstSource.Contains(":" + x.CodeType + "." + x.CodeName));
                    foreach (var item in sourceCodeInfo)
                    {
                        codeInfoModels.Add(new CodeInfoModel { Name = item.Name, CodeInfos = cbDefCode.Where(x => ":" + x.CodeType + "." + x.CodeName == item.ListSource).ToList() });
                    }
                }
                var dataCB = (await _moduleService.GetCombobox(sourceCodeInfo.Where(x => !x.ListSource.Contains(":")).ToList()));//Lấy thông tin các Combobox theo Store
                if (dataCB.Data != null)
                {
                    codeInfoModels.AddRange(dataCB.Data);
                }

                return codeInfoModels;
            }
            catch (Exception e)
            {
                return new List<CodeInfoModel>();
            }
        }

        /// <summary>
        /// Gán Parram field trường hợp update, delete
        /// </summary>
        /// <param name="parram"></param>
        /// <param name="fields"></param>
        public void AssignParamField(string parram, List<ModuleFieldInfo> fields)
        {
            if (string.IsNullOrEmpty(parram))
            {
                return;
            }
            try
            {
                var lstParram = JsonConvert.DeserializeObject<List<ButtonParamInfo>>(parram);
                foreach (var item in fields)
                {
                    foreach (var prr in lstParram)
                    {
                        if (item.FieldName.ToUpper() == prr.FieldName.ToUpper())
                        {
                            item.Value += prr.Value + ",";
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        /// <summary>
        /// Gán Parram field trường hợp update, delete
        /// </summary>
        /// <param name="parram"></param>
        /// <param name="fields"></param>
        public void AssignParamField(List<ButtonParamInfo> parram, List<ModuleFieldInfo> fields)
        {
            if (parram == null || !parram.Any())
            {
                return;
            }
            try
            {
                foreach (var item in fields)
                {
                    foreach (var prr in parram)
                    {
                        if (item.FieldName.ToUpper() == prr.FieldName.ToUpper())
                        {
                            item.Value += prr.Value + ",";
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task<MaintainModuleInfo> LoadMaintainModuleInfo(string modId)
        {
            string key = ECacheKey.MaintainModuleInfo.ToString() + modId;
            var cachedData = _distributedCache.GetString(key);
            if (cachedData != null)
            {
                var exec = JsonConvert.DeserializeObject<List<MaintainModuleInfo>>(cachedData);
                return exec.First();
            }
            else
            {
                var exec = await _moduleService.LoadMaintainModuleInfo(modId);
                RedisUtils.SetCacheData(_distributedCache, _Configuration, exec, key);
                return exec.First();
            }
        }
        public async Task<string> GetErrText(int error)
        {
            string key = ECacheKey.ErrorInfo.ToString();
            var cachedData = _distributedCache.GetString(key);
            var lstAllError = new List<ErrorInfo>();
            if (cachedData != null)
            {
                lstAllError = JsonConvert.DeserializeObject<List<ErrorInfo>>(cachedData);
            }
            else
            {
                lstAllError = await _moduleService.GetAllError();
                RedisUtils.SetCacheData(_distributedCache, _Configuration, lstAllError, key);
            }
            if (lstAllError != null && lstAllError.Any())
            {
                var err = lstAllError.Where(x => x.ErrorCode == error);
                if (err != null && err.Any())
                {
                    return err.First().ErrorName;
                }
            }

            return "Không tìm thấy lỗi";
        }

        public async Task<string> GetTextLang(string text)
        {
            string key = ECacheKey.LanguageInfo.ToString();
            var cachedData = _distributedCache.GetString(key);
            var lsAllLanguage = new List<LanguageInfo>();
            if (cachedData != null)
            {
                lsAllLanguage = JsonConvert.DeserializeObject<List<LanguageInfo>>(cachedData);
            }
            else
            {
                lsAllLanguage = await _moduleService.GetAllTextLanguage();
                RedisUtils.SetCacheData(_distributedCache, _Configuration, lsAllLanguage, key);
            }
            if (lsAllLanguage != null && lsAllLanguage.Any())
            {
                var lang = lsAllLanguage.Where(x => x.LanguageName == "DEFERROR$" + text);
                if (lang != null && lang.Any())
                {
                    return lang.First().LanguageValue;
                }
            }
            return text;
        }
        /// <summary>
        /// Load Tất cả defTask vào cache
        /// </summary>
        /// <returns></returns>
        public async Task<List<DefTasks>> LoadAllDefTasks()
        {
            try
            {
                string key = ECacheKey.DefTasks.ToString();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var defTasks = JsonConvert.DeserializeObject<List<DefTasks>>(cachedData);
                    return defTasks;
                }
                else
                {
                    var defTasks = await _moduleService.GetAllDefTasks();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defTasks, key);
                    return defTasks;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// Lấy thông tin deftask theo modId
        /// </summary>
        /// <returns></returns>
        public async Task<List<DefTasks>> LoadDefTasksByModId(string modId)
        {
            try
            {
                string key = ECacheKey.DefTasks.ToString();
                var defTasks = new List<DefTasks>();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData == null)
                {
                    var defTaskAll = await LoadAllDefTasks();
                    return defTaskAll.Where(x => x.Modid == modId).ToList();
                }
                else
                {
                    var defTaskAll = JsonConvert.DeserializeObject<List<DefTasks>>(cachedData);
                    return defTaskAll.Where(x => x.Modid == modId).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy thông tin ModWorkFollow theo modId
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModWorkflow>> LoadModWorkFollowByModId(string modId)
        {
            try
            {
                string key = ECacheKey.ModWorkFlow.ToString();
                var defTasks = new List<ModWorkflow>();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData == null)
                {
                    var modWorkflows = await GetAllModWorkFolow();
                    return modWorkflows.Where(x => x.Modid == modId).ToList();
                }
                else
                {
                    var modWorkflows = JsonConvert.DeserializeObject<List<ModWorkflow>>(cachedData);
                    return modWorkflows.Where(x => x.Modid == modId).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Load Tất cả defTask vào cache
        /// </summary>
        /// <returns></returns>
        public async Task<List<ModWorkflow>> GetAllModWorkFolow()
        {
            try
            {
                string key = ECacheKey.ModWorkFlow.ToString();
                var cachedData = _distributedCache.GetString(key);
                if (cachedData != null)
                {
                    var defTasks = JsonConvert.DeserializeObject<List<ModWorkflow>>(cachedData);
                    return defTasks;
                }
                else
                {
                    var defTasks = await _moduleService.GetAllModWorkFolow();
                    RedisUtils.SetCacheData(_distributedCache, _Configuration, defTasks, key);
                    return defTasks;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
