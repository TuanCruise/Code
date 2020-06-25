using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using WB.SYSTEM;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service {
    public class BaseService {
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
            if (!string.IsNullOrEmpty(authUname)) {
                var userName = ""; //_session.GetString("UserName");
                var textBytes = Encoding.UTF8.GetBytes(authUname + ":" + authPass +":"+ userName);
                _authToken = Convert.ToBase64String(textBytes);
            }
        }

        public async Task<string> LoadGetApi(string url)
        {
            try {
                //using (var handler = new HttpClientHandler {
                //    ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                //})
                //using (var client = new HttpClient(handler)) {
                var textBytes = Encoding.UTF8.GetBytes(_Configuration["ConfigApp:UserNameApi"] + ":" + _Configuration["ConfigApp:PasswordApi"] + ":" +""/* _session.GetString("UserName")*/);
                _authToken = Convert.ToBase64String(textBytes);
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _authToken);
                    var uri = _remoteServiceBaseUrl + url;
                    var content = "";
                    try
                    {
                        var data = await client.GetAsync(uri);
                        content = await data.Content.ReadAsStringAsync();
                    }
                    catch  (Exception e1)
                    {

                        System.Diagnostics.Debug.WriteLine("hieu="+e1.ToString());
                    }
                    return content;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            return string.Empty;
        }

        //Dongpv:15/06/2020
        public async Task<string> LoadData(string userId)
        {
            try
            {

                _remoteServiceBaseUrl = $"http://localhost:65104";
                using (var client = new HttpClient())
                {
                    ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                    var order = new {
                        ObjectName = "SYSTEM_PROCS_SP_DEF_MENU_SEL_ALL",
                        MsgType = "2",
                        MsgAction = "0",
                        Body = Body
                    };

                    var json = JsonConvert.SerializeObject(order);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    var uri = _remoteServiceBaseUrl;
                    var content = "";
                    try
                    {
                        //var data = await client.GetAsync(uri);
                        //var data = await client.PostAsync(uri);
                        //content = await data.Content.ReadAsStringAsync();
                        var responseMessage = await client.PostAsync(_remoteServiceBaseUrl, data);
                        responseMessage.EnsureSuccessStatusCode();

                        var result = await responseMessage.Content.ReadAsStringAsync();

                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            content = result;
                        }

                    }
                    catch (Exception e1)
                    {

                        System.Diagnostics.Debug.WriteLine("hieu=" + e1.ToString());
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

        public async Task<string> PostApi(string url, object o)
        {
            try {
                var uri = _remoteServiceBaseUrl + url;
                using (var client = new HttpClient()) {
                    var textBytes = Encoding.UTF8.GetBytes(_Configuration["ConfigApp:UserNameApi"] + ":" + _Configuration["ConfigApp:PasswordApi"] + ":" +""/* _session.GetString("UserName")*/);
                    _authToken = Convert.ToBase64String(textBytes);
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                    //jsonSerializerSettings.NullValueHandling= NullValueHandling.Ignore;
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(o, jsonSerializerSettings), System.Text.Encoding.UTF8, "application/json");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", _authToken);
                    try
                    {
                        var data = await client.SendAsync(requestMessage);
                        var dataString = await data.Content.ReadAsStringAsync();
                        return dataString;
                    }
                    catch (Exception ew)
                    {

                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            return null;
        }

        //Dongpv: fix co
        public async Task<string> Process(string json)
        {
            try
            {
                //FIX
                _remoteServiceBaseUrl = $"http://localhost:65104";
                var uri = _remoteServiceBaseUrl;
                //var uri = _remoteServiceBaseUrl + url;
                using (var client = new HttpClient())
                {
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var content = "";
                    try
                    {
                        //var data = await client.GetAsync(uri);
                        //var data = await client.PostAsync(uri);
                        //content = await data.Content.ReadAsStringAsync();
                        var responseMessage = await client.PostAsync(_remoteServiceBaseUrl, data);
                        responseMessage.EnsureSuccessStatusCode();

                        var result = await responseMessage.Content.ReadAsStringAsync();

                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            content = result;
                        }

                    }
                    catch (Exception e1)
                    {

                        System.Diagnostics.Debug.WriteLine("hieu=" + e1.ToString());
                    }
                    return content;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        //Dongpv
        public async Task<RestOutput<string>> SaveData(string modId, string enity, string keyEdit, List<ModuleFieldInfo> fieldEdits)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist
                ArrayList Body = new ArrayList();                
                foreach (ModuleFieldInfo moduleFieldInfo in fieldEdits)
                {
                    if (moduleFieldInfo.Value != null)
                    {
                        Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                        enity = moduleFieldInfo.Entity;
                    }
                }                

                var order = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_ADD_ACTION,
                    Body = Body
                };

                var json = JsonConvert.SerializeObject(order);

                var data = await Process(json);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                return moduleds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<RestOutput<string>> DeleteData(string modId, string enity, string keyEdit, List<ModuleFieldInfo> fieldEdits)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(obj);    
                //var  json = JsonConvert.DeserializeObject<string>(apiResponse);
                //ArrayList Body = new ArrayList(); Body.Add("pv_UserId"); Body.Add(userId);
                //1. Convert obj to arraylist
                ArrayList Body = new ArrayList();
                foreach (ModuleFieldInfo moduleFieldInfo in fieldEdits)
                {
                    Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                }

                var order = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_DELETE_ACTION,
                    Body = Body
                };

                var json = JsonConvert.SerializeObject(order);

                var data = await Process(json);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
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
                ArrayList Body = new ArrayList();
                foreach (ModuleFieldInfo moduleFieldInfo in fieldEdits)
                {
                    Body.Add(moduleFieldInfo.FieldName); Body.Add(moduleFieldInfo.Value);
                }

                var order = new {
                    UserID = "0000",
                    IP = "00000",
                    BranchID = "000",
                    ModuleID = modId,
                    ObjectName = enity,
                    MsgType = Constants.MSG_MNT_TYPE,
                    MsgAction = Constants.MSG_ADD_ACTION,
                    Body = Body
                };

                var json = JsonConvert.SerializeObject(order);

                var data = await Process(json);
                var moduleds = JsonConvert.DeserializeObject<RestOutput<string>>(data);
                return moduleds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Dongpv

    }
}
