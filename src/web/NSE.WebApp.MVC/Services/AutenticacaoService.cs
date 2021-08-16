using NSE.WebApp.MVC.Models;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin) {
            var usuarioJson = JsonSerializer.Serialize(usuarioLogin);
            var loginContent = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/entrar", loginContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };
            
            if (!TratarErrosResponse(response)) {
                return new UsuarioRespostaLogin {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(responseContent, options)
                };
            }
           
            var responseString = JsonSerializer.Deserialize<UsuarioRespostaLogin>(responseContent, options);

            return responseString;
        }

        public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro) {
            var usuarioJson = JsonSerializer.Serialize(usuarioRegistro);
            var registroContent = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/registrar", registroContent);

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!TratarErrosResponse(response)) {
                return new UsuarioRespostaLogin {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(responseContent, options)
                };
            }
            
            var responseString = JsonSerializer.Deserialize<UsuarioRespostaLogin>(responseContent, options);
            return responseString;
        }
    }
}
