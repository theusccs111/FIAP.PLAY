using FIAP.PLAY.Application.Shared.Resource;

namespace FIAP.PLAY.Application.Library.Resource.Request
{
    public sealed record LibraryRequest(long UserId) : RequestBase;
}
