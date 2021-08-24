using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers {
    public class IdentidadeController : MainController {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService) {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("nova-conta")]
        public IActionResult Registro() {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro) {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var responseRegister = await _autenticacaoService.Registrar(usuarioRegistro);
            if(ResponsePossuiErros(responseRegister.ResponseResult)) return View(usuarioRegistro);

            //Realizar login na app web
            await RealizarLogin(responseRegister);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("login")]
        public IActionResult Login() {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin) {
            if (!ModelState.IsValid) return View(usuarioLogin);

            //Chama o método de login
            var responseLogin = await _autenticacaoService.Login(usuarioLogin);
            if(ResponsePossuiErros(responseLogin.ResponseResult)) return View(usuarioLogin);

            //Realizar login na app web
            await RealizarLogin(responseLogin);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout() {
            //Limpar o cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin usuarioResposta) {
            var token = ObterTokenFormatado(usuarioResposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", usuarioResposta.AccessToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new  ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken) {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
