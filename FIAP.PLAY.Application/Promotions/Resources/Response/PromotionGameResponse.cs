using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Promotions.Resources.Response
{
    public sealed record PromotionGameResponse(long Id, long PromotionId, long GameId) : ResponseBase;

}
