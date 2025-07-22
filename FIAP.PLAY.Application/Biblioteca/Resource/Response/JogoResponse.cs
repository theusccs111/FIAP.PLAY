using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;

namespace FIAP.PLAY.Application.Biblioteca.Resource.Response
{
    public sealed record JogoResponse(long Id, string Titulo, decimal Preco, EGenero Genero, int AnoLancamento, string Desenvolvedora);
}
