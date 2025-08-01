using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Resource.Response;
using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Interfaces
{
    public interface IJogoService 
    {
        Resultado<IEnumerable<JogoResponse>> ObterJogos();
        Resultado<JogoResponse> ObterJogoPorId(long id);
        Resultado<JogoResponse> CriarJogo(JogoRequest request);
        Resultado<JogoResponse> AtualizarJogo(long id, JogoRequest request);
        void DeletarJogo(long id);
    }
}
