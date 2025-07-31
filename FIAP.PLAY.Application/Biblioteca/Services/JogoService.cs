using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FluentValidation;

namespace FIAP.PLAY.Application.Biblioteca.Services
{
    public class JogoService(IUnityOfWork uow, IValidator<JogoRequest> validator, ILoggerManager<JogoService> logger) : ServiceBase, IJogoService
    {
        public Resultado<IEnumerable<JogoResponse>> ObterJogos()
        {
            var jogos = uow.Jogos.GetAll();
            var jogosResponse = jogos.Select(d => Parse(d)).ToList();
            return new Resultado<IEnumerable<JogoResponse>>(jogosResponse);
        }

        public Resultado<JogoResponse> ObterJogoPorId(long id)
        {
            var jogo = uow.Jogos.GetById(id);
            var jogoResponse = Parse(jogo);
            return new Resultado<JogoResponse>(jogoResponse);
        }

        public Resultado<JogoResponse> CriarJogo(JogoRequest request)
        {
            var resultadoValidacao = validator.Validate(request);
            if(resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            
            var jogoCriado = uow.Jogos.Create(jogo);
            uow.Complete();

            logger.LogInformation($"Jogo {jogoCriado.Titulo} criado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogoCriado));
        }

        public Resultado<JogoResponse> AtualizarJogo(long id, JogoRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            jogo.Id = id;

            uow.Jogos.Update(jogo);
            uow.Complete();

            logger.LogInformation($"Jogo com id {jogo.Id} atualizado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogo));
        }

        private static Jogo Parse(JogoRequest request)
            => Jogo.Criar(request.Titulo, request.Preco, request.Genero, request.AnoLancamento, request.Desenvolvedora);

        private static JogoResponse Parse(Jogo entidade)
            => new(entidade.Id, entidade.Titulo, entidade.Preco, entidade.Genero, entidade.AnoLancamento, entidade.Desenvolvedora);
    }
}
