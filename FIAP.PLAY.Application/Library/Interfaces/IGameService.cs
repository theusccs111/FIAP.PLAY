using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Interfaces
{
    public interface IGameService 
    {
        Task<Result<IEnumerable<GameResponse>>> GetGamesAsync(CancellationToken cancellationToken);
        Task<Result<GameResponse>> GetGameByIdAsync(long id, CancellationToken cancellationToken);
        Task<Result<GameResponse>> CreateGameAsync(GameRequest request, CancellationToken cancellationToken);
        Task<Result<GameResponse>> UpdateGameAsync(long id, GameRequest request, CancellationToken cancellationToken);
        Task DeleteGameAsync(long id, CancellationToken cancellationToken);
    }
}
