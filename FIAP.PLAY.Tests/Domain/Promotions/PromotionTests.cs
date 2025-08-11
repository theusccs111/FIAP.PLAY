using FIAP.PLAY.Domain.Promotions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Tests.Domain.Promotions
{
    public class PromotionTests
    {
        [Fact]
        public void CriarPromocao_Valida_DeveCriarPromocao()
        {
            // Arrange
            decimal discountPercentage = 25.5m;
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(10);
            long campaignId = 1;

            // Act
            var promotion = Promotion.Create(discountPercentage, startDate, endDate, campaignId);

            // Assert
            Assert.NotNull(promotion);
            Assert.Equal(discountPercentage, promotion.DiscountPercentage);
            Assert.Equal(startDate, promotion.StartDate);
            Assert.Equal(endDate, promotion.EndDate);
            Assert.Equal(campaignId, promotion.CampaignId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(101)]
        public void CriarPromocao_DescontoInvalido_DeveLancarExcecao(decimal discount)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(10);
            long campaignId = 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Promotion.Create(discount, startDate, endDate, campaignId));
            Assert.Equal("Percentual de desconto deve ser entre 0.01 e 100. (Parameter 'discountPercentage')", ex.Message);
        }

        [Fact]
        public void CriarPromocao_DataInicioMenorQueHoje_DeveLancarExcecao()
        {
            // Arrange
            decimal discountPercentage = 20m;
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(10);
            long campaignId = 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Promotion.Create(discountPercentage, startDate, endDate, campaignId));
            Assert.Equal("Data de início não pode ser menor que hoje. (Parameter 'startDate')", ex.Message);
        }

        [Fact]
        public void CriarPromocao_DataFimMenorQueInicio_DeveLancarExcecao()
        {
            // Arrange
            decimal discountPercentage = 20m;
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(5);
            long campaignId = 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Promotion.Create(discountPercentage, startDate, endDate, campaignId));
            Assert.Equal("Data final não pode ser menor que data inicial. (Parameter 'endDate')", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CriarPromocao_CampaignIdInvalido_DeveLancarExcecao(long campaignId)
        {
            // Arrange
            decimal discountPercentage = 20m;
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(10);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Promotion.Create(discountPercentage, startDate, endDate, campaignId));
            Assert.Equal("ID da campanha deve ser maior que zero. (Parameter 'campaignId')", ex.Message);
        }
    }
}
