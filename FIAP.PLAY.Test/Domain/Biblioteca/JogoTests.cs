using FIAP.PLAY.Domain.Biblioteca.Jogo.Entities;
using FIAP.PLAY.Domain.Biblioteca.Jogo.Enums;
using Xunit;

namespace FIAP.PLAY.Test.Domain.Biblioteca
{
    public class JogoTests
    {
        [Fact]
        public void CriarJogo_Valido_DeveCriarJogo()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = "Nintendo";

            // Act
            var jogo = Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora);

            // Assert
            Assert.NotNull(jogo);
            Assert.Equal(titulo, jogo.Titulo);
            Assert.Equal(preco, jogo.Preco);
            Assert.Equal(genero, jogo.Genero);
            Assert.Equal(anoLancamento, jogo.AnoLancamento);
            Assert.Equal(desenvolvedora, jogo.Desenvolvedora);
        }

        [Fact]
        public void CriarJogo_TituloVazio_DeveLancarExcecao()
        {
            // Arrange
            string titulo = string.Empty;
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = "Nintendo";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Título não pode ser vazio. (Parameter 'titulo')", exception.Message);
        }

    }
}
