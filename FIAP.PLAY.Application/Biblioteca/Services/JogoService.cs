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
    public class JogoService(
        IHttpContextAccessor httpContextAccessor,
        IUnityOfWork uow,
        IValidator<JogoRequest> validator,
        ILoggerManager<JogoService> loggerManager) : Service(httpContextAccessor), IJogoService
    {
        public async Task<Resultado<IEnumerable<JogoResponse>>> ObterJogosAsync()
        {
            var jogos = await uow.Jogos.GetAllAsync();
            var jogosResponse = jogos.Select(d => Parse(d)).ToList();
            return new Resultado<IEnumerable<JogoResponse>>(jogosResponse);
        }

        public async Task<Resultado<JogoResponse>> ObterJogoPorIdAsync(long id)
        {
            var jogo = await uow.Jogos.GetByIdAsync(id);
            var jogoResponse = Parse(jogo);
            return new Resultado<JogoResponse>(jogoResponse);
        }

        public async Task<Resultado<JogoResponse>> CriarJogoAsync(JogoRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if(resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            
            var jogoCriado = await uow.Jogos.CreateAsync(jogo);
            await uow.CompleteAsync();

            loggerManager.LogInformation($"Jogo {jogoCriado.Titulo} criado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogoCriado));
        }

        public async Task<Resultado<JogoResponse>> AtualizarJogoAsync(long id, JogoRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            jogo.Id = id;

            await uow.Jogos.UpdateAsync(jogo);
            uow.Complete();

            loggerManager.LogInformation($"Jogo com id {jogo.Id} atualizado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogo));
        }

        public async Task DeletarJogoAsync(long id)
        {
            if(id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            if(await uow.Jogos.ExistsAsync(id) == false)
                throw new Domain.Shared.Exceptions.NotFoundException("id", "jogo não encontrado");

            await uow.Jogos.DeleteAsync(id);
            await uow.CompleteAsync();
        }

        private static Jogo Parse(JogoRequest request)
            => Jogo.Criar(request.Titulo, request.Preco, request.Genero, request.AnoLancamento, request.Desenvolvedora);

        private static JogoResponse Parse(Jogo entidade)
            => new(entidade.Id, entidade.Titulo, entidade.Preco, entidade.Genero, entidade.AnoLancamento, entidade.Desenvolvedora);
    }
}
