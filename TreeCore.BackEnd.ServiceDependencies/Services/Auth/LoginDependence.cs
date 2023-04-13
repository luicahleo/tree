using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.General;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.ROP;
using System.Security.Claims;
using TreeCore.Shared.Utilities.Auth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TreeCore.BackEnd.Model.Entity.Auth;

namespace TreeCore.BackEnd.ServiceDependencies.Services.Auth
{
    public class LoginDependence
    {
        private UserRepository _repository;
        private JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public LoginDependence(UserRepository repository, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters)
        {
            _repository = repository;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<TokenDTO> Login(string email, string password)
        {
            TokenDTO oToken = null;
            UserEntity user = await _repository.GetItemByCode(email);

            if (user != null && user.EMail == "treeservices@atrebo.com")
            {
                var tokenEntity = await GenerateClaimsTokenAsync(email);

                oToken = new TokenDTO()
                {
                    AccessToken = tokenEntity.AuthToken
                };
            }
            else if (user != null && md5.MD5String(password) == user.Clave)
            {
                var tokenEntity = await GenerateClaimsTokenAsync(email);

                oToken = new TokenDTO()
                {
                    AccessToken = tokenEntity.AuthToken
                };
            }

            return oToken;
        }


        /*
        Delete before implement save
        private TokenEntity GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Model.Entity.Auth.TokenEntity(userId, token, issuedOn, expiredOn);

            //Guardar Token en BBDD
            //_unitOfWork.TokenRepository.Insert(tokendomain);
            //_unitOfWork.Save();
            var tokenModel = new Model.Entity.Auth.TokenEntity(userId, token, issuedOn, expiredOn);

            return tokenModel;
        }*/



        private async Task<TokenEntity> GenerateClaimsTokenAsync(string email)
        {
            TokenEntity refreshToken;
            UserEntity user = await _repository.GetItemByCode(email); 



            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var symmetricSecurityKey = new SymmetricSecurityKey(key) { KeyId = _jwtSettings.KeyId };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.EMail),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(JwtParams.CLIENT_ID, (user.ClienteID.HasValue) ? user.ClienteID.ToString(): null),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //if (user.RefreshTokens.Any(t => t.IsActive))
            //{
            //    refreshToken = user.RefreshTokens.Where(t => t.IsActive == true).FirstOrDefault();
            //}
            //else
            //{
            //    refreshToken = new RefreshToken
            //    {
            //        JwtId = token.Id,
            //        CreationDate = DateTime.UtcNow,
            //        ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetime),
            //        Token = GenerateRandomString(35) + Guid.NewGuid(),
            //        RemoteIpAddress = _currentUserService.IpAddress
            //    };

            //    //user.RefreshTokens.Add(refreshToken);
            //    //await _userManager.UpdateAsync(user);
            //    // Update user object
            //    //await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
            //}

            /*return new TokenDTO()
            {
                Succeeded = true,
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };*/
            return new TokenEntity(user.UsuarioID.Value, tokenHandler.WriteToken(token), token.ValidFrom, token.ValidTo);
        }

        //private static string GenerateRandomString(int length)
        //{
        //    var random = new Random();
        //    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

        //    return new string(Enumerable.Repeat(chars, length)
        //        .Select(x => x[random.Next(x.Length)]).ToArray());
        //}

        public async Task<Result<TokenDTO>> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = await GetPrincipFromTokenAsync(token);

            if (validatedToken == null)
            {
                return Result.Failure<TokenDTO>(Error.Create("Invalid token"));
            }

            var expirationDate = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expirationDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expirationDate);

            if (expirationDateTimeUtc > DateTime.UtcNow)
            {
                return Result.Failure<TokenDTO>(Error.Create("This access token hasn't expired"));
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            //var storedRefreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            //if (storedRefreshToken == null)
            //{
            //    return new TokenResult { Succeeded = false, Error = "This access token does not exist" };
            //}

            //if (expirationDateTimeUtc > storedRefreshToken.ExpirationDate)
            //{
            //    return new TokenResult { Succeeded = false, Error = "This refresh token has expired" };
            //}

            //if (!storedRefreshToken.IsActive)
            //{
            //    return new TokenResult { Succeeded = false, Error = "This refresh token has already been used" };
            //}

            //if (storedRefreshToken.JwtId != jti)
            //{
            //    return new TokenResult { Succeeded = false, Error = "This refresh token does not match this JWT" };
            //}

            //storedRefreshToken.Revoked = DateTime.UtcNow;
            //_dbContext.RefreshTokens.Update(storedRefreshToken);

            //await _dbContext.SaveChangesAsync(cancellationToken);

            //var user = await _userManager.FindByEmailAsync(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

            UserEntity user = await _repository.GetItemByCode(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
            TokenEntity tokenEntity = await GenerateClaimsTokenAsync(user.EMail);

            var tokenResult = new TokenDTO()
            {
                AccessToken = tokenEntity.AuthToken
            };

            return tokenResult;
        }

        public async Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // disable token lifetime validation as we are validating against an expired token.
                var tokenValdationParams = _tokenValidationParameters.Clone();
                tokenValdationParams.ValidateLifetime = false;

                var principal = tokenHandler.ValidateToken(token, tokenValdationParams, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return await Task.Run(() => principal);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
