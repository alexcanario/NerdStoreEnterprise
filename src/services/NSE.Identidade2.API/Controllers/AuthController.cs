using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using NSE.Identidade.API.Models;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Extensions;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace NSE.Identidade.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager, 
                              IOptions<AppSettings> appSettings) {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }


        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            if (!result.Succeeded) return BadRequest();

            var token = await GerarJwt(usuarioLoginVM.Email);
            return Ok(token);
        }

        public async Task<UsuarioRespostaLogin> GerarJwt(string email) {
            #region Obtendo usuário, claims  e roles
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UnixEpoch.ToString(CultureInfo.InvariantCulture)));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UnixEpoch.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64));

            foreach (var role in roles) {
                claims.Add(new Claim("role", role));
            }

            var claimsIdentity = new ClaimsIdentity(claims);
            
            var claimsIdentity_ = new ClaimsIdentity();
            claimsIdentity_.AddClaims(claims);
            #endregion

            #region Gerando o token
            var secret = _appSettings.Secret;
            var encodedSecret = Encoding.ASCII.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = tokenHandler.CreateToken( new SecurityTokenDescriptor() {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedSecret), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            #endregion

            return new UsuarioRespostaLogin() {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken() {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim(){ Type = c.Type, Value = c.Value })
                }
            };
        }
    }
}
