using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Shared.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Promotions.Interfaces
{
    public interface IPromotionService
    {
        Task<Result<IEnumerable<PromotionResponse>>> GetPromotionsAsync(CancellationToken cancellationToken);
        Task<Result<PromotionResponse>> GetPromotionByIdAsync(long id, CancellationToken cancellationToken);
        Task<Result<PromotionResponse>> CreatePromotionAsync(PromotionRequest request, CancellationToken cancellationToken);
        Task<Result<PromotionResponse>> UpdatePromotionAsync(long id, PromotionRequest request, CancellationToken cancellationToken);
        Task DeletePromotionAsync(long id, CancellationToken cancellationToken);
    }
}
