using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FIAP.PLAY.Application.UserAccess.Services
{
    public class AuthenticateService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<AuthenticateRequest> validator,
        ILoggerManager<AuthenticateService> loggerManager,
        IJWTService jwtService) : Service(httpContextAccessor), IAuthenticateService
    {

        public async Task<Result<LoginResponse>> LoginAsync(AuthenticateRequest request, CancellationToken cancellationToken = default)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            string hash = request.Password.Encrypt();

            var user = await uow.Users.GetFirstAsync(u =>
                u.Email.Address.ToString().Trim().ToLower() == request.Email.ToString().Trim().ToLower());

            if (user == null)
                throw new Domain.Shared.Exceptions.NotFoundException("Erro ao autenticar", "Usuário não encontrado");

            bool senhaValida = request.Password.Decrypt(user.PasswordHash);

            if (!senhaValida)
                throw new Domain.Shared.Exceptions.ValidationException("Erro ao autenticar", "Senha inválida");

            var token = jwtService.GenerateToken(user.Id.ToString(),user.Name,user.Email,user.Role.GetDescription());
            var loginResponse = new LoginResponse()
            {
                Token = token,
            };

            loggerManager.LogInformation(JsonSerializer.Serialize(loginResponse));
            return new Result<LoginResponse>(loginResponse);
        }
    }
}
