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
using System.Security.Claims;

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
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();

        public JogoServiceTests()
        {
            // Configurar Claims de usuário simulado
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "usuario_teste"),
                new Claim(ClaimTypes.Email, "teste@fiap.com.br"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _mockHttpContext.Setup(x => x.HttpContext).Returns(mockHttpContext);

            _mockForUOF.Setup(d => d.Jogos).Returns(_mockForRepository.Object);
            _jogoService = new JogoService(_mockHttpContext.Object, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ObterJogosAsync_Valido_DeveRetornarTodosJogos()
        {
            // Prepare
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            var jogos = new List<Jogo>() { jogo };

            _mockForRepository.Setup(d => d.GetAllAsync()).ReturnsAsync(jogos);

            // Act
            var resultado = await _jogoService.ObterJogosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public async Task ObterJogoPorIdAsync_Valido_DeveRetornarOJogoPeloId()
        {
            // Prepare
            long id = 1;
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            jogo.Id = id;

            _mockForRepository.Setup(d => d.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(jogo);

            // Act
            var resultado = await _jogoService.ObterJogoPorIdAsync(id);

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
        public async Task CriarJogoAsync_Valido_DevoConseguirCriarOJogo()
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
                .Setup(d => d.CreateAsync(It.IsAny<Jogo>()))
                .ReturnsAsync(jogoEntidade);

            // Act
            var resultado = await _jogoService.CriarJogoAsync(jogoRequest);

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
        public async Task CriarJogoAsync_Invalido_NaoDevoConseguirCriarJogoInvalido()
        {
            // Prepare
            var jogoRequest = new JogoRequest(string.Empty, 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.CriarJogoAsync(jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var jogoRequest = new JogoRequest("Super mario world", 100, EGenero.Aventura, 1993, "Nintendo");

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.AtualizarJogoAsync(id, jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Invalido_NaoDevoConseguirAtualizarUmJogoInvalido()
        {
            // Prepare
            var id = 1L;
            var jogoRequest = new JogoRequest(string.Empty, 100, EGenero.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<JogoRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.AtualizarJogoAsync(id, jogoRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Valido_DevoConseguirAtualizarUmJogo()
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
                .Setup(d => d.UpdateAsync(It.IsAny<Jogo>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.Complete())
                .Verifiable();

            // Act
            var resultado = await _jogoService.AtualizarJogoAsync(id, jogoRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(jogoEntidade.Preco, resultado.Data!.Preco);
            Assert.Equal(jogoEntidade.Titulo, resultado.Data!.Titulo);
            Assert.Equal(jogoEntidade.Genero, resultado.Data!.Genero);
            Assert.Equal(jogoEntidade.AnoLancamento, resultado.Data!.AnoLancamento);
            Assert.Equal(jogoEntidade.Desenvolvedora, resultado.Data!.Desenvolvedora);
            _mockForRepository.VerifyAll();
            _mockForUOF.VerifyAll();
        }

        [Fact]
        public void DeletarJogoAsync_Invalido_IdDeveSerInformado()
        {
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _jogoService.DeletarJogoAsync(0));
            Assert.NotNull(resultado);
        }

        [Fact]
        public void DeletarJogoAsync_Invalido_JogoDeveExistir()
        {
            var id = 1L;
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _jogoService.DeletarJogoAsync(id));

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DeletarJogoAsync_Valido_DevoConseguirExcluirOJogo()
        {
            var id = 1L;

            _mockForRepository.Setup(d => d.ExistsAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            _mockForRepository.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.CompleteAsync())
                .Verifiable();

            await _jogoService.DeletarJogoAsync(id);

            _mockForRepository.VerifyAll();
        }
    }
}
