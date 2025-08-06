namespace FIAP.PLAY.Application.Biblioteca.Resource.Response
{
    public sealed record GameLibraryResponse(long Id, long LibraryId, GameResponse Game, DateTime PurchaseDate, decimal PricePaid);
}
