using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
{
    public interface IJogoService : IService<Jogo, JogoRequest>
    {
        public void Delete(long id);
    }
}
