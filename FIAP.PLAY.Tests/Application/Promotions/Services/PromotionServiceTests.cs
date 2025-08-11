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
using System.Linq.Expressions;
using System.Security.Claims;

namespace FIAP.PLAY.Tests.Application.Promotions.Services
{
    public class PromotionServiceTests
    {
        private readonly IPromotionService _promotionService;
        private readonly Mock<IUnityOfWork> _mockForUOF = new();
        private readonly Mock<IRepository<Promotion>> _mockForRepository = new();
        private readonly Mock<IRepository<Campaign>> _mockForCampaignRepository = new();

        private readonly Mock<IValidator<PromotionRequest>> _mockForValidator = new();
        private readonly Mock<ILoggerManager<PromotionService>> _mockLogger = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();

        public PromotionServiceTests()
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

            _mockForUOF.Setup(d => d.Promotions).Returns(_mockForRepository.Object);
            _mockForUOF.Setup(d => d.Campaigns).Returns(_mockForCampaignRepository.Object);
            _promotionService = new PromotionService(_mockHttpContext.Object, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ObterPromocaosAsync_Valido_DeveRetornarTodosPromocaos()
        {
            // Prepare
            var promotion = Promotion.Create(0.3m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);
            var promotions = new List<Promotion>() { promotion };

            _mockForRepository.Setup(d => d.GetAllAsync()).ReturnsAsync(promotions);

            // Act
            var resultado = await _promotionService.GetPromotionsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public async Task ObterPromocaoPorIdAsync_Valido_DeveRetornarOPromocaoPeloId()
        {
            // Prepare
            long id = 1;
            var promotion = Promotion.Create(0.3m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);
            promotion.Id = id;

            _mockForRepository.Setup(d => d.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(promotion);

            // Act
            var resultado = await _promotionService.GetPromotionByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(promotion.DiscountPercentage, resultado.Data!.DiscountPercentage);
            Assert.Equal(promotion.StartDate, resultado.Data!.StartDate);
            Assert.Equal(promotion.EndDate, resultado.Data!.EndDate);
            Assert.Equal(promotion.CampaignId, resultado.Data!.CampaignId);
        }

        [Fact]
        public async Task CriarPromocaoAsync_Valido_DevoConseguirCriarOPromocao()
        {
            // Prepare
            Campaign campaign = Campaign.Create("Campanha Teste", new DateTime(2025, 12, 1), new DateTime(2025, 12, 30));
            campaign.Id = 1;

            var promotionRequest = new PromotionRequest(0.3m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 30), 1);
            var promotionEntidade = Promotion.Create(
                promotionRequest.DiscountPercentage,
                promotionRequest.StartDate,
                promotionRequest.EndDate,
                promotionRequest.CampaignId
                );

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForCampaignRepository
                .Setup(x => x.GetFirstAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                .ReturnsAsync(campaign);

            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.CreateAsync(It.IsAny<Promotion>()))
                .ReturnsAsync(promotionEntidade);

            // Act
            var resultado = await _promotionService.CreatePromotionAsync(promotionRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(promotionEntidade.DiscountPercentage, resultado.Data!.DiscountPercentage);
            Assert.Equal(promotionEntidade.StartDate, resultado.Data!.StartDate);
            Assert.Equal(promotionEntidade.EndDate, resultado.Data!.EndDate);
            Assert.Equal(promotionEntidade.CampaignId, resultado.Data!.CampaignId);
        }

        [Fact]
        public async Task CriarPromocaoAsync_Invalido_NaoDevoConseguirCriarPromocaoInvalido()
        {
            // Prepare
            Campaign campaign = Campaign.Create("Campanha Teste", new DateTime(2025, 12, 1), new DateTime(2025, 12, 30));
            campaign.Id = 1;
            var promotionRequest = new PromotionRequest(0m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("DiscountPercentage", "O percentual de desconto deve ser maior que zero.")]);

            _mockForCampaignRepository
                .Setup(x => x.GetFirstAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                .ReturnsAsync(campaign);

            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionService.CreatePromotionAsync(promotionRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoAsync_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var promotionRequest = new PromotionRequest(0.3m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);

            // Act
            var resultado = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionService.UpdatePromotionAsync(id, promotionRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoAsync_Invalido_NaoDevoConseguirAtualizarUmPromocaoInvalido()
        {
            // Prepare
            var id = 1L;
            Campaign campaign = Campaign.Create("Campanha Teste", new DateTime(2025, 12, 1), new DateTime(2025, 12, 30));
            campaign.Id = 1;
            var promotionRequest = new PromotionRequest(0m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("DiscountPercentage", "O percentual de desconto deve ser maior que zero.")]);

            _mockForCampaignRepository
                .Setup(x => x.GetFirstAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                .ReturnsAsync(campaign);

            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionService.UpdatePromotionAsync(id, promotionRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarPromocaoAsync_Valido_DevoConseguirAtualizarUmPromocao()
        {
            // Prepare
            var id = 1L;
            Campaign campaign = Campaign.Create("Campanha Teste", new DateTime(2025, 12, 1), new DateTime(2025, 12, 30));
            campaign.Id = 1;

            var promotionRequest = new PromotionRequest(0.3m, new DateTime(2025, 12, 1), new DateTime(2025, 12, 1), 1);
            var promotionEntidade = Promotion.Create(
                promotionRequest.DiscountPercentage,
                promotionRequest.StartDate,
                promotionRequest.EndDate,
                promotionRequest.CampaignId
                );
            promotionEntidade.Id = id;

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForCampaignRepository
                .Setup(x => x.GetFirstAsync(It.IsAny<Expression<Func<Campaign, bool>>>()))
                .ReturnsAsync(campaign);

            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<PromotionRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.UpdateAsync(It.IsAny<Promotion>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.Complete())
                .Verifiable();

            // Act
            var resultado = await _promotionService.UpdatePromotionAsync(id, promotionRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(promotionEntidade.DiscountPercentage, resultado.Data!.DiscountPercentage);
            Assert.Equal(promotionEntidade.StartDate, resultado.Data!.StartDate);
            Assert.Equal(promotionEntidade.EndDate, resultado.Data!.EndDate);
            Assert.Equal(promotionEntidade.CampaignId, resultado.Data!.CampaignId);
            _mockForRepository.VerifyAll();
            _mockForUOF.VerifyAll();
        }

        [Fact]
        public void DeletarPromocaoAsync_Invalido_IdDeveSerInformado()
        {
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _promotionService.DeletePromotionAsync(0,CancellationToken.None));
            Assert.NotNull(resultado);
        }

        [Fact]
        public void DeletarPromocaoAsync_Invalido_PromocaoDeveExistir()
        {
            var id = 1L;
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _promotionService.DeletePromotionAsync(id, CancellationToken.None));

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DeletarPromocaoAsync_Valido_DevoConseguirExcluirOPromocao()
        {
            var id = 1L;

            _mockForRepository.Setup(d => d.ExistsAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            _mockForRepository.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.CompleteAsync())
                .Verifiable();

            await _promotionService.DeletePromotionAsync(id, CancellationToken.None);

            _mockForRepository.VerifyAll();
        }
    }
}