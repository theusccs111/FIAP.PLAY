using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Request
{
    public sealed record GameLibraryRequest(LibraryRequest Library, GameRequest Game) : RequestBase;
}
