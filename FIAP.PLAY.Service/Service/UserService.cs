using AutoMapper;
using FIAP.PLAY.Domain.Entities;
using FIAP.PLAY.Domain.Resource.Base;
using FIAP.PLAY.Domain.Resource.Request;
using FIAP.PLAY.Domain.Resource.Response;
using FIAP.PLAY.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAP.PLAY.Service.Service
{
    public class UserService : Service<Usuario,UsuarioRequest>
    {
        public UserService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Usuario> validator) : base(httpContextAccessor, mapper, uow, config, validator)
        {
        }

        public Resultado<LoginResponse> Autenticar(AutenticarRequest autenticarRequest)
        {
            if (string.IsNullOrEmpty(autenticarRequest.Email) || string.IsNullOrEmpty(autenticarRequest.Senha))
            {
                throw new Domain.Exceptions.ValidationException("Erro ao autenticar", "O usuário e/ou a senha não podem ser vazios.");
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
                    UserId = usuario.Id,
                };

                return new Resultado<LoginResponse>(loginResponse);
            }
            else
            {
                throw new Domain.Exceptions.ValidationException("Erro ao autenticar", "Dados de acesso incorretos.");
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
                    new Claim("UserId", usuario.Id.ToString()),
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
