using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSE.Catalogo.API.Data;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.API.Config {
    public static class ApiConfig {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            
            services.AddControllers();

            services.AddCors(options => {
                options.AddPolicy("allow_all", policy => 
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("allow_all");

            app.UseAuthConfig();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
