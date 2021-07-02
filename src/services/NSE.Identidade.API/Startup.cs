using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using NSE.Identidade.API.Data;

namespace NSE.Identidade.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //Mapeamento o context
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            //Suporte ao Identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Adiciona suporte ao WebApi
            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NSE.Identidade.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSE.Identidade.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            //Usa o suporte ao Autorization
            app.UseAuthorization();

            //Usa o suporte ao Autentication
            app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
