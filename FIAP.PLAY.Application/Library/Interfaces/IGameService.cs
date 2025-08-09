using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Interfaces
{
    public interface IGameService 
    {
        Task<Result<IEnumerable<GameResponse>>> GetGamesAsync();
        Task<Result<GameResponse>> GetGameByIdAsync(long id);
        Task<Result<GameResponse>> CreateGameAsync(GameRequest request);
        Task<Result<GameResponse>> UpdateGameAsync(long id, GameRequest request);
        Task DeleteGameAsync(long id);
    }
}
