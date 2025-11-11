using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Products.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [HttpGet] 
        public IActionResult HandleError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: exception?.Error.Message,
                title: "An unexpected error occurred!"
            );
        }
    }
}
