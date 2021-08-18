using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;

using System.Linq;

namespace NSE.WebApp.MVC.Controllers {
    public class MainController : Controller {
        protected bool ResponsePossuiErros(ResponseResult response) {
            if (response is not null && response.Errors.Mensagens.Any()) {
                response.Errors.Mensagens.ForEach(Mensagem => { ModelState.AddModelError("Response Error: ", Mensagem); });
                return true;
            }

            return false;
        }

    }
}
