using FIAP.PLAY.Domain.Entities.Base;
using FIAP.PLAY.Domain.Resource.Base;

namespace FIAP.PLAY.Domain.Resource.Response
{
    public class UserResponse : ResourceBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
