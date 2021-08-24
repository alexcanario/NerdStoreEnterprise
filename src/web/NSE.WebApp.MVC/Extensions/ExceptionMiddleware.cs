using Microsoft.AspNetCore.Http;

using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions {
    public class ExceptionMiddleware {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            } catch (CustomHttpRequestException e) {
                HandleRequestExceptionAsync(context, e);
            }
        }

        public void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException e) { 
            if(e.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                context.Response.Redirect("/login");
                return;
            }

            context.Response.StatusCode = (int)e.StatusCode;
        }
    }
}
