using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using WebModelCore.Common;

namespace WebAppCoreBlazorServer.Service
{
    public class ModuleService : BaseService, IModuleService
    {
        public ModuleService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        public async Task<List<CodeInfo>> GetCombobox(string para)
        {
            var url = "Module/LoadDefModByTypeValue?parameter=" + para;
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<List<CodeInfo>>>(data);
            return module.Data;
        }

        public async Task<ModuleInfoViewModel> GetModule(string modeId)
        {
            var url = "Module/Get?moduleName=" + modeId;
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<ModuleInfoViewModel>>(data);
            return module.Data;
        }

        public async Task<List<SearchModuleInfo>> LoadModSearchByModId(string modId)
        {
            var url = "Module/LoadModSearchByModId?modId=" + modId;
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<List<SearchModuleInfo>>>(data);
            return module.Data;
        }
        public async Task<List<ExecProcModuleInfo>> LoadExcuteModule(string modId)
        {
            var url = "Module/LoadExcuteModule?modId=" + modId;
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<List<ExecProcModuleInfo>>>(data);
            return module.Data;
        }

        public async Task<List<dynamic>> LoadQueryModule(ParramModuleQuery parram)
        {
            try
            {
                //var url = "Module/LoadQueryModule";
                //var data = await PostApi(url, parram);
                var data = await getQuery(parram);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);
                return moduledData;
            }
            catch (Exception ex)
            {
                //Common.ErrorLog.WriteLog("LoadQueryModule", ex.ToString());
                return null;
            }
        }
        public async Task<DataTable> LoadQueryModuleDataTable(ParramModuleQuery parram)
        {
            try
            {
                var url = "Module/LoadQueryModule";
                var data = await PostApi(url, parram);
                //var data = await getQuery(parram);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                return dt;
            }
            catch (Exception ex)
            {
                //Common.ErrorLog.WriteLog("LoadQueryModule", ex.ToString());
                return null;
            }
        }
        public async Task<List<dynamic>> LoadQueryModule(ParramModuleQueryDynamicQuery logic)
        {
            try
            {
                var url = "Module/LoadQueryModuleDynamic";
                var data = await PostApi(url, logic);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);
                return moduledData;
            }
            catch (Exception ex)
            {
                //Common.ErrorLog.WriteLog("LoadQueryModule", ex.ToString());
                return null;
            }
        }
        public async Task<List<dynamic>> LoadDataMainTainModule(string modId, string subModId, string parram, List<ModuleFieldInfo> moduleFieldInfos)
        {
            try
            {
                var url = string.Format("Module/LoadMainTainModule?modId={0}&subModId={1}&parram={2}", modId, subModId, parram);
                var data = await PostApi(url, moduleFieldInfos);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);
                return moduledData;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<RestOutput<string>> SaveEditModule(string modId, string store, string keyEdit, List<ModuleFieldInfo> fieldEdits)
        {
            try
            {
                var url = string.Format("Module/SaveEditModule?modId={0}&store={1}&keyEdit={2}", modId, store, keyEdit);
                var data = await PostApi(url, fieldEdits);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                return moduleds;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<RestOutput<string>> DeleteModule(string store, List<ModuleFieldInfo> fieldDels)
        {
            try
            {
                var url = string.Format("Module/DeleteModule?store={0}", store);
                var data = await PostApi(url, fieldDels);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                return moduleds;
            }
            catch (Exception ex)
            {
                var outPut = new RestOutput<string>();
                outPut.Data = ex.Message;
                return outPut;
            }
        }
        public async Task<RestOutput<List<CodeInfoModel>>> GetCombobox(List<CodeInfoParram> list)
        {
            try
            {
                var url = string.Format("Module/LoadDefModByTypeValue");
                var data = await PostApi(url, list);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<CodeInfoModel>>>(data);
                return moduleds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<dynamic>> ExcuteStore2DataTable(ParramModuleQuery query)
        {
            try
            {
                var url = string.Format("Module/ExcuteStore2DataTable");
                var data = await PostApi(url, query);
                var dataQuery = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                if (!string.IsNullOrEmpty(dataQuery.Data))
                {
                    return JsonConvert.DeserializeObject<List<dynamic>>(dataQuery.Data);
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //

        public async Task<List<MaintainModuleInfo>> LoadMaintainModuleInfo(string modId)
        {
            try
            {
                var url = string.Format("Module/LoadModMaintainByModId?modId={0}", modId);
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<MaintainModuleInfo>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<List<ErrorInfo>> GetAllError()
        {
            try
            {
                var url = "Module/GetAllError";
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<ErrorInfo>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<List<LanguageInfo>> GetAllTextLanguage()
        {
            try
            {
                var url = "Module/GetAllLanguageText";
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<LanguageInfo>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<List<LanguageInfo>> GetAllBtnLanguageText()
        {
            try
            {
                var url = "Module/GetAllBtnLanguageText";
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<LanguageInfo>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<List<SysVar>> GetAllSysVar()
        {
            try
            {
                var url = "SysVar/GetAllSysVar";
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<SysVar>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<List<GroupMod>> GetGroupModByUserId(int userId)
        {
            try
            {
                var url = "Module/GetGroupModByUserId?userId=" + userId;
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<GroupMod>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<RestOutput<string>> ChangeModel(string barcode)
        {
            try
            {
                var url = "Module/ChangeModel";
                var data = await PostApi(url, barcode);
                var output = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                return output;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<List<CodeInfo>> GetAllDefCode()
        {
            try
            {
                var url = string.Format("Module/GetAllCodeInfo");
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<CodeInfo>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Lấy tất cả thông tin bảng deftask
        /// </summary>
        /// <returns></returns>
        public async Task<List<DefTasks>> GetAllDefTasks()
        {
            try
            {
                var url = string.Format("Module/GetAllDefTask");
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<DefTasks>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ModWorkflow>> GetAllModWorkFolow()
        {
            try
            {
                var url = string.Format("Module/GetAllModWorkflow");
                var data = await LoadGetApi(url);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<List<ModWorkflow>>>(data);
                return moduleds.Data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<dynamic>> SearchModSearch(SearchModSearch searchModSearch)
        {
            try
            {
                var url = string.Format("Module/SearchModSearch");
                var data = await PostApi(url, searchModSearch);
                var dataQuery = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                if (!string.IsNullOrEmpty(dataQuery.Data))
                {
                    return JsonConvert.DeserializeObject<List<dynamic>>(dataQuery.Data);
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
    public interface IModuleService
    {
        Task<ModuleInfoViewModel> GetModule(string modeId);
        Task<List<CodeInfo>> GetCombobox(string para);
        Task<List<SearchModuleInfo>> LoadModSearchByModId(string modId);
        Task<List<ExecProcModuleInfo>> LoadExcuteModule(string modId);
        Task<List<dynamic>> LoadQueryModule(ParramModuleQuery parram);
        Task<DataTable> LoadQueryModuleDataTable(ParramModuleQuery parram);
        Task<List<dynamic>> LoadQueryModule(ParramModuleQueryDynamicQuery logic);
        /// <summary>
        /// Lấy thông tin dữ liệu cần để đưa lên view cho phần Edit và View
        /// </summary>
        /// <param name="modId"></param>
        /// <param name="subModId"></param>
        /// <param name="parram"></param>
        /// <param name="moduleFieldInfos"></param>
        /// <returns></returns>
        Task<List<dynamic>> LoadDataMainTainModule(string modId, string subModId, string parram, List<ModuleFieldInfo> moduleFieldInfos);
        Task<RestOutput<string>> SaveEditModule(string modId, string store, string keyEdit, List<ModuleFieldInfo> fieldEdits);
        Task<RestOutput<string>> DeleteModule(string store, List<ModuleFieldInfo> fieldDels);
        Task<List<MaintainModuleInfo>> LoadMaintainModuleInfo(string modId);
        Task<RestOutput<List<CodeInfoModel>>> GetCombobox(List<CodeInfoParram> list);
        Task<List<ErrorInfo>> GetAllError();
        Task<List<LanguageInfo>> GetAllTextLanguage();
        Task<List<LanguageInfo>> GetAllBtnLanguageText();
        Task<List<SysVar>> GetAllSysVar();
        Task<List<GroupMod>> GetGroupModByUserId(int userId);
        Task<List<dynamic>> ExcuteStore2DataTable(ParramModuleQuery query);
        Task<RestOutput<string>> ChangeModel(string barcode);
        Task<List<CodeInfo>> GetAllDefCode();
        Task<List<DefTasks>> GetAllDefTasks();
        Task<List<ModWorkflow>> GetAllModWorkFolow();
        Task<List<dynamic>> SearchModSearch(SearchModSearch searchModSearch);
    }
}
