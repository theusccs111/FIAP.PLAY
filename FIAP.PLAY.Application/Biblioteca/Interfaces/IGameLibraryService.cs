using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
{
    public interface IGameLibraryService
    {
        Task<Result<GameLibraryResponse>> AddGameToLibraryAsync(long libraryId, long gameId);
        Task RemoveGameFromLibraryAsync(long libraryId, long gameId);
        Task<Result<IEnumerable<GameLibraryResponse>>> GetGamesByLibraryIdAsync(long libraryId);
        Task<Result<GameLibraryResponse>> GetGameInLibraryAsync(long libraryId, long gameId);
    }
}
