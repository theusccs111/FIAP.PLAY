using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
{
    public interface IJogoService : IServiceBase
    {
        Resultado<IEnumerable<JogoResponse>> ObterJogos();
        Resultado<JogoResponse> ObterJogoPorId(long id);
    }
}
