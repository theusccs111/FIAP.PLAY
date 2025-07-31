using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Validations;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Validations
{
    public class JogoRequestValidatorTests
    {
        private readonly JogoRequestValidator _validator = new();

        [Fact]
        public void Request_Valido_DeveAceitarUmRequestValido()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.True(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Titulo_Invalido_NaoPodeSerVazio()
        {
            var jogoRequest = new JogoRequest(string.Empty, 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Titulo_Invalido_MenorDoQueOPermitido()
        {
            var jogoRequest = new JogoRequest("A", 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Titulo_Invalido_MaiorDoQueOPermitido()
        {
            var jogoRequest = new JogoRequest(new string('a', 101), 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Preco_Invalido_DeveSerMaiorQueZero()
        {
            var jogoRequest = new JogoRequest("super mario world", -1, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }
        
        [Fact]
        public void Genero_Invalido_DeveExistirDentroDoEnumEGenero()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, 0, 1993, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void AnoLancamento_Invalido_DeveSerMenorOuIgualOAnoAtual()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, DateTime.Now.Year + 1, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void AnoLancamento_Invalido_DeveSerMaiorOuIgualQue1950()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, 1949, "Nintendo");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Desenvolvedora_Invalido_NaoPodeSerVazio()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, 1993, string.Empty);

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Desenvolvedora_Invalido_MenorDoQueOPermitido()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, 1993, "A");

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }

        [Fact]
        public void Desenvolvedora_Invalido_MaiorDoQueOPermitido()
        {
            var jogoRequest = new JogoRequest("super mario world", 100, EGenero.Aventura, 1993, new string('a', 101));

            var resultadoValidacao = _validator.Validate(jogoRequest);

            Assert.NotNull(resultadoValidacao);
            Assert.False(resultadoValidacao.IsValid);
        }
    }
}
