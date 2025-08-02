using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Library.Enums;

namespace FIAP.PLAY.Tests.Domain.Biblioteca
{
    public class GameLibraryTests
    {
        [Fact]
        public void CriarGameLibrary_DeveCriarGameLibraryValida()
        {
            // Arrange
            var library = Library.Create(1);
            var game = Game.Create("Test Game", 59.99m, EGenre.Acao, 2023, "Test Developer");

            // Act
            var gameLibrary = GameLibrary.Create(library, game);

            // Assert
            Assert.NotNull(gameLibrary);
            Assert.Equal(game.Id, gameLibrary.GameId);
            Assert.Equal(library.Id, gameLibrary.LibraryId);
            Assert.Equal(game.Price, gameLibrary.Price);
            Assert.True(gameLibrary.PurchaseDate <= DateTime.UtcNow);
        }

        [Fact]
        public void CriarGameLibrary_ComBibliotecaNula_DeveLancarExcecao()
        {
            // Arrange
            Library library = null!;
            var game = Game.Create("Test Game", 59.99m, EGenre.Acao, 2023, "Test Developer");

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => GameLibrary.Create(library, game));
            Assert.Equal("A biblioteca não pode ser nula. (Parameter 'lib')", exception.Message);
        }

        [Fact]
        public void CriarGameLibrary_ComJogoNulo_DeveLancarExcecao()
        {
            // Arrange
            var library = Library.Create(1);
            Game game = null!;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => GameLibrary.Create(library, game));
            Assert.Equal("O jogo não pode ser nulo. (Parameter 'game')", exception.Message);
        }
    }
}
