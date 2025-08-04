using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
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
