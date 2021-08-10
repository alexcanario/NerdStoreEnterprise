using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions {
    public interface IUser {
        string Nome { get; }
        Guid ObterUserId();
        string ObterUserEmail();
        string ObterUserToken();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaim();
        HttpContext ObterHttpContext();
    }
}
