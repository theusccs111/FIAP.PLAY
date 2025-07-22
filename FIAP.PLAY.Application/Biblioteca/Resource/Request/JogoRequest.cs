using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Request
{
    public sealed record JogoRequest(string Titulo, decimal Preco, EGenero Genero, int AnoLancamento, string Desenvolvedora ) : ResourceBase;
}
