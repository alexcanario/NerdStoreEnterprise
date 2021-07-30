using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;

using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller {
        [HttpGet("nova-conta")]
        public IActionResult Registro() {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro) {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            //Chama o método Registro da Api

            if(false) return View(usuarioRegistro);

            //Login

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
            if(false) return View(usuarioLogin);

            //Login
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout() {
            //Limpar o cookie
            return RedirectToAction("Index", "Home");
        }
    }
}
