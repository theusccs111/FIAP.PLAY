using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;
using System.Runtime.CompilerServices;

namespace FIAP.PLAY.Domain.UserAccess.Entities
{
    public class User : EntityBase
    {
        #region Propriedades
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public ERole Role { get; set; }
        public bool Active { get; set; }
        #endregion

        #region Construtor
        public User() { }

        private User(string name, string passwordHash, string email, ERole role, bool active)
        {
            Name = name;
            PasswordHash = passwordHash.Encrypt();
            Email = email;
            Role = role;
            Active = active;
        }
        public static User Criar(string name, string passwordHash, string email, ERole role, bool active)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(name));
            if (name.Length < 3 || name.Length > 100)
                throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres.", nameof(name));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(passwordHash));
            if (passwordHash.Length < 8 || !passwordHash.Any(char.IsLetter) || !passwordHash.Any(char.IsDigit) || !passwordHash.Any(c => "!@#$%^&*(),.?\":{}|<>".Contains(c)))
                throw new ArgumentException("A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));
            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Informe um e-mail válido.", nameof(email));
            if (!Enum.IsDefined(typeof(ERole), role))
                throw new ArgumentException("Perfil inválido.", nameof(role));

            return new User(name, passwordHash, email, role, active);
        }
        #endregion
    }
}
