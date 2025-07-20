using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Domain.Shared.Resource.Request
{
    public class UsuarioRequest : ResourceBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
    }
}
