using FIAP.PLAY.Domain.Entities.Base;

namespace FIAP.PLAY.Domain.Entities
{
    public class User : EntidadeBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
    }
}
