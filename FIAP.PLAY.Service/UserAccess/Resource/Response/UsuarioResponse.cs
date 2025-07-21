using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.UserAccess.Resource.Response
{
    public class UsuarioResponse : ResourceBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
