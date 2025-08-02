using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Library.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class GameServiceTests
    {
        private readonly IGameService _gameService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Mock<IUnityOfWork> _mockForUOF = new();
        private readonly Mock<IRepository<Game>> _mockForRepository = new();
        private readonly Mock<IValidator<GameRequest>> _mockForValidator = new();
        private readonly Mock<ILoggerManager<GameService>> _mockLogger = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();

        public GameServiceTests()
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

            _mockForUOF.Setup(d => d.Games).Returns(_mockForRepository.Object);
            _gameService = new GameService(_mockHttpContext.Object, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ObterJogosAsync_Valido_DeveRetornarTodosJogos()
        {
            // Prepare
            var game = Game.Criar("Super mario", 100, EGenre.Acao, 1993, "Nintendo");
            var games = new List<Game>() { game };

            _mockForRepository.Setup(d => d.GetAllAsync()).ReturnsAsync(games);

            // Act
            var resultado = await _gameService.GetGamesAsync();

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
            var game = Game.Criar("Super mario", 100, EGenre.Acao, 1993, "Nintendo");
            game.Id = id;

            _mockForRepository.Setup(d => d.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(game);

            // Act
            var resultado = await _gameService.GetGameByIdAsync(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(game.Price, resultado.Data!.Price);
            Assert.Equal(game.Title, resultado.Data!.Title);
            Assert.Equal(game.Genre, resultado.Data!.Genre);
            Assert.Equal(game.YearLaunch, resultado.Data!.YearLaunch);
            Assert.Equal(game.Developer, resultado.Data!.Developer);
        }

        [Fact]
        public async Task CriarJogoAsync_Valido_DevoConseguirCriarOJogo()
        {
            // Prepare
            var gameRequest = new GameRequest("Super mario world", 100, EGenre.Aventura, 1993, "Nintendo");
            var gameEntidade = Game.Criar(
                gameRequest.Title,
                gameRequest.Price,
                gameRequest.Genre,
                gameRequest.YearLaunch,
                gameRequest.Developer);

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<GameRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.CreateAsync(It.IsAny<Game>()))
                .ReturnsAsync(gameEntidade);

            // Act
            var resultado = await _gameService.CreateGameAsync(gameRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(gameEntidade.Price, resultado.Data!.Price);
            Assert.Equal(gameEntidade.Title, resultado.Data!.Title);
            Assert.Equal(gameEntidade.Genre, resultado.Data!.Genre);
            Assert.Equal(gameEntidade.YearLaunch, resultado.Data!.YearLaunch);
            Assert.Equal(gameEntidade.Developer, resultado.Data!.Developer);
        }

        [Fact]
        public async Task CriarJogoAsync_Invalido_NaoDevoConseguirCriarJogoInvalido()
        {
            // Prepare
            var gameRequest = new GameRequest(string.Empty, 100, EGenre.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<GameRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _gameService.CreateGameAsync(gameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var gameRequest = new GameRequest("Super mario world", 100, EGenre.Aventura, 1993, "Nintendo");

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _gameService.UpdateGameAsync(id, gameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Invalido_NaoDevoConseguirAtualizarUmJogoInvalido()
        {
            // Prepare
            var id = 1L;
            var gameRequest = new GameRequest(string.Empty, 100, EGenre.Aventura, 1993, "Nintendo");

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Titulo", "Título não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<GameRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _gameService.UpdateGameAsync(id, gameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarJogoAsync_Valido_DevoConseguirAtualizarUmJogo()
        {
            // Prepare
            var id = 1L;
            var gameRequest = new GameRequest("Super mario world", 100, EGenre.Aventura, 1993, "Nintendo");
            var gameEntidade = Game.Criar(
                gameRequest.Title,
                gameRequest.Price,
                gameRequest.Genre,
                gameRequest.YearLaunch,
                gameRequest.Developer);
            gameEntidade.Id = id;

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<GameRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.UpdateAsync(It.IsAny<Game>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.Complete())
                .Verifiable();

            // Act
            var resultado = await _gameService.UpdateGameAsync(id, gameRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(gameEntidade.Price, resultado.Data!.Price);
            Assert.Equal(gameEntidade.Title, resultado.Data!.Title);
            Assert.Equal(gameEntidade.Genre, resultado.Data!.Genre);
            Assert.Equal(gameEntidade.YearLaunch, resultado.Data!.YearLaunch);
            Assert.Equal(gameEntidade.Developer, resultado.Data!.Developer);
            _mockForRepository.VerifyAll();
            _mockForUOF.VerifyAll();
        }

        [Fact]
        public void DeletarJogoAsync_Invalido_IdDeveSerInformado()
        {
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _gameService.DeleteGameAsync(0));
            Assert.NotNull(resultado);
        }

        [Fact]
        public void DeletarJogoAsync_Invalido_JogoDeveExistir()
        {
            var id = 1L;
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _gameService.DeleteGameAsync(id));

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

            await _gameService.DeleteGameAsync(id);

            _mockForRepository.VerifyAll();
        }
    }
}
