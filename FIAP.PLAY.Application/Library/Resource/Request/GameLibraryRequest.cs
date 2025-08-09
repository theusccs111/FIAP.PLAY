using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Resource.Request
{
    public sealed record GameLibraryRequest(LibraryRequest Library, GameRequest Game) : RequestBase;
}
