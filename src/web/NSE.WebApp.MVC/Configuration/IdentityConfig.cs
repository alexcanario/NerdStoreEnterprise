using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Configuration {
    public static class IdentityConfig {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            return services;
        }

        public static IApplicationBuilder UseIdentityConfig(this IApplicationBuilder app) {
            app.UseAuthentication();
            app.UseAuthorization();

            return app.UseIdentityConfig();
        }
    }
}
