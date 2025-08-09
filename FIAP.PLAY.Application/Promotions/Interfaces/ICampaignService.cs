using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Promotions.Resources.Request;

namespace FIAP.PLAY.Application.Promotions.Interfaces
{
    public interface ICampaignService
    {
        Task<Result<IEnumerable<CampaignResponse>>> GetCampaignsAsync(CancellationToken cancellationToken);
        Task<Result<CampaignResponse>> GetCampaignByIdAsync(long id, CancellationToken cancellationToken);
        Task<Result<CampaignResponse>> CreateCampaignAsync(CampaignRequest request, CancellationToken cancellationToken);
        Task<Result<CampaignResponse>> UpdateCampaignAsync(long id, CampaignRequest request, CancellationToken cancellationToken);
        Task DeleteCampaignAsync(long id, CancellationToken cancellationToken);
    }
}
