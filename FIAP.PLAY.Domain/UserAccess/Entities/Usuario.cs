using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using System.Runtime.CompilerServices;

namespace FIAP.PLAY.Domain.UserAccess.Entities
{
    public class Usuario : EntidadeBase
    {
        #region Propriedades
        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }
        public bool Ativo { get; set; }
        #endregion

        #region Construtor
        public Usuario() { }

        private Usuario(string nome, string senha, string email, TipoPerfil perfil, bool ativo)
        {
            Id=Guid.NewGuid().GetHashCode();
            Nome = nome;
            SenhaHash = GerarHashSenha(senha);
            Email = email;
            Perfil = perfil;
            Ativo = ativo;
        }
        public static Usuario Criar(string nome, string senha, string email, TipoPerfil perfil, bool ativo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));
            if (nome.Length < 3 || nome.Length > 100)
                throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres.", nameof(nome));
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(senha));
            if (senha.Length < 8 || !senha.Any(char.IsLetter) || !senha.Any(char.IsDigit) || !senha.Any(c => "!@#$%^&*(),.?\":{}|<>".Contains(c)))
                throw new ArgumentException("A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.", nameof(senha));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));
            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Informe um e-mail válido.", nameof(email));
            if (!Enum.IsDefined(typeof(TipoPerfil), perfil))
                throw new ArgumentException("Perfil inválido.", nameof(perfil));
            return new Usuario(nome, senha, email, perfil, ativo);
        }

        private static string GerarHashSenha(string senha)
        {            
            return BCrypt.Net.BCrypt.HashPassword(senha);
          
        }
        #endregion
    }
}
