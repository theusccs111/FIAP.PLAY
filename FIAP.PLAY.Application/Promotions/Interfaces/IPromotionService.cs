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
        Task<Result<IEnumerable<PromotionResponse>>> GetPromotionsAsync();
        Task<Result<PromotionResponse>> GetPromotionByIdAsync(long id);
        Task<Result<PromotionResponse>> CreatePromotionAsync(PromotionRequest request);
        Task<Result<PromotionResponse>> UpdatePromotionAsync(long id, PromotionRequest request);
        Task DeletePromotionAsync(long id);
    }
}
