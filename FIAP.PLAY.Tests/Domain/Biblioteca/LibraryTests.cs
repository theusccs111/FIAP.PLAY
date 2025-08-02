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

        [Fact]
        public void AdicionarJogo_DeveAdicionarJogoValido()
        {
            // Arrange
            var library = Library.Create(1);
            var game = Game.Create("Test Game", 59.99m, EGenre.Acao, 2023, "Test Developer");
           
            // Act
            library.AdicionarJogo(game);
            
            // Assert
            Assert.Single(library.Games);
            Assert.Equal(game.Id, library.Games.First().GameId);
        }

        [Fact]
        public void AdicionarJogo_JogoJaAdicionado_DeveLancarExcecao()
        {
            // Arrange
            var library = Library.Create(1);
            var game = Game.Create("Test Game", 59.99m, EGenre.Acao, 2023, "Test Developer");

            // Adiciona o jogo pela primeira vez
            library.AdicionarJogo(game); 

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => library.AdicionarJogo(game));
            Assert.Equal("Este jogo já foi adicionado", exception.Message);
        }

        [Fact]
        public void AdicionarJogo_JogoNulo_DeveLancarExcecao()
        {
            // Arrange
            var library = Library.Create(1);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => library.AdicionarJogo(null));
            Assert.Equal("O jogo não pode ser nulo.", exception.Message);
        }
    }
}
