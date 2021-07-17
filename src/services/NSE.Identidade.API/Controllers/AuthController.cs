using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using NSE.Identidade.API.Extensions;
using NSE.Identidade.API.Models;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

//using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace NSE.Identidade.API.Controllers {
    [Route("api/[controller]")]
    public class AuthController : MainController {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        public AuthController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                IOptions<AppSettings> appSettings) {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Registrar(UsuarioRegistroViewModel usuarioRegistroVM) {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var identityUser = new IdentityUser {
                UserName = usuarioRegistroVM.Email,
                Email = usuarioRegistroVM.Email,
                EmailConfirmed = true
            };

            var userCreated = await _userManager.CreateAsync(identityUser, usuarioRegistroVM.Senha);
            if (userCreated.Succeeded) {
                //Não é necessário logar o usuario após o seu registro
                //await _signInManager.SignInAsync(identityUser, false);
                var jwt = await GerarJwt(identityUser.Email);
                return CustomResponse(jwt);
            }

            userCreated.Errors.ToList().ForEach(error => AdicionarErroProcessamento(error.Description));

            return CustomResponse();
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> entrar(UsuarioLoginViewModel usuario) {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha, false, true);
            var jwt = await GerarJwt(usuario.Email);
            if (result.Succeeded) return CustomResponse(jwt);

            if (result.IsLockedOut) {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas!");
                return CustomResponse();
            }
            AdicionarErroProcessamento("Usuário ou senha inválido");

            return CustomResponse();
        }

        private async Task<UsuarioResposta> GerarJwt(string email) {
            #region Obter as Claims do Usuário
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in roles) {
                claims.Add(new Claim("role", userRole));
            }

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claims);
            #endregion

            #region Gerar o Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var encondedKey = Encoding.ASCII.GetBytes(_appSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor() {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encondedKey), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            #endregion

            #region Gerar a classe UsuarioResposta para devolver
            var usuarioResposta = new UsuarioResposta {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim() { Type = c.Type, Value = c.Value })
                }
            };
            #endregion

            return usuarioResposta;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
