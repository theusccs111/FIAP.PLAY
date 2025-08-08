using FIAP.PLAY.Domain.Promotions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Tests.Domain.Promotions
{
    public class PromotionGameTests
    {
        [Fact]
        public void CriarPromotionGame_Valido_DeveCriarPromotionGame()
        {
            // Arrange
            long promotionId = 1;
            long gameId = 2;

            // Act
            var promotionGame = PromotionGame.Create(promotionId, gameId);

            // Assert
            Assert.NotNull(promotionGame);
            Assert.Equal(promotionId, promotionGame.PromotionId);
            Assert.Equal(gameId, promotionGame.GameId);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        public void CriarPromotionGame_PromotionIdInvalido_DeveLancarExcecao(long promotionId, long gameId)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => PromotionGame.Create(promotionId, gameId));
            Assert.Equal("ID da promoção deve ser maior que zero. (Parameter 'promotionId')", ex.Message);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -5)]
        public void CriarPromotionGame_GameIdInvalido_DeveLancarExcecao(long promotionId, long gameId)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => PromotionGame.Create(promotionId, gameId));
            Assert.Equal("ID do jogo deve ser maior que zero. (Parameter 'gameId')", ex.Message);
        }
    }
}
