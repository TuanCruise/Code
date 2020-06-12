using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service
{
    public class LanguageService : BaseService, ILanguageService
    {
        public LanguageService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public async Task<List<LanguageInfo>> GetAllIcon()
        {
            var url = string.Format("Language/GetAllIcon");
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<List<LanguageInfo>>>(data);
            return module.Data;
        }
    }
    public interface ILanguageService
    {
        Task<List<LanguageInfo>> GetAllIcon();
    }
}
