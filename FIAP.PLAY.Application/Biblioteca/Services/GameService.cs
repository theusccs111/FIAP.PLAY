using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.Biblioteca.Services
{
    public class GameService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<GameRequest> validator,
        ILoggerManager<GameService> loggerManager) : Service(httpContextAccessor), IGameService
    {
        public async Task<Result<IEnumerable<GameResponse>>> GetGamesAsync()
        {
            var jogos = await uow.Games.GetAllAsync();
            var jogosResponse = jogos.Select(d => Parse(d)).ToList();
            return new Result<IEnumerable<GameResponse>>(jogosResponse);
        }

        public async Task<Result<GameResponse>> GetGameByIdAsync(long id)
        {
            var jogo = await uow.Games.GetByIdAsync(id);
            var jogoResponse = Parse(jogo);
            return new Result<GameResponse>(jogoResponse);
        }

        public async Task<Result<GameResponse>> CreateGameAsync(GameRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if(resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            
            var jogoCriado = await uow.Games.CreateAsync(jogo);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Jogo {jogoCriado.Title} criado com sucesso");
            return new Result<GameResponse>(Parse(jogoCriado));
        }

        public async Task<Result<GameResponse>> UpdateGameAsync(long id, GameRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            jogo.Id = id;

            await uow.Games.UpdateAsync(jogo);
            uow.Complete();

            loggerManager.LogInformation($"Jogo com id {jogo.Id} atualizado com sucesso");
            return new Result<GameResponse>(Parse(jogo));
        }

        public async Task DeleteGameAsync(long id)
        {
            if(id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            if(await uow.Games.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "jogo não encontrado");

            await uow.Games.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static Game Parse(GameRequest request)
            => Game.Criar(request.Title, request.Price, request.Genre, request.YearLaunch, request.Developer);

        private static GameResponse Parse(Game entidade)
            => new(entidade.Id, entidade.Title, entidade.Price, entidade.Genre, entidade.YearLaunch, entidade.Developer);
    }
}
