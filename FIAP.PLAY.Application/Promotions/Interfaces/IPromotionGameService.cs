using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Promotions.Interfaces
{
    public interface IPromotionGameService
    {
        Task<Result<IEnumerable<PromotionGameResponse>>> GetPromotionGamesAsync(CancellationToken cancellationToken);
        Task<Result<PromotionGameResponse>> GetPromotionGameByIdAsync(long id, CancellationToken cancellationToken);
        Task<Result<PromotionGameResponse>> CreatePromotionGameAsync(PromotionGameRequest request, CancellationToken cancellationToken);
        Task<Result<PromotionGameResponse>> UpdatePromotionGameAsync(long id, PromotionGameRequest request, CancellationToken cancellationToken);
        Task DeletePromotionGameAsync(long id, CancellationToken cancellationToken);
    }
}
