using System;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions {
    public static class ClaimsPrincipalExtensions {
        public static string ObterUserId(this ClaimsPrincipal principal) {
            if(principal.Equals(null)) throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst("sub");
            return claim?.Value;
        }

        public static string ObterUserEmail(this ClaimsPrincipal principal) {
            if(principal.Equals(null)) throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst("email");
            return claim?.Value;
        }

        public static string ObterUserToken(this ClaimsPrincipal principal) {
            if(principal.Equals(null)) throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}
