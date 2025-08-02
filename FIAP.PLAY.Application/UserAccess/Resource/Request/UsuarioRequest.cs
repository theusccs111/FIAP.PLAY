using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Application.UserAccess.Resource.Request
{
    public record UsuarioRequest : RequestBase
    {
        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }

    }
}
