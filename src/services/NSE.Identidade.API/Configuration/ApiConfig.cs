using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSE.WebApi.Core.Identidade;

namespace NSE.Identidade.API.Configuration {
    public static class ApiConfig {
        public static void AddApiConfig(this IServiceCollection services) {
            services.AddControllers();
        }

        public static void UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfig();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
