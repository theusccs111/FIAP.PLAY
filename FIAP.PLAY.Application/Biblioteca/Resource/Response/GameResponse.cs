using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Library.Enums;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Response
{
    public sealed record GameResponse(long Id, string Title, decimal Price, EGenre Genre, int YearLaunch, string Developer) : ResponseBase;
}
