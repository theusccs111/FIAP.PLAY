using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Library.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Application.UserAccess.Resource.Request
{
    public sealed record UserRequest(string Name, string PasswordHash, string Email, ERole Role, bool Active) : RequestBase;
}
