using NSE.WebApp.MVC.Models;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<string> Login(UsuarioLogin usuarioLogin) {
            var usuarioJson = JsonSerializer.Serialize(usuarioLogin);
            var loginContent = new StringContent(usuarioJson, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/entrar", loginContent);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseString = JsonSerializer.Deserialize<string>(responseContent);

            return responseString;
        }

        public async Task<string> Registrar(UsuarioRegistro usuarioRegistro) {
            var usuarioJson = JsonSerializer.Serialize(usuarioRegistro);
            var registroContent = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/registrar", registroContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseString = JsonSerializer.Deserialize<string>(responseContent);

            return responseString;
        }
    }
}
