using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSE.WebApp.MVC.Configuration;

namespace NSE.WebApp.MVC {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnviroment) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnviroment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings{hostEnviroment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if(hostEnviroment.IsDevelopment()) {
                builder.AddUserSecrets<Startup>();      
            }

            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddIdentityConfig();
            services.AddWebAppConfig(Configuration);
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseIdentityConfig();
            app.UseWebAppConfig(env);
        }
    }
}
