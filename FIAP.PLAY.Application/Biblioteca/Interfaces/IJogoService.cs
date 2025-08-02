using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
{
    public interface IJogoService 
    {
        Task<Resultado<IEnumerable<JogoResponse>>> ObterJogosAsync();
        Task<Resultado<JogoResponse>> ObterJogoPorIdAsync(long id);
        Task<Resultado<JogoResponse>> CriarJogoAsync(JogoRequest request);
        Task<Resultado<JogoResponse>> AtualizarJogoAsync(long id, JogoRequest request);
        Task DeletarJogoAsync(long id);
    }
}
