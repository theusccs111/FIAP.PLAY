using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Application.UserAccess.Resource.Request
{
    public record UserRequest : RequestBase
    {
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public ERole Role { get; set; }
        public bool Active { get; set; }

    }
}
