using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Request
{
    public sealed record GameRequest(string Title, decimal Price, EGenre Genre, int YearLaunch, string Developer ) : RequestBase;
}
