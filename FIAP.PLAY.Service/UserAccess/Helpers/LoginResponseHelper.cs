using FIAP.PLAY.Application.Shared.Helpers;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAP.PLAY.Application.UserAccess.Helpers
{
    public static class LoginResponseHelper
    {
        public static LoginResponse ObterLoginResponse(IHttpContextAccessor _httpContextAccessor)
        {
            LoginResponse loginResponse = new LoginResponse();
            if (_httpContextAccessor.HttpContext != null)
            {
                loginResponse.UserId = Convert.ToInt64(ClaimsHelper.ObterInformacaoDoClaims(_httpContextAccessor.HttpContext, "UserId"));
                loginResponse.Nome = ClaimsHelper.ObterInformacaoDoClaims(_httpContextAccessor.HttpContext, "Nome");
                loginResponse.Email = ClaimsHelper.ObterInformacaoDoClaims(_httpContextAccessor.HttpContext, "Email");

                loginResponse.EstaAutenticado = loginResponse.UserId != 0;
            }
            return loginResponse;
        }

        public static string AlterarIdentidade(LoginResponse loginResponse, IHttpContextAccessor _httpContextAccessor, IConfiguration Config)
        {
            var identity = _httpContextAccessor.HttpContext.User.Identities.First();

            identity.TryRemoveClaim(identity.FindFirst("UserId"));
            identity.AddClaim(new Claim("UserId", loginResponse.UserId.ToString()));

            identity.TryRemoveClaim(identity.FindFirst("Nome"));
            identity.AddClaim(new Claim("Nome", loginResponse.Nome));

            identity.TryRemoveClaim(identity.FindFirst("Email"));
            identity.AddClaim(new Claim("Email", loginResponse.Email));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config["JwtSecurityToken:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenReturn = tokenHandler.WriteToken(token);
            return tokenReturn;
        }
    }
}
