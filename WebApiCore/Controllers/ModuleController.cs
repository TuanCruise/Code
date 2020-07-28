using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApiCore.Common;
using WebApiCore.Controllers;
using WebApiCoreNew.Common.Filters;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using WebModelCore.Common;
using WebModelCore.ControlModel;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[IdentityBasicAuthentication]
    [CustomAuthentication]
    //public class ModuleController : ControllerBase {
    public class ModuleController : BaseController
    {

        private IConfiguration _configuration;
        private string _Schema = "";
        public ModuleController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        // GET api/<controller>
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult> Get(string moduleName)
        {
            var outPut = new RestOutput<ModuleInfoViewModel>();
            var moduleInfo = new ModuleInfoViewModel();
            try
            {
                var modulesInfo = new List<ModuleInfo>();
                var fieldsInfo = new List<ModuleFieldInfo>();
                var buttonsInfo = new List<ButtonInfo>();
                var buttonParamsInfo = new List<ButtonParamInfo>();
                var languageInfo = new List<LanguageInfo>();
                var oracleParamsInfo = new List<OracleParam>();
                var m_Client = new CoreController(_configuration);
                m_Client.ForceLoadModule(out modulesInfo, out fieldsInfo, out buttonsInfo, out buttonParamsInfo, out languageInfo, out oracleParamsInfo, moduleName);
                var fields = fieldsInfo.Where(x => String.IsNullOrEmpty(x.ParentId)).ToList();
                var parentIds = fieldsInfo.Where(x => !String.IsNullOrEmpty(x.ParentId)).Select(x => x.ParentId).Distinct().ToList();
                if (parentIds != null)
                {
                    foreach (var parentId in parentIds)
                    {
                        var field = fields.Where(x => x.FieldID == parentId);
                        if (field != null && field.Any())
                        {
                            field.First().FieldChilds = fieldsInfo.Where(x => x.ParentId == parentId).ToList();
                        }
                    }
                }
                //var dicFieldInfo = new Dictionary<ModuleFieldInfo, string>();
                moduleInfo = new ModuleInfoViewModel
                {
                    ButtonParamsInfo = buttonParamsInfo,
                    ButtonsInfo = buttonsInfo,
                    FieldsInfo = fields,
                    LanguageInfo = languageInfo,
                    ModulesInfo = modulesInfo,
                    OracleParamsInfo = oracleParamsInfo
                };
                outPut.Data = moduleInfo;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("LoadDefModByTypeValue")]
        public async Task<ActionResult> LoadDefModByTypeValue(string parameter)
        {
            var outPut = new RestOutput<List<CodeInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var outPutData = m_Client.LoadDefModByTypeValue(parameter);
                outPut.Data = outPutData;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
            return Ok(outPut);
        }
        [HttpPost]
        [Route("LoadDefModByTypeValue")]
        public async Task<ActionResult> LoadDefModByTypeValue([FromBody] List<CodeInfoParram> codeInfos)
        {
            var outPut = new RestOutput<List<CodeInfoModel>>();
            var outPutData = new List<CodeInfoModel>();
            try
            {
                var m_Client = new CoreController(_configuration);
                foreach (var item in codeInfos)
                {
                    try
                    {
                        if (item.ListSource == null)
                        {
                            continue;
                        }
                        if (item.ListSource.Contains(":"))
                        {
                            var codeInfoModel = new CodeInfoModel
                            {
                                Name = item.Name,
                                CodeInfos = m_Client.LoadDefModByTypeValue(item.ListSource)
                            };
                            outPutData.Add(codeInfoModel);
                        }
                        else
                        {
                            var parram = new List<string>();
                            if (!string.IsNullOrEmpty(item.Parrams))
                            {
                                parram.Add(item.Parrams);
                            }
                            var param = new List<SqlParameter>();
                            var storeName = item.ListSource;
                            if (!(item.ListSource.IndexOf("(") < 0 && item.ListSource.IndexOf(")") < 0))
                            {
                                storeName = item.ListSource.Substring(0, item.ListSource.IndexOf("("));
                                var strParr = item.ListSource.Substring(item.ListSource.IndexOf("(") + 1, item.ListSource.IndexOf(")") - item.ListSource.IndexOf("(") - 1);
                                if (!string.IsNullOrEmpty(strParr))
                                {
                                    parram.AddRange(strParr.Split(','));
                                }
                            }

                            param = m_Client.DiscoveryParameters(storeName);
                            int index = 0;
                            foreach (var pr in param)
                            {
                                //if (pr.NpgsqlDbType != NpgsqlTypes.NpgsqlDbType.Refcursor)
                                //{
                                pr.Value = parram[index];
                                index++;
                                //}
                            }
                            var data = new DataTable();

                            data = (DataTable)m_Client.RunStoreToDataTable(storeName, param);


                            if (data != null)
                            {
                                if (data.Columns.Contains("VALUE"))//Trường hợp load các Listsource không phải callback
                                {
                                    if (item.CtrlType != EControlType.SCL.ToString())
                                    {
                                        var codeInfoControls = (from p in data.AsEnumerable() select new CodeInfo { CodeValue = p["VALUE"].ToString(), CodeValueName = p["TEXT"].ToString() }).ToList();
                                        var codeInfoModel = new CodeInfoModel
                                        {
                                            ControlType = item.CtrlType,
                                            Name = item.Name,
                                            CodeInfos = codeInfoControls
                                        };
                                        outPutData.Add(codeInfoModel);
                                    }
                                    else
                                    {
                                        var scheduleControl = (from p in data.AsEnumerable() select new ScheduleControlModel { id = p["id"].ToString(), title = p["title"].ToString(), className = p["className"].ToString(), end = p["end"].ToString(), start = p["start"].ToString(), url = p["url"].ToString(), modId = p["modId"].ToString() }).ToList();
                                        var codeInfoModel = new CodeInfoModel
                                        {
                                            ControlType = item.CtrlType,
                                            Name = item.Name,
                                            ScheduleControls = scheduleControl
                                        };
                                        outPutData.Add(codeInfoModel);
                                    }
                                }
                                else//Trường hợp load các Listsource  callback
                                {
                                    var dataCallBack = JsonConvert.SerializeObject(data);
                                    var codeInfoModel = new CodeInfoModel
                                    {
                                        ControlType = item.CtrlType,
                                        Name = item.Name,
                                        DataCallBack = dataCallBack
                                    };
                                    outPutData.Add(codeInfoModel);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                outPut.Data = outPutData;
                outPut.ResultCode = 1;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
            return Ok(outPut);
        }


        [HttpPost]
        [Route("ExcuteStore2DataTable")]
        public async Task<string> ExcuteStore2DataTable(ParramModuleQuery query)
        {
            var outPut = new RestOutput<string>();
            try
            {

                var m_Client = new CoreController(_configuration);
                PostgresqlHelper postgresqlHelper = new PostgresqlHelper();
                var param = m_Client.DiscoveryParameters(query.Store);
                // var checkParram = param.Where(x => x.SqlDbType != SqlDbType.);
                if (param != null && param.Any())
                {
                    if (query.Parram.Length > 0)
                    {
                        param.First().Value = query.Parram.First();
                    }
                }
                var tb = (DataTable)m_Client.RunStoreToDataTable(query.Store, param); //m_Client.ExecuteStoreProcedurePostgreSQL(query.Store, query.Parram.Length>0? query.Parram.First().ToString():"");
                if (tb != null)
                {
                    outPut.Data = JsonConvert.SerializeObject(tb);
                    outPut.ResultCode = 1;
                    outPut.Message = "";
                    return JsonConvert.SerializeObject(outPut);
                }
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return JsonConvert.SerializeObject(outPut);
            }

            return JsonConvert.SerializeObject(outPut);
        }
        /// <summary>
        /// Chạy một store theo param truyền vào
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoadQueryModule")]
        public async Task<string> LoadQueryModule([FromBody] ParramModuleQuery query)
        {
            var outPut = new RestOutput<string>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var ds = m_Client.LoadProcedure(query.Store, query.Parram);
                if (ds != null)
                {
                    //Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                    //json.Converters.Add(new DataSetConverter());
                    //(((DataSet)ds).Tables[0])
                    outPut.Data = JsonConvert.SerializeObject(((DataSet)ds).Tables[0], Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                    //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
                    outPut.ResultCode = 1;
                    outPut.Message = "";
                    return JsonConvert.SerializeObject(outPut);
                }
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return JsonConvert.SerializeObject(outPut);
            }
            return null;
        }

        //[HttpPost]
        //[Route("LoadQueryModuleDynamic")]
        //public async Task<string> LoadQueryModuleDynamic([FromBody] ParramModuleQueryDynamicQuery logic)
        //{
        //    var outPut = new RestOutput<string>();
        //    try
        //    {
        //        var m_Client = new CoreController(_configuration);

        //        var ds = m_Client.LoadProcedureDynamicQuery(logic);
        //        //if (ds != null)
        //        //{
        //        //    //Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
        //        //    //json.Converters.Add(new DataSetConverter());
        //        //    //(((DataSet)ds).Tables[0])
        //        //    outPut.Data = JsonConvert.SerializeObject(((DataSet)ds).Tables[0], Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
        //        //    //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
        //        //    outPut.ResultCode = 1;
        //        //    outPut.Message = "";
        //        //    return JsonConvert.SerializeObject(outPut);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        outPut.ResultCode = -1;
        //        outPut.Message = ex.ToString();
        //        return JsonConvert.SerializeObject(outPut);
        //    }
        //    return null;
        //}

        [HttpGet]
        [Route("LoadExcuteModule")]
        public async Task<ActionResult> LoadExcuteModule(string modId)
        {
            var outPut = new RestOutput<List<ExecProcModuleInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadModExecProcByModId(modId);
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
        }
        /// <summary>
        /// Lấy thông tin dữ liệu cho trường hợp sửa và view
        /// </summary>
        /// <param name="modId"></param>
        /// <param name="subModId"></param>
        /// <param name="parram"></param>
        /// <param name="fieldsInfos">List field của ModParameter đc truyền từ app lên</param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoadMainTainModule")]
        public async Task<ActionResult> LoadMainTainModule(string modId, string subModId, string parram, [FromBody] List<ModuleFieldInfo> fieldsInfos)
        {
            var outPut = new RestOutput<object>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadMainTainModule(modId);
                MaintainModuleInfo modMaintain = null;
                var store = "";
                if (data.Any())
                {
                    modMaintain = data.First();
                    store = CommonFunction.GetStoreRunModMaintain(modMaintain, subModId ?? ESubMod.MVW.ToString());
                }
                var fields = CommonFunction.GetModuleFields(fieldsInfos, modId, FLDGROUP.PARAMETER);
                var cm = new WebApiCore.CommonFunction();
                var lst = new List<string>();
                if (!string.IsNullOrEmpty(store))
                {
                    var param = m_Client.DiscoveryParameters(store.IndexOf(".") > 0 ? store : store);
                    cm.GetNpgsqlParameterValues(param, fields);
                    var dataStore = m_Client.RunStoreToDataTable(store, param);
                    outPut.Data = JsonConvert.SerializeObject(dataStore);
                    return Ok(outPut);
                }
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }

        }

        [HttpPost]
        [Route("SaveEditModule")]
        public async Task<ActionResult> SaveEditModule(string modId, string store, string keyEdit, [FromBody] List<ModuleFieldInfo> fieldsInfos)
        {
            var outPut = new RestOutput<string>();
            try
            {

                var m_Client = new CoreController(_configuration);
                var cm = new WebApiCore.CommonFunction();
                var lst = new List<string>();
                if (!string.IsNullOrEmpty(store))
                {
                    var param = m_Client.DiscoveryParameters(store.IndexOf(".") > 0 ? store : _Schema + "." + store);
                    //cm.GetSQLServerParameterValues(out lst, store, param, fieldsInfos);
                    cm.GetNpgsqlParameterValues(param, fieldsInfos);
                    var userName = GetUserName();
                    var dataStore = m_Client.RunFunction(store, param);
                    outPut.ResultCode = 1;
                    outPut.Data = dataStore;
                    return Ok(outPut);
                }
                return Ok(null);
            }
            catch (Exception ex)
            {
                outPut.Data = ex.Message;
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return Ok(outPut);
            }
        }

        private string GetUserName()
        {
            var basic = HttpContext.Request.Headers["Authorization"];
            if (basic.Count > 0)
            {
                byte[] b = Convert.FromBase64String(basic.First().Replace("Basic", "").Trim());
                var strOriginal = System.Text.Encoding.UTF8.GetString(b);
                var split = strOriginal.Split(":");
                string userId = "";
                if (split.Length > 2)
                {
                    return split[2];
                }
            }
            return "";
        }
        [HttpPost]
        [Route("DeleteModule")]
        public async Task<ActionResult> DeleteModule(string store, [FromBody] List<ModuleFieldInfo> fieldsInfos)
        {
            var outPut = new RestOutput<string>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var cm = new WebApiCore.CommonFunction();
                //var lst = new List<string>();
                //var param = m_Client.DiscoveryParameters(store);
                ////cm.GetSQLServerParameterValues(out lst, store, param, fieldsInfos);
                //cm.GetNpgsqlParameterValues(param, fieldsInfos);
                //var userName = GetUserName();
                //var dataStore = m_Client.RunStore(store, lst, userName);


                var param = m_Client.DiscoveryParameters(store.IndexOf(".") > 0 ? store : _Schema + "." + store);
                cm.GetNpgsqlParameterValues(param, fieldsInfos);
                var userName = GetUserName();
                var dataStore = m_Client.RunFunction(store, param);

                outPut.Data = dataStore;
                outPut.ResultCode = 1;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("LoadModSearchByModId")]
        public async Task<ActionResult> LoadModSearchByModId(string modId)
        {
            var outPut = new RestOutput<List<SearchModuleInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadModSearchByModId(modId);
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
            }
            return Ok(null);
        }


        [HttpGet]
        [Route("LoadModExecProcByModId")]
        public async Task<ActionResult> LoadModExecProcByModId(string modId)
        {
            var outPut = new RestOutput<List<ExecProcModuleInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadModExecProcByModId(modId);
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }

        [HttpGet]
        [Route("LoadModMaintainByModId")]
        public async Task<ActionResult> LoadModMaintainByModId(string modId)
        {
            var outPut = new RestOutput<List<MaintainModuleInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadMainTainModule(modId);
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("GetAllError")]
        public async Task<ActionResult> GetAllError()
        {
            var outPut = new RestOutput<List<ErrorInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.LoadAllErrorInfo();
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("GetAllLanguageText")]
        public async Task<ActionResult> GetAllLanguageText()
        {
            var outPut = new RestOutput<List<LanguageInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.GetAllLanguageText();
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }

        [HttpGet]
        [Route("GetAllBtnLanguageText")]
        public async Task<ActionResult> GetAllBtnLanguageText()
        {
            var outPut = new RestOutput<List<LanguageInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.GetAllBtnLanguageText();
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("GetAllCodeInfo")]
        public async Task<ActionResult> GetAllCodeInfo()
        {
            var outPut = new RestOutput<List<CodeInfo>>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var data = m_Client.GetAllCodeInfo();
                outPut.Data = data;
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(outPut);
        }
        [HttpGet]
        [Route("GetGroupModByUserId")]
        public async Task<ActionResult> GetGroupModByUserId(string userId)
        {
            try
            {
                var outPut = new RestOutput<List<GroupMod>>();
                try
                {
                    var m_Client = new CoreController(_configuration);
                    var data = m_Client.GetGroupModByUserId(userId);
                    outPut.Data = data;
                    return Ok(outPut);
                }
                catch (Exception ex)
                {
                }
                return Ok(outPut);
            }
            catch (Exception ex)
            {
            }
            return Ok(null);
        }

        /// <summary>
        /// Chạy một store theo param truyền vào
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SearchModSearch")]
        public async Task<string> SearchModSearch([FromBody] SearchModSearch searchModSearch)
        {
            var outPut = new RestOutput<string>();
            try
            {
                var m_Client = new CoreController(_configuration);
                var whereExtension = string.Empty;
                var query = BuildQuerySearch.BuildSearchCondition(searchModSearch.ModInfo, ref whereExtension, null, searchModSearch.StaticConditionInstances.First());

                //var ds = m_Client.LoadProcedure(query.Store, query.Parram);
                //if (ds != null)
                //{
                //    //Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                //    //json.Converters.Add(new DataSetConverter());
                //    //(((DataSet)ds).Tables[0])
                //    outPut.Data = JsonConvert.SerializeObject(((DataSet)ds).Tables[0], Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                //    //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
                //    outPut.ResultCode = 1;
                //    outPut.Message = "";
                //    return JsonConvert.SerializeObject(outPut);
                //}
            }
            catch (Exception ex)
            {
                outPut.ResultCode = -1;
                outPut.Message = ex.ToString();
                return JsonConvert.SerializeObject(outPut);
            }
            return null;
        }
    }
}