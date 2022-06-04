using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSE.Identidade.API.Configuration;

namespace NSE.Identidade.API {
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfig(Configuration);

            services.AddApiConfig();

            #region Dependency Injection for Swagger
            services.AddSwaggerConfig();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            #region Use Swagger for pipeline
            app.UseSwaggerConfig(env);
            #endregion

            app.UseApiConfig(env);
        }
    }
}
