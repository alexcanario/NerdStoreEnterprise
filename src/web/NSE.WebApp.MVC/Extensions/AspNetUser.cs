using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions {
    public class AspNetUser : IUser {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUser(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public string Nome { get => _contextAccessor.HttpContext.User.Identity.Name; }

        public bool EstaAutenticado() {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Claim> ObterClaim() {
            return _contextAccessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext() {
            return _contextAccessor.HttpContext;
        }

        public string ObterUserEmail() {
            return _contextAccessor.HttpContext.User.ObterUserEmail();
        }

        public Guid ObterUserId() {
            return EstaAutenticado() ? Guid.Parse(_contextAccessor.HttpContext.User.ObterUserId()) : Guid.Empty;
        }

        public string ObterUserToken() {
            return _contextAccessor.HttpContext.User.ObterUserToken();
        }

        public bool PossuiRole(string role) {
            return _contextAccessor.HttpContext.User.IsInRole(role);
        }
    }
}
