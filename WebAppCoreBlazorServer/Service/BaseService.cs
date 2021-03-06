﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.Modal;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NuGet.Frameworks;
using WB.MESSAGE;
using WB.SYSTEM;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service
{
    public class BaseService
    {
        private string _remoteServiceBaseUrl = "";        
        private string _authToken = "";
        protected IConfiguration _Configuration;
        private IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public BaseService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _Configuration = configuration;
            _remoteServiceBaseUrl = _Configuration["ConfigApp:UrlApi"];
            var authUname = _Configuration["ConfigApp:UserNameApi"];
            var authPass = _Configuration["ConfigApp:PasswordApi"];
            if (!string.IsNullOrEmpty(authUname))
            {
                var userName = ""; //_session.GetString("UserName");
                var textBytes = Encoding.UTF8.GetBytes(authUname + ":" + authPass + ":" + userName);
                _authToken = Convert.ToBase64String(textBytes);
            }
        }

        public async Task<string> LoadGetApi(string url)
        {
            try
            {
                //using (var handler = new HttpClientHandler {
                //    ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                //})
                //using (var client = new HttpClient(handler)) {
                var textBytes = Encoding.UTF8.GetBytes(_Configuration["ConfigApp:UserNameApi"] + ":" + _Configuration["ConfigApp:PasswordApi"] + ":" + ""/* _session.GetString("UserName")*/);
                _authToken = Convert.ToBase64String(textBytes);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _authToken);
                    var uri = _remoteServiceBaseUrl + url;
                    var content = "";
                    try
                    {
                        var data = await client.GetAsync(uri);
                        content = await data.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Process(ex);
                        System.Diagnostics.Debug.WriteLine("hieu=" + ex.ToString());
                    }
                    return content;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return string.Empty;
        }
        public async Task<string> PostApi(string url, object obj)
        {
            try
            {
                //Dongpv:20/07/2020
                if (url == "Module/ExcuteStore2DataTable")
                {
                    ParramModuleQuery query = (ParramModuleQuery)obj;

                    Message msg = new Message();
                    msg.ObjectName = Constants.OBJ_SEARCH;
                    msg.MsgType = Constants.MSG_MISC_TYPE;
                    msg.MsgAction = Constants.MSG_SEARCH;

                    msg.Body.Add("SearchObject");
                    msg.Body.Add(query.Store);
                    msg.Body.Add("Condition");
                    msg.Body.Add(" WHERE 1=0");
                    msg.Body.Add("Page");
                    msg.Body.Add(0);
              
                    var json = JsonConvert.SerializeObject(msg);

                    var result = await SendMessage(json);                    

                    return result;
                }
                else
                {
                    var uri = _remoteServiceBaseUrl + url;
                    ErrorHandler.WirteTrace(uri);
                    using (var client = new HttpClient())
                    {
                        var textBytes = Encoding.UTF8.GetBytes(_Configuration["ConfigApp:UserNameApi"] + ":" + _Configuration["ConfigApp:PasswordApi"] + ":" + ""/* _session.GetString("UserName")*/);
                        _authToken = Convert.ToBase64String(textBytes);
                        //Dongpv
                        //var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                        var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, uri);
                        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                        //jsonSerializerSettings.NullValueHandling= NullValueHandling.Ignore;
                        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(obj, jsonSerializerSettings), System.Text.Encoding.UTF8, "application/json");
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", _authToken);
                        try
                        {
                            var data = await client.SendAsync(requestMessage);
                            var dataString = await data.Content.ReadAsStringAsync();
                            return dataString;
                        }
                        catch (Exception ex)
                        {

                            ErrorHandler.Process(ex);
                        }
                    }
                }
                //Dongpv:20/07/2020
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
            }
            return null;
        }

        //Dongpv:20/07/2020
        public async Task<DataTable> PostApi(object obj)
        {
            try
            {
                ParramModuleQuery query = (ParramModuleQuery)obj;

                Message msg = new Message();
                msg.ObjectName = Constants.OBJ_SEARCH;
                msg.MsgType = Constants.MSG_MISC_TYPE;
                msg.MsgAction = Constants.MSG_SEARCH;

                msg.Body.Add("SearchObject");
                msg.Body.Add(query.Store);
                msg.Body.Add("Condition");
                msg.Body.Add(" WHERE 1=0");
                msg.Body.Add("Page");
                msg.Body.Add(0);

                var json = JsonConvert.SerializeObject(msg);

                var result = await SendMessage(json);

                DataTable tb = new DataTable();
                if (!string.IsNullOrEmpty(result) && result != "null")
                    tb = SysUtils.Json2Table(result);

                return tb;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
            }

            return null;
        }

        //Dongpv:15/06/2020
        //public async Task<List<dynamic>> getQuery(ParramModuleQuery parram)
        public async Task<List<dynamic>> getQuery(Message msg) //ParramModuleQuery parram ModalParameters
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist
                //foreach (ParramModuleQuery moduleFieldInfo in  parram)
                //{
                //    if (moduleFieldInfo.Value != null)
                //    {
                //        Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                //        enity = moduleFieldInfo.Entity;
                //    }
                //}

                var json = JsonConvert.SerializeObject(msg);

                //2. CALL HOST
                var result = await SendMessage(json);
                DataTable tb = new DataTable();
                if (!string.IsNullOrEmpty(result) && result != "null")
                    tb = SysUtils.Json2Table(result);

                //DataTable tb = SysUtils.Json2Table(data);

                //var moduleds = JsonConvert.DeserializeObject<string> (data);
                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                //var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);
                //var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds);
                // return moduledData;       
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                var outPut = new RestOutput<string>();
                outPut.Data = JsonConvert.SerializeObject(tb, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
                outPut.ResultCode = 1;
                outPut.Message = "";
                //return JsonConvert.SerializeObject(outPut);

                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(JsonConvert.SerializeObject(outPut));
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);

                return moduledData;

                //return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<dynamic>> getDetail(string modId, string modSearchId, List<ModuleFieldInfo> fieldEdits) //ParramModuleQuery parram ModalParameters
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist

                var order = new {
                    UserID = "0000",
                    MsgIP = "00000",
                    BranchID = "000",
                    ModId = modId,
                    ObjectName = Constants.OBJ_DETAIL,
                    MsgType = Constants.MSG_MISC_TYPE,
                    MsgAction = Constants.MSG_SEARCH,
                    Body = fieldEdits
                };
                
                var json = JsonConvert.SerializeObject(order);

                //2. CALL HOST
                var result = await SendMessage(json);
                DataTable tb = new DataTable();
                if (!string.IsNullOrEmpty(result) && result != "null")
                    tb = SysUtils.Json2Table(result);

                //DataTable tb = SysUtils.Json2Table(data);
                //var moduleds = JsonConvert.DeserializeObject<string> (data);
                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                //var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);
                //var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds);
                // return moduledData;       
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(data, (typeof(DataTable)));
                var outPut = new RestOutput<string>();
                outPut.Data = JsonConvert.SerializeObject(tb, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
                outPut.ResultCode = 1;
                outPut.Message = "";
                //return JsonConvert.SerializeObject(outPut);

                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(JsonConvert.SerializeObject(outPut));
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);

                return moduledData;

                //return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<RestOutput<string>> SaveData(string modId, string enity, string keyEdit, List<ModuleFieldInfo> fieldEdits)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist
                //ArrayList Body = new ArrayList();
                //foreach (ModuleFieldInfo moduleFieldInfo in fieldEdits)
                //{
                //    if (moduleFieldInfo.Value != null)
                //    {
                //        if(string.IsNullOrEmpty(enity)) enity = moduleFieldInfo.Entity;

                //        string controlType = moduleFieldInfo.ControlType;
                //        string fileName = moduleFieldInfo.FieldName.Trim();
                //        object value = moduleFieldInfo.Value;

                //        if (controlType == "GE")
                //        {
                //            ArrayList arrDetail = new ArrayList();
                //            SysUtils.String2ArrayList(ref arrDetail, value.ToString().Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\r\n", "").Replace("\"", ""), ",", ":");
                //            value = arrDetail;
                //        }

                //       Body.Add(fileName); Body.Add(value);                        
                //    }
                //}

                var message = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ModId = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_ADD_ACTION,
                    Body = fieldEdits
                };

                var json = JsonConvert.SerializeObject(message);

                var result = await SendMessage(json);
                DataTable tb = new DataTable();
                if (!string.IsNullOrEmpty(result) && result != "null")
                    tb = SysUtils.Json2Table(result);

                var outPut = new RestOutput<string>();
                outPut.Data = JsonConvert.SerializeObject(tb, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                //string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new Newtonsoft.Json.Converters.DataSetConverter() } });
                outPut.ResultCode = 1;
                outPut.Message = "";

                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(JsonConvert.SerializeObject(outPut));
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);

                return moduleds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RestOutput<string>> DeleteData(string modId, string enity, string keyEdit, List<ButtonParamInfo> fieldEdits)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist

                //ArrayList Body = new ArrayList();
                //foreach (ButtonParamInfo moduleFieldInfo in fieldEdits)
                //{
                //    Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                //}

                var order = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_DELETE_ACTION,
                    Body = fieldEdits
                };

                var json = JsonConvert.SerializeObject(order);

                var data = await SendMessage(json);
                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                var outPut = new RestOutput<string>();
                outPut.Data = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });                
                outPut.ResultCode = 1;
                outPut.Message = "";
                
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(JsonConvert.SerializeObject(outPut));
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);

                return moduleds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RestOutput<string>> UpdateData(string modId, string enity, string keyEdit, List<ModuleFieldInfo> fieldEdits)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);

                //1. Convert obj to arraylist
                //ArrayList Body = new ArrayList();
                //foreach (ModuleFieldInfo moduleFieldInfo in fieldEdits)
                //{
                //    Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                //}

                var order = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_UPDATE_ACTION,
                    Body = fieldEdits
                };

                var json = JsonConvert.SerializeObject(order);

                var result = await SendMessage(json);
                DataTable tb = new DataTable();
                if (!string.IsNullOrEmpty(result) && result != "null")
                    tb = SysUtils.Json2Table(result);

                //var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                var outPut = new RestOutput<string>();
                outPut.Data = JsonConvert.SerializeObject(tb, Formatting.Indented, new JsonSerializerSettings { Converters = new[] { new DataSetConverter() } });
                outPut.ResultCode = 1;
                outPut.Message = "";

                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(JsonConvert.SerializeObject(outPut));
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(moduleds.Data, (typeof(DataTable)));
                var moduledData = JsonConvert.DeserializeObject<List<dynamic>>(moduleds.Data);

                return moduleds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> SendMessage(string json)
        {
            try
            {
                //Dongpv:Fix to debug
                //_remoteServiceBaseUrl = $"http://localhost:65104";
                var debug = _Configuration["ConfigApp:Debug"];
                var uri = _Configuration["ConfigApp:UrlCoreApi"];
                if (debug == "N")
                {
                    uri = _remoteServiceBaseUrl;
                }
                //Dongpv:Fix to debug

                //var uri = _remoteServiceBaseUrl + url;
                using (var client = new HttpClient())
                {
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    //DataTable content = null;
                    string content = null;

                    try
                    {
                        //var data = await client.GetAsync(uri);
                        //var data = await client.PostAsync(uri);
                        //content = await data.Content.ReadAsStringAsync();
                        var responseMessage = await client.PostAsync(uri, data);
                        //responseMessage.EnsureSuccessStatusCode();

                        var result = await responseMessage.Content.ReadAsStringAsync();

                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            content = result;
                            //if (result != "null")
                            //    content = SysUtils.Json2Table(result);
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Process(ex);
                        //System.Diagnostics.Debug.WriteLine(e1.ToString());
                    }

                    return content;

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                //Console.WriteLine(e);
            }
            return null;
        }


        //public async Task<DataTable> SendMessage(string json)
        //{
        //    try
        //    {
        //        //Dongpv:Fix to debug
        //        //_remoteServiceBaseUrl = $"http://localhost:65104";
        //        var debug = _Configuration["ConfigApp:Debug"];
        //        var uri = _Configuration["ConfigApp:UrlCoreApi"];
        //        if (debug == "N")
        //        {
        //            uri = _remoteServiceBaseUrl;
        //        }
        //        //Dongpv:Fix to debug

        //        //var uri = _remoteServiceBaseUrl + url;
        //        using (var client = new HttpClient())
        //        {
        //            var data = new StringContent(json, Encoding.UTF8, "application/json");
        //            DataTable content =  null;
        //            try
        //            {
        //                //var data = await client.GetAsync(uri);
        //                //var data = await client.PostAsync(uri);
        //                //content = await data.Content.ReadAsStringAsync();
        //                var responseMessage = await client.PostAsync(uri, data);
        //                //responseMessage.EnsureSuccessStatusCode();

        //                var result = await responseMessage.Content.ReadAsStringAsync();

        //                if (responseMessage.StatusCode == HttpStatusCode.OK)
        //                {
        //                    //content = result;
        //                    if (result != "null")
        //                        content = SysUtils.Json2Table(result);
        //                }

        //            }
        //            catch (Exception e1)
        //            {

        //                System.Diagnostics.Debug.WriteLine("hieu=" + e1.ToString());
        //            }

        //            return content;

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    return null;
        //}

    }
}
