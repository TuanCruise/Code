using WebAppCoreBlazorServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCoreBlazorServer.Services
{
    public interface IUserService
    {
        public interface IUserService
        {
            public Task<User> LoginAsync(User user);
            public Task<User> RegisterUserAsync(User user);
            public Task<User> GetUserByAccessTokenAsync(string accessToken);
            
        }

        Task<User> GetUserByAccessTokenAsync(object accessToken);
    }
}
