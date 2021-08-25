using Microsoft.Extensions.Options;

using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services {
    public class AutenticacaoService : Service, IAutenticacaoService {
        private readonly HttpClient _httpClient;
        //private readonly AppSettings _settings;

        public AutenticacaoService(HttpClient httpClient, 
                                    IOptions<AppSettings> settings) {
            _httpClient = httpClient;
            //_settings = settings.Value; Não é mais necessario, pois estamo susando o httpClient.BaseAddress
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin) {
            var loginContent = ObterConteudo(usuarioLogin);

            //var response = await _httpClient.PostAsync($"{_settings.AutenticacaoUrl}/api/auth/entrar", loginContent);
            var response = await _httpClient.PostAsync("/api/auth/entrar", loginContent); //Utilizando o httpClient.BaseAddress

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

            //var response = await _httpClient.PostAsync($"{_settings.AutenticacaoUrl}/api/auth/registrar", registroContent); 
            var response = await _httpClient.PostAsync("/api/auth/registrar", registroContent); //Utilizando o httpClient.BaseAddress

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
