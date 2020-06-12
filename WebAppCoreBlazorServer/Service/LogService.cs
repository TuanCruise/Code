using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service
{
    public class LogService : BaseService, ILogService
    {
        public LogService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public async Task<RestOutput<int>> WriteLog(string modId, string type, string action, string note, string ip)
        {
            var log = new LOG { ActionError = action, Ip = ip, ModId = modId, Note = note, Type = type };
            var url = string.Format("Log/InsertLog");
            var data = await PostApi(url, log);
            var module = JsonConvert.DeserializeObject<RestOutput<int>>(data);
            return module;
        }
      
    }
    public interface ILogService
    {
        Task<RestOutput<int>> WriteLog(string modId, string type, string action, string note, string ip);
    }
}
