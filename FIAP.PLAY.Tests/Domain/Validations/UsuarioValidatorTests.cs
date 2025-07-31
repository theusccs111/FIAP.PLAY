using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;

namespace FIAP.PLAY.Tests.Domain.Validations
{
    public class UsuarioValidatorTests
    {
        [Fact]
        public void Usuario_DeveCriarComDadosValidos()
        {
            // Arrange & Act
            var usuario = new Usuario(
                Nome: "João Silva",
                email: "joao@fiap.com.br",
                perfil: TipoPerfil.Administrador);

            // Assert
            Assert.Equal("João Silva", usuario.Nome);
            Assert.Equal("joao@fiap.com.br", usuario.Email);
            Assert.Equal(TipoPerfil.Administrador, usuario.Perfil);
        }

        [Theory]
        [InlineData(null)] // Nome nulo
        [InlineData("")]   // Nome vazio
        public void Usuario_NomeInvalido_DeveLancarException(string nomeInvalido)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Usuario(nomeInvalido, "joao@fiap.com.br", TipoPerfil.Administrador));
        }

        [Theory]
        [InlineData("email-invalido")]
        [InlineData("joao.com.br")]
        public void Usuario_EmailInvalido_DeveLancarException(string emailInvalido)
        {
            Assert.Throws<ArgumentException>(() =>
                new Usuario("João Silva", emailInvalido, TipoPerfil.Administrador));
        }
    }
}
