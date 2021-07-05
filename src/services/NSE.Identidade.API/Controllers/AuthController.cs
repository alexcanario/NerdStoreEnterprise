using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using NSE.Identidade.API.Models;

using System.Threading.Tasks;

namespace NSE.Identidade.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(UsuarioRegistroViewModel usuarioRegistroVM) {
            if (!ModelState.IsValid) return BadRequest();

            var identityUser = new IdentityUser() {
                UserName = usuarioRegistroVM.Email,
                Email = usuarioRegistroVM.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, usuarioRegistroVM.Senha);
            if(result.Succeeded) {
                await _signInManager.SignInAsync(identityUser, false);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("logar")]
        public async Task<ActionResult> Logar(UsuarioLoginViewModel usuarioLoginVM) {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(usuarioLoginVM.Email, usuarioLoginVM.Senha, false, true);

            return Ok();
        }
    }
}
