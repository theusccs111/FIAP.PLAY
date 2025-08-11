using FIAP.PLAY.Domain.Shared.ValueObject;

namespace FIAP.PLAY.Application.UserAccess.Resource.Request
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
