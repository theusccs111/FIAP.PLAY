using AutoMapper;
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

        private Jogo Parse(JogoRequest request)
            => Jogo.Criar(request.Titulo, request.Preco, request.Genero, request.AnoLancamento, request.Desenvolvedora);

        private JogoResponse Parse(Jogo entidade)
            => new(entidade.Id, entidade.Titulo, entidade.Preco, entidade.Genero, entidade.AnoLancamento, entidade.Desenvolvedora);
    }
}
