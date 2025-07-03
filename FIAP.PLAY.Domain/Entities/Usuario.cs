using FIAP.PLAY.Domain.Entities.Base;
using FIAP.PLAY.Domain.Enums;

namespace FIAP.PLAY.Domain.Entities
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
