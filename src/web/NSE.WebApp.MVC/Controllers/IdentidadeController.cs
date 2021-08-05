using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers {
    public class IdentidadeController : Controller {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService) {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("nova-conta")]
        public IActionResult Registrar() {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro) {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var responseRegister = await _autenticacaoService.Registrar(usuarioRegistro);
            if(responseRegister is null) return View(usuarioRegistro);

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
            if(responseLogin is null) return View(usuarioLogin);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sair")]
        public async Task<IActionResult>  Logout() {
            //Limpar o cookie
            return RedirectToAction("Index", "Home");
        }
    }
}
