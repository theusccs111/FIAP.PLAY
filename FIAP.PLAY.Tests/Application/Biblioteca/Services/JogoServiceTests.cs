using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class JogoServiceTests
    {
        private readonly IJogoService _jogoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Mock<IUnityOfWork> _mockForUOF = new();
        private readonly Mock<IRepository<Jogo>> _mockForRepository = new();
        private readonly Mock<IValidator<JogoRequest>> _mockForValidator = new();
        private readonly Mock<ILoggerManager<JogoService>> _mockLogger = new();

        public JogoServiceTests()
        {
            _mockForUOF.Setup(d => d.Jogos).Returns(_mockForRepository.Object);
            _jogoService = new JogoService(_httpContextAccessor!, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public void ObterJogos_Valido_DeveRetornarTodosJogos()
        {
            // Prepare
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            var jogos = new List<Jogo>() { jogo };

            _mockForRepository.Setup(d => d.GetAll()).Returns(jogos);

            // Act
            var resultado = _jogoService.ObterJogos();

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public void ObterJogoPorId_Valido_DeveRetornarOJogoPeloId()
        {
            // Prepare
            long id = 1;
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            jogo.Id = id;

            _mockForRepository.Setup(d => d.GetById(It.IsAny<long>())).Returns(jogo);

            // Act
            var resultado = _jogoService.ObterJogoPorId(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(jogo.Preco, resultado.Data!.Preco);
            Assert.Equal(jogo.Titulo, resultado.Data!.Titulo);
            Assert.Equal(jogo.Genero, resultado.Data!.Genero);
            Assert.Equal(jogo.AnoLancamento, resultado.Data!.AnoLancamento);
            Assert.Equal(jogo.Desenvolvedora, resultado.Data!.Desenvolvedora);
        }

        [Fact]
        public void CriarJogo_Valido_DevoConseguirCriarOJogo()
        {
            // Prepare
            var jogoRequest = new JogoRequest("Super mario world", 100, EGenero.Aventura, 1993, "Nintendo");
            var jogoEntidade = Jogo.Criar(
                jogoRequest.Titulo,
                jogoRequest.Preco,
                jogoRequest.Genero,
                jogoRequest.AnoLancamento,
                jogoRequest.Desenvolvedora);

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.Create(It.IsAny<Jogo>()))
                .Returns(jogoEntidade);

            // Act
            var resultado = _jogoService.CriarJogo(jogoRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(jogoEntidade.Preco, resultado.Data!.Preco);
            Assert.Equal(jogoEntidade.Titulo, resultado.Data!.Titulo);
            Assert.Equal(jogoEntidade.Genero, resultado.Data!.Genero);
            Assert.Equal(jogoEntidade.AnoLancamento, resultado.Data!.AnoLancamento);
            Assert.Equal(jogoEntidade.Desenvolvedora, resultado.Data!.Desenvolvedora);
        }

        [Fact]
        public void CriarJogo_Invalido_NaoDevoConseguirCriarJogoInvalido()
        {
            // Prepare
            var jogoRequest = new JogoRequest(string.Empty, 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.Throws<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.CriarJogo(jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public void AtualizarJogo_Valido_DevoConseguirAtualizarUmJogo()
        {
            // Prepare
            var id = 1L;
            var jogoRequest = new JogoRequest("Super mario world", 100, EGenero.Aventura, 1993, "Nintendo");
            var jogoEntidade = Jogo.Criar(
                jogoRequest.Titulo,
                jogoRequest.Preco,
                jogoRequest.Genero,
                jogoRequest.AnoLancamento,
                jogoRequest.Desenvolvedora);
            jogoEntidade.Id = id;

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.Update(It.IsAny<Jogo>()));

            // Act
            var resultado = _jogoService.AtualizarJogo(id, jogoRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(jogoEntidade.Preco, resultado.Data!.Preco);
            Assert.Equal(jogoEntidade.Titulo, resultado.Data!.Titulo);
            Assert.Equal(jogoEntidade.Genero, resultado.Data!.Genero);
            Assert.Equal(jogoEntidade.AnoLancamento, resultado.Data!.AnoLancamento);
            Assert.Equal(jogoEntidade.Desenvolvedora, resultado.Data!.Desenvolvedora);
        }

        [Fact]
        public void AtualizarJogo_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var jogoRequest = new JogoRequest("Super mario world", 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("id", "id do jogo não pode ser nulo.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.Throws<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.AtualizarJogo(id, jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public void AtualizarJogo_Invalido_NaoDevoConseguirAtualizarUmJogoInvalido()
        {
            // Prepare
            var id = 1L;
            var jogoRequest = new JogoRequest(string.Empty, 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.Throws<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.AtualizarJogo(id, jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }
    }
}
