using FIAP.PLAY.Application.Library.Resource.Request;

namespace FIAP.PLAY.Application.Library.Resource.Response
{
    public sealed record LibraryResponse(long Id, long UserId, List<GameLibraryResponse> games);
}
