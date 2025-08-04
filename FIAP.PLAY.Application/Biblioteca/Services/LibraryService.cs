using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Domain.Library.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Application.Biblioteca.Services
{
    public class LibraryService(       
        IUnityOfWork uow,
        IValidator<LibraryRequest> validator,
        IUserService userService,
        ILoggerManager<LibraryRequest> loggerManager) : ILibraryService
    {
        public async Task<Result<LibraryResponse>> CreateLibraryAsync(LibraryRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);
        
            var possuiLibrary = await IsLibraryExistsAsync(request.UserId);

            if (possuiLibrary)
                throw new Domain.Shared.Exceptions.ValidationException("userId", "Usuário já possui uma biblioteca.");

            var library = Parse(request);
            
            await uow.Libraries.CreateAsync(library);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Biblioteca criada com sucesso para o usuário {request.UserId}");

            return new Result<LibraryResponse>(Parse(library));

        }

        public async Task DeleteLibraryAsync(long libraryId)
        {
            var library = await uow.Libraries.GetByIdAsync(libraryId);

            if (library is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Biblioteca não encontrada.");

            await uow.Libraries.DeleteAsync(libraryId);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Biblioteca com ID {libraryId} deletada com sucesso.");

        }

        public async Task<Result<IEnumerable<LibraryResponse>>> GetLibrariesAsync()
        {
            var libraries = await uow.Libraries.GetDbSet().Include(l => l.Games).ToListAsync();           

            var librariesResponse = libraries.Select(l => Parse(l)).ToList();

            return new Result<IEnumerable<LibraryResponse>>(librariesResponse);
        }

        public async Task<Result<LibraryResponse>> GetLibraryByIdAsync(long libraryId)
        {
            if (libraryId <= 0)
                throw new Domain.Shared.Exceptions.ValidationException("libraryId", "O ID da biblioteca não pode ser nulo ou negativo.");
            
            var library = await uow.Libraries.GetDbSet().Include( l => l.Games)
                .FirstOrDefaultAsync(l => l.Id == libraryId);

            if (library is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Biblioteca não encontrada.");
            
            return new Result<LibraryResponse>(Parse(library));
        }

        public async Task<Result<LibraryResponse>> GetLibraryByUserIdAsync(long userId)
        {
            var userHasLib = await IsLibraryExistsAsync(userId);

            if(userHasLib)
            {
                var library = await uow.Libraries.GetDbSet().Include(l => l.Games)
                    .FirstOrDefaultAsync(l => l.UserId == userId);

                return new Result<LibraryResponse>(Parse(library!));
            }

            var result = await CreateLibraryAsync(new LibraryRequest(userId));

            if (!result.Success)
                throw new Domain.Shared.Exceptions.ValidationException("userId", "Não foi possível criar a biblioteca para o usuário.");
            
            return new Result<LibraryResponse>(result.Data!);            
        }
       
        public async Task<bool> IsLibraryExistsAsync(long userId)
        {
            var user = await userService.GetUserByIdAsync(userId);

            if (user is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Usuário inválido");

            var library = await uow.Libraries.GetFirstAsync(l => l.UserId == userId);

            if (library is null)
                return false;
            return true;
        }

        private static Library Parse(LibraryRequest request)
           => Library.Create(request.UserId);

        private static LibraryResponse Parse(Library entidade)
            => new(entidade.Id, entidade.UserId);
    }
}
