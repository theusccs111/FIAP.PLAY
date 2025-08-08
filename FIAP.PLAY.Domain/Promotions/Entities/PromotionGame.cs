using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Domain.Promotions.Entities
{
    public class PromotionGame : EntityBase
    {
        public long PromotionId { get; private set; }
        public virtual Promotion Promotion { get; private set; }
        public long GameId { get; private set; }
        public virtual Game Game { get; private set; }

        public PromotionGame()
        {
            
        }

        public PromotionGame(long promotionId, long gameId)
        {
            PromotionId = promotionId;
            GameId = gameId;
        }

        public static PromotionGame Create(long promotionId, long gameId)
        {
            if (promotionId <= 0)
                throw new ArgumentException("ID da promoção deve ser maior que zero.", nameof(promotionId));
            if (gameId <= 0)
                throw new ArgumentException("ID do jogo deve ser maior que zero.", nameof(gameId));

            return new PromotionGame(promotionId, gameId);
        }
    }
}
