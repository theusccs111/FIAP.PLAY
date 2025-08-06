using FIAP.PLAY.Application.Biblioteca.Resource.Request;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Response
{
    public sealed record LibraryResponse(long Id, long UserId, List<GameLibraryResponse> games);
}
