using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Promotions.Resources.Response
{
    public sealed record PromotionResponse(long Id, decimal DiscountPercentage, DateTime StartDate, DateTime EndDate, long CampaignId) : ResponseBase;

}
