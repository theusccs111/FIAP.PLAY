namespace FIAP.PLAY.Application.Library.Resource.Response
{
    public sealed record GameLibraryResponse(long Id, long LibraryId, GameResponse Game, DateTime PurchaseDate, decimal PricePaid);
}
