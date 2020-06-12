using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore;
using WebApiCore.BasicAuthenFilter.Filters;

namespace WebApiCore.Common.Filters {

    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute {
        protected  override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            //UserManager<IdentityUser> userManager = CreateUserManager();
            userName = userName.ToLower();
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.

            var uname = "hieupx3"; //WebConfigurationManager.AppSettings["UsernameAuthen"];
            var pw = "hieupx3";//WebConfigurationManager.AppSettings["PasswordAuthen"];
            var usernameConfig = "hieupx3"; //Utilities.EncryptDecrypt.Instance.Decrypt(uname);
            var passwordConfig = "hieupx3";//Utilities.EncryptDecrypt.Instance.Decrypt(pw);
            bool result = false;
            try {
                //xử lý kiểm tra thông tin đăng nhập
                if (userName.Trim() == usernameConfig.Trim() && password.Trim() == passwordConfig.Trim()) {
                    result = true;
                }
            }
            catch (Exception) {
                //return null để raise 401: Unauthorized
                return null;
            }
            if (!result) return null;

            // Create a ClaimsIdentity with all the claims for this user.
            var nameClaim = new Claim(ClaimTypes.Name, userName);
            var claims = new List<Claim> { nameClaim };

            // important to set the identity this way, otherwise IsAuthenticated will be false
            var identity = new ClaimsIdentity(claims, "Basic");

            //var principal = new ClaimsPrincipal(identity);
            return new ClaimsPrincipal(identity);
        }

    }
}