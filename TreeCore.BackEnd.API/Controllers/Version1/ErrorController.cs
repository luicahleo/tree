using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TreeCore.BackEnd.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            var errorMessage = new ErrorMessage
            {
                Status = StatusCodes.Status404NotFound,
                Message = "The specified resource was not found.",
                Error = "404 Not Found",
                Timestamp = DateTime.UtcNow
            };

            return NotFound(errorMessage);
        }
    }
}
