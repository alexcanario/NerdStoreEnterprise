using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;

using System.Linq;

namespace NSE.WebApp.MVC.Controllers {
    public class MainController : Controller {
        protected bool ResponsePossuiErros(ResponseResult response) {
            if (response is not null && response.Errors.Mensagens.Any()) {
                //response.Errors.Mensagens.ForEach(mensagem => { ModelState.AddModelError("Response error: ", mensagem); });
                foreach(var msg in response.Errors.Mensagens) { 
                    ModelState.AddModelError(string.Empty, msg);
                }
                return true;
            }

            return false;
        }

    }
}
