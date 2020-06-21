using Blazored.Modal;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAppCoreBlazorServer.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using WebAppCoreBlazorServer.Data;

namespace WebAppCoreBlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddServerSideBlazor(o => o.DetailedErrors = true);
            services.AddBlazoredModal();
            services.AddAuthentication(
               CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();
            //services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IModuleService, ModuleService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<ILanguageService, LanguageService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddScoped<ISessionStorageService, SessionStorageService>();
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
            services.AddSingleton<HttpClient>();

            //services.AddTransient<IValidator<Person>, PersonValidator>();
            //services.AddTransient<IValidator<ModuleFieldInfo>,Common.FluentValidation>();
            services.AddBlazoredSessionStorage();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddDistributedRedisCache(options => // config redis cache server
            {
                options.Configuration = Configuration["ConfigApp:RedisConnection"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
