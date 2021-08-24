using NSE.WebApp.MVC.Models;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services {
    public class AutenticacaoService : Service, IAutenticacaoService {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin) {
            var loginContent = ObterConteudo(usuarioLogin);

            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/entrar", loginContent);

            if (!TratarErrosResponse(response)) {
                return new UsuarioRespostaLogin {
                    ResponseResult = await DeserealizarObjetoResponse<ResponseResult>(response)
                };
            }
           
            var responseString = await DeserealizarObjetoResponse<UsuarioRespostaLogin>(response);
            return responseString;
        }

        public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro) {
            
            var registroContent = ObterConteudo(usuarioRegistro);

            var response = await _httpClient.PostAsync("https://localhost:44304/api/auth/registrar", registroContent);

            if (!TratarErrosResponse(response)) {
                return new UsuarioRespostaLogin {
                    ResponseResult = await DeserealizarObjetoResponse<ResponseResult>(response)
                };
            }

            var responseString = await DeserealizarObjetoResponse<UsuarioRespostaLogin>(response);
            return responseString;
        }
    }
}
