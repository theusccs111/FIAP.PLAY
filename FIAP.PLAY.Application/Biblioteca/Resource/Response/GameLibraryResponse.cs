namespace FIAP.PLAY.Application.Biblioteca.Resource.Response
{
    public sealed record GameLibraryResponse(int Id, LibraryResponse Library, GameResponse Game, DateTime PurchaseDate, decimal PricePaid);
}
