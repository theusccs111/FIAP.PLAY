using AutoMapper;
using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FIAP.PLAY.Application.Biblioteca.Services
{
    public class JogoService : Service<Jogo, JogoRequest>, IJogoService
    {
        public JogoService(IHttpContextAccessor httpContextAccessor, IMapper mapper, IUnityOfWork uow, IConfiguration config, IValidator<Jogo> validator, ILoggerManager<JogoService> logger)
            : base(httpContextAccessor, mapper, uow, config, validator)
        {
            _logger = logger;
        }

        private readonly ILoggerManager<JogoService> _logger;

        public void Delete(long id)
        {
            var jogo = Uow.Jogos.GetFirst(x => x.Id == id);

            if(jogo is null) 
            {
                _logger.LogError($"JogoService.Delete - Jogo com ID {id} não encontrado.");
                throw new Domain.Shared.Exceptions.ValidationException("Erro ao deletar", "Jogo não encontrado.");
            }
            Uow.Repository<Jogo>().Delete(jogo);
        }        
    }
}
