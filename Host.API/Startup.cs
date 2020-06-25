using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IService, Service>();
            services.AddScoped<BaseConsumer>();

            services.AddMassTransit(x =>
            {
                // add the consumer to the container
                x.AddConsumer<BaseConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("localhost", "/", h => { });

                //cfg.UseExtensionsLogging(provider.GetService<ILoggerFactory>());

                cfg.ReceiveEndpoint(host, "web-service-endpoint", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.Consumer<BaseConsumer>(provider);                    
                    EndpointConvention.Map<Message>(e.InputAddress);
                });

            }));

            //services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            //services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<DoSomething>());
            services.AddSingleton<IHostedService, BusService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMvc();
            app.UseRouting();
            app.UseAuthorization();

        }
    }
}