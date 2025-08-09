using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;

namespace FIAP.PLAY.Application.UserAccess.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserResponse>>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetUserByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> CreateUserAsync(UserRequest request, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> UpdateUserAsync(long id, UserRequest request, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(long id, CancellationToken cancellationToken = default);
    }
}
