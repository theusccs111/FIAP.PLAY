using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Interfaces
{
    public interface ILibraryService
    {
        public Task<Result<LibraryResponse>> CreateLibraryAsync(LibraryRequest request, CancellationToken cancellationToken);
        public Task<Result<LibraryResponse>> GetLibraryByIdAsync(long libraryId, CancellationToken cancellationToken);
        public Task<Result<LibraryResponse>> GetLibraryByUserIdAsync(long userId, CancellationToken cancellationToken);
        public Task<Result<IEnumerable<LibraryResponse>>> GetLibrariesAsync(CancellationToken cancellationToken);
        public Task DeleteLibraryAsync(long libraryId, CancellationToken cancellationToken);     
        public Task <bool> IsLibraryExistsAsync(long userId);
    }
}
