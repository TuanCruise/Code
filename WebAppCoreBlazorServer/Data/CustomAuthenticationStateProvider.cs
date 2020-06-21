
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCoreBlazorServer.Services;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Blazored.SessionStorage;

namespace WebAppCoreBlazorServer.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }    
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var Email = await _sessionStorageService.GetItemAsync<string>("Email");
            ClaimsIdentity indentity;
            if (Email!=null)
            {
                 indentity = new ClaimsIdentity(new[]
            {
                 new Claim(ClaimTypes.Name,Email),
             }, "apiauth_type");
            }
            else
            {
                indentity = new ClaimsIdentity();
            }    

             var user = new ClaimsPrincipal(indentity);
            return await Task.FromResult(new AuthenticationState(user));
        }
        public void MarkUserAsAuthenticated (string Email)
        {
            var indentity = new ClaimsIdentity(new[]
           {
                new Claim(ClaimTypes.Name,Email),
            }, "apiauth_type");

            var user = new ClaimsPrincipal(indentity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        }
       public void MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("Email");
            var indentity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(indentity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }


}