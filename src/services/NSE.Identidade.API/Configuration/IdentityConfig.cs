using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebApi.Core.Identidade;

namespace NSE.Identidade.API.Configuration {
    public static class IdentityConfig {
        public static void AddIdentityConfig(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<ApplicationDbContext>(opt => {
                opt.UseSqlServer(configuration.GetConnectionString("DevConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            #region JWT
            services.AddJwtConfig(configuration);
            #endregion
        }
    }
}
