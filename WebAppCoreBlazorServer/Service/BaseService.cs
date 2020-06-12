using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

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
    }
}
