using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Domain.Library.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.Promotions.Services
{
    public class CampaignService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<CampaignRequest> validator,
        ILoggerManager<CampaignService> loggerManager) : Service(httpContextAccessor), ICampaignService
    {
        public async Task<Result<IEnumerable<CampaignResponse>>> GetCampaignsAsync()
        {
            var campaigns = await uow.Campaigns.GetAllAsync();
            var campaignsResponse = campaigns.Select(d => Parse(d)).ToList();
            return new Result<IEnumerable<CampaignResponse>>(campaignsResponse);
        }

        public async Task<Result<CampaignResponse>> GetCampaignByIdAsync(long id)
        {
            var campaign = await uow.Campaigns.GetByIdAsync(id);
            var campaignResponse = Parse(campaign);
            return new Result<CampaignResponse>(campaignResponse);
        }

        public async Task<Result<CampaignResponse>> CreateCampaignAsync(CampaignRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var campaign = Parse(request);

            var campaignCreated = await uow.Campaigns.CreateAsync(campaign);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Campanha {campaignCreated.Description} criado com sucesso");
            return new Result<CampaignResponse>(Parse(campaignCreated));
        }

        public async Task<Result<CampaignResponse>> UpdateCampaignAsync(long id, CampaignRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var campaign = Parse(request);
            campaign.Id = id;

            await uow.Campaigns.UpdateAsync(campaign);
            uow.Complete();

            loggerManager.LogInformation($"Campanha com id {campaign.Id} atualizado com sucesso");
            return new Result<CampaignResponse>(Parse(campaign));

        }

        public async Task DeleteCampaignAsync(long id)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            if (await uow.Campaigns.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "Campanha não encontrado");

            await uow.Campaigns.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static Campaign Parse(CampaignRequest request)
            => Campaign.Create(request.Description, request.StartDate, request.EndDate);

        private static CampaignResponse Parse(Campaign entidade)
            => new(entidade.Id, entidade.Description, entidade.StartDate, entidade.EndDate);
    }
}
