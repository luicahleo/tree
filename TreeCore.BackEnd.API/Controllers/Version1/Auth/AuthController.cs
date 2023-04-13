using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreeCore.BackEnd.Service.Services.Auth;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.API.Controllers.Version1.Auth
{
    /// <summary>
    /// CompanyController
    /// </summary>
    [ApiController]
    //[Authorize]
    [Route(Routes.Base + "[controller]")]
    public class AuthController : ControllerBase
    {
        LoginService _loginService;

        public AuthController(LoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        ///  Authenticate user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     { 
        ///         "email":"test@email.com", 
        ///         "password":"Password"
        ///     }
        ///     
        /// </remarks>
        /// <param name="login"></param>
        /// <response code ="401">Unauthorized - not authenticated</response>
        /// <response code ="429">Too Many Requests</response>
        [HttpPost]
        [Route("login")]
        public async Task<ResultDto<TokenDTO>> Login(LoginDTO login)
        {
            return await _loginService.Login(login);
        }

        /// <summary>
        /// Refresh expired JWT tokens.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     {
        ///         "token":"eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9l",
        ///         "resfreshToken":"IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ"
        ///      }
        ///      
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<Result<TokenDTO>> RefreshToken(TokenDTO request)
        {
            return await _loginService.RefreshToken(request);
        }
    }
}
