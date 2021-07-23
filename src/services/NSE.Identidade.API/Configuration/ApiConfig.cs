using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NSE.Identidade.API.Configuration {
    public static class ApiConfig {
        public static IServiceCollection AddApiConfig(this IServiceCollection services) {
            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //O Identity precisa estar entre o UseRouting e o UseEndpoint
            app.UseIdentityConfig();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
