using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NSE.WebApp.MVC.Configuration
{
    public static class IdentityConfig {
        public static void AddIdentityConfig(this IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt => {
                    opt.LoginPath = "/login";
                    opt.AccessDeniedPath = "/erro/403";
                });
        }

        public static void UseIdentityConfig(this IApplicationBuilder app) {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
