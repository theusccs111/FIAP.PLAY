using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Promotions.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace FIAP.PLAY.Tests.Application.Promotions.Services
{
    public class PromotionGameServiceTests
    {
        private readonly IPromotionGameService _promotionGameService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Mock<IUnityOfWork> _mockForUOF = new();
        private readonly Mock<IRepository<PromotionGame>> _mockForRepository = new();
        private readonly Mock<IValidator<PromotionGameRequest>> _mockForValidator = new();
        private readonly Mock<ILoggerManager<PromotionGameService>> _mockLogger = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();

        public PromotionGameServiceTests()
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

            _mockForUOF.Setup(d => d.PromotionGames).Returns(_mockForRepository.Object);
            _promotionGameService = new PromotionGameService(_mockHttpContext.Object, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ObterPromocaoJogosAsync_Valido_DeveRetornarTodosPromocaoJogos()
        {
            // Prepare
            var promotionGame = PromotionGame.Create(1,1);
            var promotionGames = new List<PromotionGame>() { promotionGame };

            _mockForRepository.Setup(d => d.GetAllAsync()).ReturnsAsync(promotionGames);

            // Act
            var resultado = await _promotionGameService.GetPromotionGamesAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public async Task ObterPromocaoJogoPorIdAsync_Valido_DeveRetornarOPromocaoJogoPeloId()
        {
            // Prepare
            long id = 1;
            var promotionGame = PromotionGame.Create(1, 1);
            promotionGame.Id = id;

            _mockForRepository.Setup(d => d.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(promotionGame);

            // Act
            var resultado = await _promotionGameService.GetPromotionGameByIdAsync(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(promotionGame.PromotionId, resultado.Data!.PromotionId);
            Assert.Equal(promotionGame.GameId, resultado.Data!.GameId);
        }

        [Fact]
        public async Task CriarPromocaoJogoAsync_Valido_DevoConseguirCriarOPromocaoJogo()
        {
            // Prepare
            var promotionGameRequest = new PromotionGameRequest(1,1);
            var promotionGameEntidade = PromotionGame.Create(
                promotionGameRequest.PromotionId,
                promotionGameRequest.GameId
                );

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionGameRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.CreateAsync(It.IsAny<PromotionGame>()))
                .ReturnsAsync(promotionGameEntidade);

            // Act
            var resultado = await _promotionGameService.CreatePromotionGameAsync(promotionGameRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(promotionGameEntidade.PromotionId, resultado.Data!.PromotionId);
            Assert.Equal(promotionGameEntidade.GameId, resultado.Data!.GameId);
        }

        [Fact]
        public async Task CriarPromocaoJogoAsync_Invalido_NaoDevoConseguirCriarPromocaoJogoInvalido()
        {
            // Prepare
            var promotionGameRequest = new PromotionGameRequest(0,1);

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("PromotionId", "O Id da promoção deve ser informado.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionGameRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionGameService.CreatePromotionGameAsync(promotionGameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoJogoAsync_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var promotionGameRequest = new PromotionGameRequest(1,1);

            // Act
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionGameService.UpdatePromotionGameAsync(id, promotionGameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoJogoAsync_Invalido_NaoDevoConseguirAtualizarUmPromocaoJogoInvalido()
        {
            // Prepare
            var id = 1L;
            var promotionGameRequest = new PromotionGameRequest(0,1);

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("PromotionId", "O Id da promoção deve ser informado.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionGameRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionGameService.UpdatePromotionGameAsync(id, promotionGameRequest));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoJogoAsync_Valido_DevoConseguirAtualizarUmPromocaoJogo()
        {
            // Prepare
            var id = 1L;
            var promotionGameRequest = new PromotionGameRequest(1,1);
            var promotionGameEntidade = PromotionGame.Create(
                promotionGameRequest.PromotionId,
                promotionGameRequest.GameId
                );
            promotionGameEntidade.Id = id;

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionGameRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.UpdateAsync(It.IsAny<PromotionGame>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.Complete())
                .Verifiable();

            // Act
            var resultado = await _promotionGameService.UpdatePromotionGameAsync(id, promotionGameRequest);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(promotionGameEntidade.PromotionId, resultado.Data!.PromotionId);
            Assert.Equal(promotionGameEntidade.GameId, resultado.Data!.GameId);
            _mockForRepository.VerifyAll();
            _mockForUOF.VerifyAll();
        }

        [Fact]
        public void DeletarPromocaoJogoAsync_Invalido_IdDeveSerInformado()
        {
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionGameService.DeletePromotionGameAsync(0));
            Assert.NotNull(resultado);
        }

        [Fact]
        public void DeletarPromocaoJogoAsync_Invalido_PromocaoJogoDeveExistir()
        {
            var id = 1L;
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _promotionGameService.DeletePromotionGameAsync(id));

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DeletarPromocaoJogoAsync_Valido_DevoConseguirExcluirOPromocaoJogo()
        {
            var id = 1L;

            _mockForRepository.Setup(d => d.ExistsAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            _mockForRepository.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.CompleteAsync())
                .Verifiable();

            await _promotionGameService.DeletePromotionGameAsync(id);

            _mockForRepository.VerifyAll();
        }
    }
}