using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Tests.Domain.UserAccess
{
    public class UserTests
    {
        [Fact]
        public void CriarUsuario_Valido_DeveCriarUsuario()
        {
            // Arrange
            string name = "Matheus Santana";
            string password = "Senha@123";
            string email = "matheus@fiap.com.br";
            ERole role = ERole.Admin;
            bool active = true;

            // Act
            var usuario = User.Criar(name, password, email, role, active);

            // Assert
            Assert.NotNull(usuario);
            Assert.Equal(name, usuario.Name);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(role, usuario.Role);
            Assert.True(usuario.Active);
            Assert.False(string.IsNullOrWhiteSpace(usuario.PasswordHash));
            Assert.NotEqual(password, usuario.PasswordHash);
        }

        [Fact]
        public void CriarUsuario_NomeVazio_DeveLancarExcecao()
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("", "Senha@123", "email@fiap.com.br", ERole.Admin, true));
            Assert.Equal("Nome não pode ser vazio. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void CriarUsuario_NomeTamanhoInvalido_DeveLancarExcecao()
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Ma", "Senha@123", "email@fiap.com.br", ERole.Admin, true));
            Assert.Equal("Nome deve ter entre 3 e 100 caracteres. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void CriarUsuario_SenhaVazia_DeveLancarExcecao()
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Matheus", "", "email@fiap.com.br", ERole.Admin, true));
            Assert.Equal("Senha não pode ser vazia. (Parameter 'passwordHash')", exception.Message);
        }

        [Theory]
        [InlineData("12345678")] 
        [InlineData("abcdefgh")] 
        [InlineData("abc12345")]
        public void CriarUsuario_SenhaForaDoPadrao_DeveLancarExcecao(string senha)
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Matheus", senha, "email@fiap.com.br", ERole.Admin, true));
            Assert.Equal("A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais. (Parameter 'passwordHash')", exception.Message);
        }

        [Fact]
        public void CriarUsuario_EmailVazio_DeveLancarExcecao()
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Matheus", "Senha@123", "", ERole.Admin, true));
            Assert.Equal("Email não pode ser vazio. (Parameter 'email')", exception.Message);
        }

        [Theory]
        [InlineData("emailsemarroba.com")]
        [InlineData("email@semPonto")]
        public void CriarUsuario_EmailInvalido_DeveLancarExcecao(string email)
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Matheus", "Senha@123", email, ERole.Admin, true));
            Assert.Equal("Informe um e-mail válido. (Parameter 'email')", exception.Message);
        }

        [Fact]
        public void CriarUsuario_PerfilInvalido_DeveLancarExcecao()
        {
            var exception = Assert.Throws<ArgumentException>(() => User.Criar("Matheus", "Senha@123", "email@fiap.com.br", (ERole)999, true));
            Assert.Equal("Perfil inválido. (Parameter 'role')", exception.Message);
        }
    }
}
