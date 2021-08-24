using NSE.WebApp.MVC.Extensions;

using System;
using System.Net;
using System.Net.Http;

namespace NSE.WebApp.MVC.Services {
    public abstract class Service {
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
