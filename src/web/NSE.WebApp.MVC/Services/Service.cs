using System;
using System.Net;
using System.Net.Http;

namespace NSE.WebApp.MVC.Services {
    public abstract class Service {
        protected bool TratarErrosResponse(HttpResponseMessage response) {
            switch (response.StatusCode) {
                case HttpStatusCode.Unauthorized:   //401
                case HttpStatusCode.Forbidden:      //403
                case HttpStatusCode.NotFound:       //404
                case HttpStatusCode.InternalServerError: //500
                    throw new Exception();

                case HttpStatusCode.BadRequest:     //400
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
