using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Application.Promotions.Resources.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Domain.Promotions.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.Promotions.Services
{
    public class PromotionService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<PromotionRequest> validator,
        ILoggerManager<PromotionService> loggerManager) : Service(httpContextAccessor), IPromotionService
    {
        public async Task<Result<IEnumerable<PromotionResponse>>> GetPromotionsAsync(CancellationToken cancellationToken)
        {
            var promotions = await uow.Promotions.GetAllAsync();
            var promotionsResponse = promotions.Select(d => Parse(d)).ToList();
            return new Result<IEnumerable<PromotionResponse>>(promotionsResponse);
        }

        public async Task<Result<PromotionResponse>> GetPromotionByIdAsync(long id, CancellationToken cancellationToken)
        {
            var promotion = await uow.Promotions.GetByIdAsync(id);
            var promotionResponse = Parse(promotion);
            return new Result<PromotionResponse>(promotionResponse);
        }

        public async Task<Result<PromotionResponse>> CreatePromotionAsync(PromotionRequest request, CancellationToken cancellationToken)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            Campaign campaign = await uow.Campaigns.GetFirstAsync(x => x.Id == request.CampaignId);

            if (request.StartDate < campaign.StartDate)
            {
                throw new Domain.Shared.Exceptions.ValidationException("StartDate", "A data de início da promoção não pode ser menor que a data de início da campanha.");
            }

            if (request.EndDate > campaign.EndDate)
            {
                throw new Domain.Shared.Exceptions.ValidationException("EndDate", "A data de fim da promoção não pode ser menor que a data de fim da campanha.");
            }

            var promotion = Parse(request);

            var promotionCreated = await uow.Promotions.CreateAsync(promotion);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Promoção criado com sucesso");
            return new Result<PromotionResponse>(Parse(promotionCreated));
        }

        public async Task<Result<PromotionResponse>> UpdatePromotionAsync(long id, PromotionRequest request, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            Campaign campaign = await uow.Campaigns.GetFirstAsync(x => x.Id == request.CampaignId);

            if (request.StartDate < campaign.StartDate)
            {
                throw new Domain.Shared.Exceptions.ValidationException("StartDate", "A data de início da promoção não pode ser menor que a data de início da campanha.");
            }

            if (request.EndDate > campaign.EndDate)
            {
                throw new Domain.Shared.Exceptions.ValidationException("EndDate", "A data de fim da promoção não pode ser menor que a data de fim da campanha.");
            }

            var promotion = Parse(request);
            promotion.Id = id;

            await uow.Promotions.UpdateAsync(promotion);
            uow.Complete();

            loggerManager.LogInformation($"Promoção com id {promotion.Id} atualizado com sucesso");
            return new Result<PromotionResponse>(Parse(promotion));

        }

        public async Task DeletePromotionAsync(long id, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            if (await uow.Promotions.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "Promoção não encontrado");

            await uow.Promotions.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static Promotion Parse(PromotionRequest request)
            => Promotion.Create(request.DiscountPercentage, request.StartDate, request.EndDate, request.CampaignId);

        private static PromotionResponse Parse(Promotion entidade)
            => new(entidade.Id, entidade.DiscountPercentage, entidade.StartDate, entidade.EndDate, entidade.CampaignId);
    }
}
