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
    public class PromotionGameService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<PromotionGameRequest> validator,
        ILoggerManager<PromotionGameService> loggerManager) : Service(httpContextAccessor), IPromotionGameService
    {
        public async Task<Result<IEnumerable<PromotionGameResponse>>> GetPromotionGamesAsync(CancellationToken cancellationToken)
        {
            var promotionGames = await uow.PromotionGames.GetAllAsync();
            var promotionGamesResponse = promotionGames.Select(d => Parse(d)).ToList();
            return new Result<IEnumerable<PromotionGameResponse>>(promotionGamesResponse);
        }

        public async Task<Result<PromotionGameResponse>> GetPromotionGameByIdAsync(long id, CancellationToken cancellationToken)
        {
            var promotionGame = await uow.PromotionGames.GetByIdAsync(id);
            var promotionGameResponse = Parse(promotionGame);
            return new Result<PromotionGameResponse>(promotionGameResponse);
        }

        public async Task<Result<PromotionGameResponse>> CreatePromotionGameAsync(PromotionGameRequest request, CancellationToken cancellationToken)
        {
            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var promotionGame = Parse(request);

            var promotionGameCreated = await uow.PromotionGames.CreateAsync(promotionGame);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Promoção criado com sucesso");
            return new Result<PromotionGameResponse>(Parse(promotionGameCreated));
        }

        public async Task<Result<PromotionGameResponse>> UpdatePromotionGameAsync(long id, PromotionGameRequest request, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var promotionGame = Parse(request);
            promotionGame.Id = id;

            await uow.PromotionGames.UpdateAsync(promotionGame);
            uow.Complete();

            loggerManager.LogInformation($"Promoção com id {promotionGame.Id} atualizado com sucesso");
            return new Result<PromotionGameResponse>(Parse(promotionGame));

        }

        public async Task DeletePromotionGameAsync(long id, CancellationToken cancellationToken)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do usuário não pode ser nulo");

            if (await uow.PromotionGames.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "Promoção não encontrado");

            await uow.PromotionGames.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static PromotionGame Parse(PromotionGameRequest request)
            => PromotionGame.Create(request.PromotionId, request.GameId);

        private static PromotionGameResponse Parse(PromotionGame entidade)
            => new(entidade.Id, entidade.PromotionId, entidade.GameId);
    }
}
