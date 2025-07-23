using AutoMapper;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces.Services;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
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
    public class UserService : Service<Usuario,UsuarioRequest> , IUserService
    {
        private readonly ILoggerManager<UserService> _logger;

        public UserService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Usuario> validator, ILoggerManager<UserService> logger) : base(httpContextAccessor, mapper, uow, config, validator)
        {
            _logger = logger;
        }

        public Resultado<LoginResponse> Autenticar(AutenticarRequest autenticarRequest)
        {
            _logger.LogInformation("UserService.Autenticar - Iniciado");

            if (string.IsNullOrEmpty(autenticarRequest.Email) || string.IsNullOrEmpty(autenticarRequest.Senha))
            {
                _logger.LogError("UserService.Autenticar - O usuário e/ou a senha não podem ser vazios.");

                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "O usuário e/ou a senha não podem ser vazios.");
            }

            var usuario = Uow.Users.GetFirst(u => u.Email.Trim().ToLower().Equals(autenticarRequest.Email.Trim().ToLower()) &&
                                                u.Senha.Trim().ToLower().Equals(autenticarRequest.Senha.Trim().ToLower()));

            if (usuario.Id > 0)
            {
                var token = SalvarUserNoClaims(usuario);
                var loginResponse = new LoginResponse()
                {
                    EstaAutenticado = true,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Token = token,
                    UsuarioId = usuario.Id,
                };

                _logger.LogInformation(JsonSerializer.Serialize(loginResponse));
                return new Resultado<LoginResponse>(loginResponse);
            }
            else
            {
                _logger.LogError("UserService.Autenticar - Dados de acesso incorretos.");
                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "Dados de acesso incorretos.");
            }
        }

        private string SalvarUserNoClaims(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config["JwtSecurityToken:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, string.IsNullOrEmpty(usuario.Email) ? "" : usuario.Email),
                    new Claim("UsuarioId", usuario.Id.ToString()),
                    new Claim("Nome", usuario.Nome),
                    new Claim("Email", string.IsNullOrEmpty(usuario.Email) ? "" : usuario.Email),
                }),

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

        public override Resultado<UsuarioRequest> Add(UsuarioRequest request)
        {
            var result = base.Add(request);
            base.Complete();

            return result;
        }

        public override Resultado<UsuarioRequest[]> AddMany(UsuarioRequest[] request)
        {
            var result = base.AddMany(request);
            base.Complete();

            return result;
        }

        public override Resultado<UsuarioRequest> Update(UsuarioRequest request)
        {
            var result = base.Update(request);
            base.Complete();

            return result;
        }

        public override Resultado<UsuarioRequest[]> UpdateMany(UsuarioRequest[] request)
        {
            var result = base.UpdateMany(request);
            base.Complete();

            return result;
        }

        public override Resultado<UsuarioRequest> Delete(UsuarioRequest request)
        {
            var result = base.Delete(request);
            base.Complete();

            return result;
        }
        public override Resultado<UsuarioRequest[]> DeleteMany(UsuarioRequest[] request)
        {
            var result = base.DeleteMany(request);
            base.Complete();

            return result;
        }
    }
}
