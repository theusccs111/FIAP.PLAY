using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.UserAccess.Services
{
    public class UserService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<UserRequest> validator,
        ILoggerManager<UserService> loggerManager) : Service(httpContextAccessor), IUserService
    {
        public async Task<Result<IEnumerable<UserResponse>>> GetUsersAsync()
        {
            var users = await uow.Users.GetAllAsync();
            var usersResponse = users.Select(d => Parse(d)).ToList();
            return new Result<IEnumerable<UserResponse>>(usersResponse);
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(long id)
        {
            var user = await uow.Users.GetByIdAsync(id);
            var userResponse = Parse(user);
            return new Result<UserResponse>(userResponse);
        }

        public async Task<Result<UserResponse>> CreateUserAsync(UserRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var user = Parse(request);

            var userCreated = await uow.Users.CreateAsync(user);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Usuário {userCreated.Name} criado com sucesso");
            return new Result<UserResponse>(Parse(userCreated));
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(long id, UserRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var user = Parse(request);
            user.Id = id;

            await uow.Users.UpdateAsync(user);
            uow.Complete();

            loggerManager.LogInformation($"Usuário com id {user.Id} atualizado com sucesso");
            return new Result<UserResponse>(Parse(user));

        }

        public async Task DeleteUserAsync(long id)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            if (await uow.Users.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "Usuário não encontrado");

            await uow.Users.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static User Parse(UserRequest request)
            => Domain.UserAccess.Entities.User.Criar(request.Name, request.PasswordHash, request.Email, request.Role, request.Active);

        private static UserResponse Parse(User entidade)
            => new(entidade.Id, entidade.Name, entidade.Email, entidade.Role);

    }
}
