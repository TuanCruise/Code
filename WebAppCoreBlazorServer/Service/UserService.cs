using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }
        public async Task<User> GetUserByUserNamePassword(string userName, string password)
        {
            var url = string.Format("User/GetUserLogin?userName={0}&password={1}", userName, password);
            var data = await LoadGetApi(url);
            var module = JsonConvert.DeserializeObject<RestOutput<User>>(data);
            return module.Data;
        }
    }
    public interface IUserService
    {
        Task<User> GetUserByUserNamePassword(string userName, string password);
    }
   
}
