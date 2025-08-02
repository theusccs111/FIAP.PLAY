using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.Shared.Extensions;
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
    public class UserService : Service, IUserService
    {
        private readonly IUnityOfWork _uow;
        private readonly IValidator<UsuarioRequest> _validator;
        private readonly ILoggerManager<UserService> _loggerManager;
        private readonly IConfiguration _config;
       

        public UserService(IHttpContextAccessor httpContextAccessor, IUnityOfWork uow, IValidator<UsuarioRequest> validator, ILoggerManager<UserService> loggerManager, IConfiguration config)
            : base(httpContextAccessor)
        {
            _uow = uow;
            _validator = validator;
            _loggerManager = loggerManager;
            _config = config;
           
        }

        public async Task<Resultado<LoginResponse>> LoginAsync(AutenticarRequest autenticarRequest)
        {
            _loggerManager.LogInformation("UserService.Autenticar - Iniciado");

            if (string.IsNullOrEmpty(autenticarRequest.Email) || string.IsNullOrEmpty(autenticarRequest.Senha))
            {
                _loggerManager.LogError("UserService.Autenticar - O usuário e/ou a senha não podem ser vazios.");

                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "O usuário e/ou a senha não podem ser vazios.");
            }

            var usuario = await _uow.Users.GetFirstAsync(u => u.Email.Trim().ToLower().Equals(autenticarRequest.Email.Trim().ToLower()) &&
                                                u.SenhaHash.Trim().ToLower().Equals(autenticarRequest.Senha.Trim().ToLower()));

            if (usuario is null || usuario.Id <= 0)
            {
                _loggerManager.LogError("UserService.Autenticar - Dados de acesso incorretos.");
                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "Dados de acesso incorretos.");
            }


            var token = GerarTokenJwt(usuario);
            var loginResponse = new LoginResponse()
            {
                //    UsuarioId = usuario.Id,
                //    Nome = usuario.Nome,
                //    Email = usuario.Email,
                //    Perfil = usuario.Perfil,
                Token = token,
                //EstaAutenticado = true,
            };

            _loggerManager.LogInformation(JsonSerializer.Serialize(loginResponse));
            return new Resultado<LoginResponse>(loginResponse);
        } 

        public Resultado<LoginResponse> ObterUserLogado()
        {
            if (Usuario == null)
            {
                throw new Domain.Shared.Exceptions.ValidationException("Usuario","Usuário não autenticado");
            }

            return new Resultado<LoginResponse>(Usuario);
        }

        public async Task<Resultado<LoginResponse>> AtualizarUsuario(long id, UsuarioRequest request)
        {
            if (id == 0)
            {
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");
            }

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new Domain.Shared.Exceptions.ValidationException(validationResult.Errors);
            }

            var usuario = await _uow.Users.GetByIdAsync(id);
            if (usuario == null)
            {
                throw new Domain.Shared.Exceptions.NotFoundException("Usuário não encontrado");
            }

            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.Perfil = request.Perfil;
            usuario.SenhaHash = request.SenhaHash;


            await _uow.Users.UpdateAsync(usuario);
            await _uow.CompleteAsync();

            var token = GerarTokenJwt(usuario);
            return new Resultado<LoginResponse>(new LoginResponse
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Token = token,
                EstaAutenticado = true
            });
        }

        private string SalvarUserNoClaims(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSecurityToken:key"]);
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

        public async Task DeletarUsuario(long id)
        {
            var usuario = await _uow.Users.GetByIdAsync(id);
            if (usuario == null)
            {
                throw new Domain.Shared.Exceptions.NotFoundException("Usuário não encontrado");
            }

            await _uow.Users.UpdateAsync(usuario);
            await _uow.CompleteAsync();
        }

        private string GerarTokenJwt(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSecurityToken:key"]);

            var claims = new List<Claim>
        {
            new Claim("UsuarioId", usuario.Id.ToString()),
            new Claim("Nome", usuario.Nome),
            new Claim("Email", usuario.Email),
            new Claim("Perfil", usuario.Perfil.ToString()),
            new Claim("PerfilDescricao", usuario.Perfil.GetDescription()),
            new Claim("EstaAutenticado", true.ToString()),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
