using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Library.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace FIAP.PLAY.Tests.Application.Promotions.Services
{
    public class CampaignServiceTests
    {
        private readonly ICampaignService _campaignService;
        private readonly Mock<IUnityOfWork> _mockForUOF = new();
        private readonly Mock<IRepository<Campaign>> _mockForRepository = new();
        private readonly Mock<IValidator<CampaignRequest>> _mockForValidator = new();
        private readonly Mock<ILoggerManager<CampaignService>> _mockLogger = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();

        public CampaignServiceTests()
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

            _mockForUOF.Setup(d => d.Campaigns).Returns(_mockForRepository.Object);
            _campaignService = new CampaignService(_mockHttpContext.Object, _mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ObterCampanhasAsync_Valido_DeveRetornarTodosCampanhas()
        {
            // Prepare
            var campaign = Campaign.Create("Campanha de natal", new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));
            var campaigns = new List<Campaign>() { campaign };

            _mockForRepository.Setup(d => d.GetAllAsync()).ReturnsAsync(campaigns);

            // Act
            var resultado = await _campaignService.GetCampaignsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public async Task ObterCampanhaPorIdAsync_Valido_DeveRetornarOCampanhaPeloId()
        {
            // Prepare
            long id = 1;
            var campaign = Campaign.Create("Campanha de natal", new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));
            campaign.Id = id;

            _mockForRepository.Setup(d => d.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(campaign);

            // Act
            var resultado = await _campaignService.GetCampaignByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(campaign.Description, resultado.Data!.Description);
            Assert.Equal(campaign.StartDate, resultado.Data!.StartDate);
            Assert.Equal(campaign.EndDate, resultado.Data!.EndDate);
        }

        [Fact]
        public async Task CriarCampanhaAsync_Valido_DevoConseguirCriarOCampanha()
        {
            // Prepare
            var campaignRequest = new CampaignRequest("Campanha de natal", new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));
            var campaignEntidade = Campaign.Create(
                campaignRequest.Description,
                campaignRequest.StartDate,
                campaignRequest.EndDate);

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<CampaignRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.CreateAsync(It.IsAny<Campaign>()))
                .ReturnsAsync(campaignEntidade);

            // Act
            var resultado = await _campaignService.CreateCampaignAsync(campaignRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(campaignEntidade.Description, resultado.Data!.Description);
            Assert.Equal(campaignEntidade.StartDate, resultado.Data!.StartDate);
            Assert.Equal(campaignEntidade.EndDate, resultado.Data!.EndDate);
        }

        [Fact]
        public async Task CriarCampanhaAsync_Invalido_NaoDevoConseguirCriarCampanhaInvalido()
        {
            // Prepare
            var campaignRequest = new CampaignRequest(string.Empty, new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Description", "Descrição não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<CampaignRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _campaignService.CreateCampaignAsync(campaignRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarCampanhaAsync_Invalido_NaoDevoConseguirAtualizarSemId()
        {
            // Prepare
            var id = 0L;
            var campaignRequest = new CampaignRequest("Campanha de natal", new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));

            // Act
            var resultado = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _campaignService.UpdateCampaignAsync(id, campaignRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarCampanhaAsync_Invalido_NaoDevoConseguirAtualizarUmCampanhaInvalido()
        {
            // Prepare
            var id = 1L;
            var campaignRequest = new CampaignRequest(string.Empty, new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Description", "Descrição não pode ser vazio.")]);
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<CampaignRequest>()))
                .Returns(resultadoValidacaoInvalido);

            // Act
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _campaignService.UpdateCampaignAsync(id, campaignRequest, CancellationToken.None));

            // Assert
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task AtualizarCampanhaAsync_Valido_DevoConseguirAtualizarUmCampanha()
        {
            // Prepare
            var id = 1L;
            var campaignRequest = new CampaignRequest("Campanha de natal", new DateTime(2025, 12, 20), new DateTime(2025, 12, 26));
            var campaignEntidade = Campaign.Create(
                campaignRequest.Description,
                campaignRequest.StartDate,
                campaignRequest.EndDate);
            campaignEntidade.Id = id;

            var resultadoValidacaoSucesso = new ValidationResult();
            _mockForValidator
                .Setup(d => d.Validate(It.IsAny<CampaignRequest>()))
                .Returns(resultadoValidacaoSucesso);

            _mockForRepository
                .Setup(d => d.UpdateAsync(It.IsAny<Campaign>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.Complete())
                .Verifiable();

            // Act
            var resultado = await _campaignService.UpdateCampaignAsync(id, campaignRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(campaignEntidade.Description, resultado.Data!.Description);
            Assert.Equal(campaignEntidade.StartDate, resultado.Data!.StartDate);
            Assert.Equal(campaignEntidade.EndDate, resultado.Data!.EndDate);
            _mockForRepository.VerifyAll();
            _mockForUOF.VerifyAll();
        }

        [Fact]
        public void DeletarCampanhaAsync_Invalido_IdDeveSerInformado()
        {
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() => _campaignService.DeleteCampaignAsync(0, CancellationToken.None));
            Assert.NotNull(resultado);
        }

        [Fact]
        public void DeletarCampanhaAsync_Invalido_CampanhaDeveExistir()
        {
            var id = 1L;
            var resultado = Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _campaignService.DeleteCampaignAsync(id, CancellationToken.None));

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task DeletarCampanhaAsync_Valido_DevoConseguirExcluirOCampanha()
        {
            var id = 1L;

            _mockForRepository.Setup(d => d.ExistsAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            _mockForRepository.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                .Verifiable();

            _mockForUOF
                .Setup(d => d.CompleteAsync())
                .Verifiable();

            await _campaignService.DeleteCampaignAsync(id, CancellationToken.None);

            _mockForRepository.VerifyAll();
        }
    }
}