using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Application.UserAccess.Resource.Response
{
    public record UsuarioResponse : ResourceBase
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }
        public string PerfilDescricao { get { return Perfil.GetDescription(); }}
    }
}
