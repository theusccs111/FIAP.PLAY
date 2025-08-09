using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Interfaces
{
    public interface IGameLibraryService
    {
        Task<Result<GameLibraryResponse>> AddGameToLibraryAsync(long libraryId, long gameId, CancellationToken cancellationToken);
        Task RemoveGameFromLibraryAsync(long libraryId, long gameId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<GameLibraryResponse>>> GetGamesByLibraryIdAsync(long libraryId, CancellationToken cancellationToken);
        Task<Result<GameLibraryResponse>> GetGameInLibraryAsync(long libraryId, long gameId, CancellationToken cancellationToken);
    }
}
