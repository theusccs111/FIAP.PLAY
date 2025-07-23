using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;

namespace FIAP.PLAY.Tests.Domain.Biblioteca
{
    public class JogoTests
    {/*
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

        [Fact]
        public void CriarJogo_PrecosNegativo_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = -59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Preço deve ser maior que zero. (Parameter 'preco')", exception.Message);
        }

        [Fact]
        public void CriarJogo_GeneroInvalido_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = 59.99m;
            EGenero genero = (EGenero)999;
            int anoLancamento = 2017;
            string desenvolvedora = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Gênero inválido. (Parameter 'genero')", exception.Message);
        }

        [Fact]
        public void CriarJogo_AnoLancamentoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 1949; 
            string desenvolvedora = "Nintendo";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Ano de lançamento inválido. (Parameter 'anoLancamento')", exception.Message);
        }

        [Fact]
        public void CriarJogo_DesenvolvedoraVazia_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = string.Empty;
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Desenvolvedora não pode ser vazia. (Parameter 'desenvolvedora')", exception.Message);
        }

        [Fact]
        public void CriarJogo_DesenvolvedoraTamanhoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "The Legend of Zelda: Breath of the Wild";
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = "N"; // Tamanho inválido (1 caractere)
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Desenvolvedora deve ter entre 3 e 100 caracteres. (Parameter 'desenvolvedora')", exception.Message);
        }

        [Fact]
        public void CriarJogo_TituloTamanhoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string titulo = "Z"; // Tamanho inválido (1 caractere)
            decimal preco = 59.99m;
            EGenero genero = EGenero.Aventura;
            int anoLancamento = 2017;
            string desenvolvedora = "Nintendo";
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Jogo.Criar(titulo, preco, genero, anoLancamento, desenvolvedora));
            Assert.Equal("Título deve ter entre 3 e 100 caracteres. (Parameter 'titulo')", exception.Message);
        }*/
    }
}
