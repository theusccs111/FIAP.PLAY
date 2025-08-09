using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;

namespace FIAP.PLAY.Application.UserAccess.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserResponse>>> GetUsersAsync();
        Task<Result<UserResponse>> GetUserByIdAsync(long id);
        Task<Result<UserResponse>> CreateUserAsync(UserRequest request);
        Task<Result<UserResponse>> UpdateUserAsync(long id, UserRequest request);
        Task DeleteUserAsync(long id);
    }
}
