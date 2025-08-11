using FIAP.PLAY.Domain.Promotions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Tests.Domain.Promotions
{
    public class CampaignTests
    {
        [Fact]
        public void CriarCampanha_Valida_DeveCriarCampanha()
        {
            // Arrange
            string description = "Campanha de Verão";
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(30);

            // Act
            var campaign = Campaign.Create(description, startDate, endDate);

            // Assert
            Assert.NotNull(campaign);
            Assert.Equal(description, campaign.Description);
            Assert.Equal(startDate, campaign.StartDate);
            Assert.Equal(endDate, campaign.EndDate);
        }

        [Fact]
        public void CriarCampanha_DescricaoVazia_DeveLancarExcecao()
        {
            // Arrange
            string description = "";
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(30);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Campaign.Create(description, startDate, endDate));
            Assert.Equal("Descrição não pode ser vazio. (Parameter 'description')", ex.Message);
        }

        [Fact]
        public void CriarCampanha_DescricaoTamanhoInvalido_DeveLancarExcecao()
        {
            // Arrange
            string description = "ab"; // menos de 3 caracteres
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(30);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Campaign.Create(description, startDate, endDate));
            Assert.Equal("Descrição deve ter entre 3 e 100 caracteres. (Parameter 'description')", ex.Message);
        }

        [Fact]
        public void CriarCampanha_DataInicioMenorQueHoje_DeveLancarExcecao()
        {
            // Arrange
            string description = "Campanha válida";
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(10);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Campaign.Create(description, startDate, endDate));
            Assert.Equal("Data de início não pode ser menor que hoje. (Parameter 'startDate')", ex.Message);
        }

        [Fact]
        public void CriarCampanha_DataFimMenorQueInicio_DeveLancarExcecao()
        {
            // Arrange
            string description = "Campanha válida";
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(5);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Campaign.Create(description, startDate, endDate));
            Assert.Equal("Data final não pode ser menor que data inicial. (Parameter 'endDate')", ex.Message);
        }
    }
}
