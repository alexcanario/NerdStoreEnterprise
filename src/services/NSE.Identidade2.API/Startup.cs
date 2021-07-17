using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using NSE.Identidade.API.Data;

using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Extensions;

namespace NSE.Identidade.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            #region Mapeamento o context
            //services.AddDbContext<ApplicationDbContext>(options => {
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            //});
            #endregion

            #region Suporte ao Identity
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();
            #endregion

            #region Suporte ao Jason Web Token (JWT)

            //Obter os dados do AppSettings referente aos dados do JWT
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var keyJwt = Encoding.ASCII.GetBytes(appSettings.Secret);

            //services.AddAuthentication(opt => {
               opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions => {
               bearerOptions.RequireHttpsMetadata = true;
               bearerOptions.SaveToken = true;
               bearerOptions.TokenValidationParameters = new TokenValidationParameters() {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(keyJwt),
                   ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = appSettings.ValidoEm,
            //        ValidIssuer = appSettings.Emissor
               };
            });
            #endregion

            //Adiciona suporte ao WebApi
            services.AddControllers();

            #region Suporte ao Swagger
            //services.AddSwaggerGen(c => {
            //    c.SwaggerDoc("v1", new OpenApiInfo {
            //        Title = "NerdStore Enterprise Identity API", 
            //        Description = "Athentication Web Api",
            //        Contact = new OpenApiContact() { Name = "Alex Canario", Email = "alexcanario@gmail.com", Url = new Uri("https://www.linkedin.com/in/alex-canario/") },
            //        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            //        Version = "v1"
            //    });
            //});

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "NSE.Identidade.API", Version = "v1"});
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                #region Usa o Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSE.Identidade.API v1"));
                #endregion
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Usa o suporte ao Autorization
            app.UseAuthorization();

            //Usa o suporte ao Autentication
            //app.UseAuthentication();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
