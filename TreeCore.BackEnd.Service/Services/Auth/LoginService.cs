using System.Threading.Tasks;
using TreeCore.BackEnd.ServiceDependencies.Services.Auth;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Auth
{
    public class LoginService
    {
        private readonly LoginDependence _LoginDependence;

        public LoginService(LoginDependence loginDependence)
        {
            _LoginDependence = loginDependence;
        }

        public async Task<ResultDto<TokenDTO>> Login(LoginDTO login)
        {
            var token = await _LoginDependence.Login(login.email, login.password);

            Result<TokenDTO> resultToken = (token != null && !string.IsNullOrEmpty(token.AccessToken)) ? 
                token : 
                Result.Failure<TokenDTO>(Error.Create("User or password not valid"));

            return (await resultToken.Async()).MapDto(x => x);
        }

        public async Task<Result<TokenDTO>> RefreshToken(TokenDTO tokenIn)
        {
            return await _LoginDependence.RefreshTokenAsync(tokenIn.AccessToken, tokenIn.RefreshToken);
        }

    }
}
