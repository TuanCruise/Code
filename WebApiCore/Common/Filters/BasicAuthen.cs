using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApiCore.Common.Filters
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthen
    {
        private readonly RequestDelegate _next;

        public BasicAuthen(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authenHeader = httpContext.Request.Headers["Authorization"];
            if (authenHeader!=null && authenHeader.StartsWith("Basic"))
            {
                string userAndPassHeader = authenHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                var userAndPass = encoding.GetString(Convert.FromBase64String(userAndPassHeader));
                int index = userAndPass.IndexOf(":");
                var userName = userAndPass.Substring(0,index);
                var pass = userAndPass.Substring(index+1);
                var userNameConfig = "";
                var passConfig = "";
                if (userName== userNameConfig && pass== passConfig) 
                {
                    await _next.Invoke(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenExtensions
    {
        public static IApplicationBuilder UseBasicAuthen(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthen>();
        }
    }
}
