using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
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
