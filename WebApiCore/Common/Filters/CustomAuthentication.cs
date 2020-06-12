using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApiCoreNew.Common.Filters {
    public class CustomAuthentication : TypeFilterAttribute {
        public CustomAuthentication(string claimType = "", string claimValue = "") : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //string actionName = (string)context.RouteData.Values["action"];
            //string controllerName = (string)context.RouteData.Values["controller"];
            //if (!(actionName.ToLower()=="user" && controllerName.ToLower() == "GetUserLogin".ToLower())) {

            //}
            var basic = context.HttpContext.Request.Headers["Authorization"];
            if (basic.Count > 0) {
                byte[] b = Convert.FromBase64String(basic.First().Replace("Basic", "").Trim());
                var strOriginal = System.Text.Encoding.UTF8.GetString(b);
                var split = strOriginal.Split(":");
                var user = split[0];
                var pass = split[1];
                var configUser = "hieupx3";
                var configPass = "hieupx3";
                string userId = "";
                if (split.Length > 2) {
                    userId = split[2];
                }
                if (!(user == configUser && configPass == pass)) {
                    context.Result = new ForbidResult();
                }
                else {
                }
            }

            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            //if (!hasClaim)
            //{
            //    context.Result = new ForbidResult();
            //}
        }
    }
}
