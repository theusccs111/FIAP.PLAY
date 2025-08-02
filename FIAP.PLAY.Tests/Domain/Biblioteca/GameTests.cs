using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Library.Enums;

namespace FIAP.PLAY.Tests.Domain.Biblioteca
{
    public class GameTests
    {
        [Fact]
        public void CriarJogo_Valido_DeveCriarJogo()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = "Nintendo";

            // Act
            var jogo = Game.Criar(title, price, genre, yearLaunch, developer);

            // Assert
            Assert.NotNull(jogo);
            Assert.Equal(title, jogo.Title);
            Assert.Equal(price, jogo.Price);
            Assert.Equal(genre, jogo.Genre);
            Assert.Equal(yearLaunch, jogo.YearLaunch);
            Assert.Equal(developer, jogo.Developer);
        }

        [Fact]
        public void CriarJogo_TituloVazio_DeveLancarExcecao()
        {
            // Arrange
            string title = string.Empty;
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = "Nintendo";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Título não pode ser vazio. (Parameter 'title')", exception.Message);
        }

        [Fact]
        public void CriarJogo_PrecosNegativo_DeveLancarExcecao()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = -59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Preço deve ser maior que zero. (Parameter 'price')", exception.Message);
        }

        [Fact]
        public void CriarJogo_GeneroInvalido_DeveLancarExcecao()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = 59.99m;
            EGenre genre = (EGenre)999;
            int yearLaunch = 2017;
            string developer = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Gênero inválido. (Parameter 'genre')", exception.Message);
        }

        [Fact]
        public void CriarJogo_AnoLancamentoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 1949;
            string developer = "Nintendo";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Ano de lançamento inválido. (Parameter 'yearLaunch')", exception.Message);
        }

        [Fact]
        public void CriarJogo_DesenvolvedoraVazia_DeveLancarExcecao()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = string.Empty;
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Desenvolvedora não pode ser vazia. (Parameter 'developer')", exception.Message);
        }

        [Fact]
        public void CriarJogo_DesenvolvedoraTamanhoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string title = "The Legend of Zelda: Breath of the Wild";
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = "N"; // Tamanho inválido (1 caractere)
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Desenvolvedora deve ter entre 3 e 100 caracteres. (Parameter 'developer')", exception.Message);
        }

        [Fact]
        public void CriarJogo_TituloTamanhoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string title = "Z"; // Tamanho inválido (1 caractere)
            decimal price = 59.99m;
            EGenre genre = EGenre.Aventura;
            int yearLaunch = 2017;
            string developer = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Game.Criar(title, price, genre, yearLaunch, developer));
            Assert.Equal("Título deve ter entre 3 e 100 caracteres. (Parameter 'title')", exception.Message);
        }
    }
}
