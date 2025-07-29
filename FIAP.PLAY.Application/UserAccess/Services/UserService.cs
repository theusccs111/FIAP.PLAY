using AutoMapper;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces.Services;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FIAP.PLAY.Application.UserAccess.Services
{
    public class UserService : Service<Usuario, UsuarioRequest, UsuarioResponse> , IUserService
    {
        private readonly ILoggerManager<UserService> _logger;

        public UserService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Usuario> validator, ILoggerManager<UserService> logger) : base(httpContextAccessor, mapper, uow, config, validator)
        {
            _logger = logger;
        }

        public Resultado<LoginResponse> Login(AutenticarRequest autenticarRequest)
        {
            _logger.LogInformation("UserService.Autenticar - Iniciado");

            if (string.IsNullOrEmpty(autenticarRequest.Email) || string.IsNullOrEmpty(autenticarRequest.Senha))
            {
                _logger.LogError("UserService.Autenticar - O usuário e/ou a senha não podem ser vazios.");

                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "O usuário e/ou a senha não podem ser vazios.");
            }

            var usuario = Uow.Users.GetFirst(u => u.Email.Trim().ToLower().Equals(autenticarRequest.Email.Trim().ToLower()) &&
                                                u.Senha.Trim().ToLower().Equals(autenticarRequest.Senha.Trim().ToLower()));

            if (usuario is null || usuario.Id <= 0)
            {
                _logger.LogError("UserService.Autenticar - Dados de acesso incorretos.");
                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "Dados de acesso incorretos.");
            }

            
            var token = SalvarUserNoClaims(usuario);
            var loginResponse = new LoginResponse()
            {
                //    UsuarioId = usuario.Id,
                //    Nome = usuario.Nome,
                //    Email = usuario.Email,
                //    Perfil = usuario.Perfil,
                Token = token,
                //EstaAutenticado = true,
            };

            _logger.LogInformation(JsonSerializer.Serialize(loginResponse));
            return new Resultado<LoginResponse>(loginResponse);
        }

        private string SalvarUserNoClaims(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config["JwtSecurityToken:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UsuarioId", usuario.Id.ToString()),
                    new Claim("Nome", usuario.Nome),
                    new Claim("Email", string.IsNullOrEmpty(usuario.Email) ? "" : usuario.Email),
                    new Claim("Perfil", usuario.Perfil.ToString()),
                    new Claim("PerfilDescricao", usuario.Perfil.GetDescription()),
                    new Claim("EstaAutenticado", true.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenReturn = tokenHandler.WriteToken(token);
            var a = HttpContextAccessor;
            return tokenReturn;
        }

        public Resultado<LoginResponse> ObterUserLogado()
        {
            var a = HttpContextAccessor;
            return new Resultado<LoginResponse>(Usuario);
        }
    }
}
