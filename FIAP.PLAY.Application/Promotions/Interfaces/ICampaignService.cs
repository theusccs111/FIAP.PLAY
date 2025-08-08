using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Promotions.Resources.Request;

namespace FIAP.PLAY.Application.Promotions.Interfaces
{
    public interface ICampaignService
    {
        Task<Result<IEnumerable<CampaignResponse>>> GetCampaignsAsync();
        Task<Result<CampaignResponse>> GetCampaignByIdAsync(long id);
        Task<Result<CampaignResponse>> CreateCampaignAsync(CampaignRequest request);
        Task<Result<CampaignResponse>> UpdateCampaignAsync(long id, CampaignRequest request);
        Task DeleteCampaignAsync(long id);
    }
}
