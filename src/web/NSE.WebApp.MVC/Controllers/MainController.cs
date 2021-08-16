using Microsoft.AspNetCore.Mvc;

using NSE.WebApp.MVC.Models;

using System.Linq;

namespace NSE.WebApp.MVC.Controllers {
    public class MainController : Controller {
        protected bool ResponsePossuiErros(ResponseResult response) {
            return (response is not null && response.Errors.Mensagens.Any());
        }

    }
}
