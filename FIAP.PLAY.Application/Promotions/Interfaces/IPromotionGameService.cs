using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.PromotionGames.Interfaces
{
    public interface IPromotionGameService
    {
        Task<Result<IEnumerable<PromotionGameResponse>>> GetPromotionGamesAsync();
        Task<Result<PromotionGameResponse>> GetPromotionGameByIdAsync(long id);
        Task<Result<PromotionGameResponse>> CreatePromotionGameAsync(PromotionGameRequest request);
        Task<Result<PromotionGameResponse>> UpdatePromotionGameAsync(long id, PromotionGameRequest request);
        Task DeletePromotionGameAsync(long id);
    }
}
