using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Interfaces
{
    public interface ILibraryService
    {
        public Task<Result<LibraryResponse>> CreateLibraryAsync(LibraryRequest request);
        public Task<Result<LibraryResponse>> GetLibraryByIdAsync(long libraryId);
        public Task<Result<LibraryResponse>> GetLibraryByUserIdAsync(long userId);
        public Task<Result<IEnumerable<LibraryResponse>>> GetLibrariesAsync();
        public Task DeleteLibraryAsync(long libraryId);     
        public Task <bool> IsLibraryExistsAsync(long userId);
    }
}
