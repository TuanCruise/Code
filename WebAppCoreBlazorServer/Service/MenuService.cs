using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public async Task<List<MenuItemInfo>> GetAllMenu(int userId)
        {
            var url = string.Format("Menu/GetAllMenu?userId=" + userId);
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<List<MenuItemInfo>>>(data);
            return module.Data;
        }
    }
    public interface IMenuService
    {
        Task<List<MenuItemInfo>> GetAllMenu(int userId);
    }
}
