using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Application.Library.Services
{
    public class GameLibraryService(
        IUnityOfWork uow,
        IValidator<GameLibraryRequest> validator,
        ILoggerManager<GameLibraryRequest> loggerManager) : IGameLibraryService
    {
        public async Task<Result<GameLibraryResponse>> AddGameToLibraryAsync(long libraryId, long gameId, CancellationToken cancellationToken)
        {

            var library = await uow.Libraries.GetByIdAsync(libraryId);
            if (library is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Biblioteca não encontrada.");

            var game = await uow.Games.GetByIdAsync(gameId);
            if (game is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Jogo não encontrado.");

            var gameRequest = new GameRequest(game.Title, game.Price, game.Genre, game.YearLaunch, game.Developer);
            var libraryRequest = new LibraryRequest(library.UserId);

            var request = new GameLibraryRequest(libraryRequest, gameRequest);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new Domain.Shared.Exceptions.ValidationException([.. validationResult.Errors]);

            var gameLibrary = GameLibrary.Create(library, game);

            await uow.GameLibraries.CreateAsync(gameLibrary);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Jogo {game.Title} adicionado à biblioteca {library.Id} com sucesso.");
            return new Result<GameLibraryResponse>(Parse(gameLibrary));
        }

        public async Task RemoveGameLibraryAsync(long gameLibraryId, CancellationToken cancellationToken)
        {
            var gameLibrary = await uow.GameLibraries.GetByIdAsync(gameLibraryId);
            if (gameLibrary is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Registro de jogo na biblioteca não encontrado.");
            
            await uow.GameLibraries.DeleteAsync(gameLibrary.Id);
            await uow.CompleteAsync();
            loggerManager.LogInformation($"Registro de jogo com ID {gameLibraryId} removido com sucesso.");
        }

        public async Task<Result<IEnumerable<GameLibraryResponse>>> GetGamesByLibraryIdAsync(long libraryId, CancellationToken cancellationToken)
        {
            var library = await uow.Libraries.GetByIdAsync(libraryId);
            if (library is null)
                throw new Domain.Shared.Exceptions.NotFoundException("Biblioteca não encontrada.");

            var gameLibraries = await uow.GameLibraries.GetDbSet()
                .Include(gl => gl.Game)
                .Where(gl => gl.LibraryId == libraryId)
                .ToListAsync();

            var response = gameLibraries.Select(Parse);
            return new Result<IEnumerable<GameLibraryResponse>>(response);
        }

        public async Task<Result<GameLibraryResponse>> GetGameInLibraryAsync(long libraryId, long gameId, CancellationToken cancellationToken)
        {
            var gameLibrary = await uow.GameLibraries.GetDbSet()
                .Include(gl => gl.Game)
                .FirstOrDefaultAsync(gl => gl.LibraryId == libraryId && gl.GameId == gameId);

            if (gameLibrary is null)
                throw new Domain.Shared.Exceptions.NotFoundException("O jogo não foi encontrado na biblioteca.");
            return new Result<GameLibraryResponse>(Parse(gameLibrary));
        }   

        public static GameLibraryResponse Parse(GameLibrary gameLibrary)
        => new GameLibraryResponse(
            gameLibrary.Id,
            gameLibrary.LibraryId,
            new GameResponse(
                gameLibrary.Game.Id,
                gameLibrary.Game.Title,
                gameLibrary.Game.Price,
                gameLibrary.Game.Genre,
                gameLibrary.Game.YearLaunch,
                gameLibrary.Game.Developer
            ),
            gameLibrary.PurchaseDate,
            gameLibrary.Price
        );

        public static GameLibrary Parse(GameLibraryRequest request)
        => GameLibrary.Create(
            Domain.Library.Entities.Library.Create(request.Library.UserId),
            Game.Create(request.Game.Title, request.Game.Price, request.Game.Genre, request.Game.YearLaunch, request.Game.Developer)
        );
    }
}
