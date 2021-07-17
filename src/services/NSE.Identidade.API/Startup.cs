using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;

using System;
using System.Text;

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
            #region Dependency Injection for ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
            });
            #endregion
          
            #region Dependency Injection for Identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            #endregion

            #region Dependency Injection for JWT
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Segredo);

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => {
                bearerOptions.RequireHttpsMetadata = true; 
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                 };
            });

            
            
            #endregion

            services.AddControllers();

            #region Dependency Injection for Swagger
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "NSE.Identidade.API", 
                    Description = "Authentication API",
                    Version = "V1",
                    Contact = new OpenApiContact { Name = "Alex Canario", Email = "alexcanario@gmail.com" },
                    License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                #region Add Swagger for pipeline
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1"));
                #endregion
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            #region Use Authentication
            app.UseAuthentication();
            #endregion

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
