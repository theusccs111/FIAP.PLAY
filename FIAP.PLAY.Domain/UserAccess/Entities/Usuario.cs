using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Domain.UserAccess.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }
        public bool Ativo { get; set; }
    }
}
