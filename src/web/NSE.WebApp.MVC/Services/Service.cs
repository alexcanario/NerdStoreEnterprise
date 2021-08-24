using NSE.WebApp.MVC.Extensions;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services {
    public abstract class Service {
        public object JsonDeserealizerOptions { get; private set; }

        protected StringContent ObterConteudo(object dado) {
            var usuarioJson = JsonSerializer.Serialize(dado);
            return new StringContent(usuarioJson, Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserealizarObjetoResponse<T>(HttpResponseMessage response) {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response) {
            switch ((int)response.StatusCode) {
                case 401:  //HttpStatusCode.Unauthorized:   //401
                case 403:  //HttpStatusCode.Forbidden:      //403
                case 404:  //HttpStatusCode.NotFound:       //404
                case 500:  //HttpStatusCode.InternalServerError: //500
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400: // HttpStatusCode.BadRequest:     //400
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
