using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.Biblioteca.Services
{
    public class JogoService : Service, IJogoService
    {
        private readonly IUnityOfWork _uow;
        private readonly IValidator<JogoRequest> _validator;
        private readonly ILoggerManager<JogoService> _loggerManager;
        public JogoService(IHttpContextAccessor httpContextAccessor, IUnityOfWork uow, IValidator<JogoRequest> validator, ILoggerManager<JogoService> loggerManager) : base(httpContextAccessor)
        {
            _uow = uow;
            _validator = validator;
            _loggerManager = loggerManager;
        }

        public Resultado<IEnumerable<JogoResponse>> ObterJogos()
        {
            var jogos = _uow.Jogos.GetAll();
            var jogosResponse = jogos.Select(d => Parse(d)).ToList();
            return new Resultado<IEnumerable<JogoResponse>>(jogosResponse);
        }

        public Resultado<JogoResponse> ObterJogoPorId(long id)
        {
            var jogo = _uow.Jogos.GetById(id);
            var jogoResponse = Parse(jogo);
            return new Resultado<JogoResponse>(jogoResponse);
        }

        public Resultado<JogoResponse> CriarJogo(JogoRequest request)
        {
            var resultadoValidacao = _validator.Validate(request);
            if(resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            
            var jogoCriado = _uow.Jogos.Create(jogo);
            _uow.Complete();

            _loggerManager.LogInformation($"Jogo {jogoCriado.Titulo} criado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogoCriado));
        }

        public Resultado<JogoResponse> AtualizarJogo(long id, JogoRequest request)
        {
            if (id == 0)
                throw new Domain.Shared.Exceptions.ValidationException("id", "id do jogo não pode ser nulo");

            var resultadoValidacao = _validator.Validate(request);
            if (resultadoValidacao.IsValid == false)
                throw new Domain.Shared.Exceptions.ValidationException([.. resultadoValidacao.Errors]);

            var jogo = Parse(request);
            jogo.Id = id;

            _uow.Jogos.Update(jogo);
            _uow.Complete();

            _loggerManager.LogInformation($"Jogo com id {jogo.Id} atualizado com sucesso");
            return new Resultado<JogoResponse>(Parse(jogo));
        }

        private static Jogo Parse(JogoRequest request)
            => Jogo.Criar(request.Titulo, request.Preco, request.Genero, request.AnoLancamento, request.Desenvolvedora);

        private static JogoResponse Parse(Jogo entidade)
            => new(entidade.Id, entidade.Titulo, entidade.Preco, entidade.Genero, entidade.AnoLancamento, entidade.Desenvolvedora);
    }
}
