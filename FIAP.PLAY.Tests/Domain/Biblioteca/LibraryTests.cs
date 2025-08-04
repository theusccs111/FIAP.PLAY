using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Library.Enums;

namespace FIAP.PLAY.Tests.Domain.Biblioteca
{
    public class LibraryTests
    {
        [Fact]
        public void CriarBiblioteca_DeveCriarBibliotecaValida()
        {
            // Arrange
            long userId = 1;

            // Act
            var library = Library.Create(userId);

            // Assert
            Assert.NotNull(library);
            Assert.Equal(userId, library.UserId);
            Assert.Empty(library.Games);
        }

        [Fact]
        public void CriarBiblioteca_ComUsuarioInvalido_DeveLancarExcecao()
        {
            // Arrange
            long userId = 0; 

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Library.Create(userId));
            Assert.Equal("Usuário inválido. (Parameter 'userId')", exception.Message);
        }

    }
}
