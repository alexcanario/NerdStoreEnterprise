using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;

namespace NSE.Catalogo.API.Config {
    public static class SwaggerConfig {
        public static void AddSwaggerConfig(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "NerdStore Enterprise Catálogo API", 
                    Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                    Contact = new OpenApiContact { Name = "Alex Canario", Email = "contato@desenvolvedor.io" },
                    License = new OpenApiLicense {  Name = "MIT", Url = new Uri("https://opensource.org/license") },
                    Version = "v1" 
                });
            });
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSE.Catalogo.API v1"));
            }
        }
    }
}
