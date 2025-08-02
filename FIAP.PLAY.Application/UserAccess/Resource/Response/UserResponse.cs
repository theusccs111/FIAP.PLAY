using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Library.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Application.UserAccess.Resource.Response
{
    public sealed record UserResponse(long Id, string Name, string Email, ERole Role) : ResponseBase;

}
