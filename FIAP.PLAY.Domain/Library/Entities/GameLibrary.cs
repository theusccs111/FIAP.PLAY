using System.ComponentModel;

namespace FIAP.PLAY.Domain.Library.Entities
{
    public class GameLibrary
    {
        #region Properties
        public long GameId { get; private set; }
        public long LibraryId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public decimal Price { get; private set; }

        #endregion

        #region Constructors

        private GameLibrary() { }

        private GameLibrary(long libraryId, long gameId, decimal price, DateTime purchaseDate)
        {
            if (libraryId <= 0 || gameId <= 0 || price < 0 || purchaseDate > DateTime.Now)
                throw new ArgumentException("Jogo inválido");

            GameId = gameId;
            LibraryId = libraryId;
            PurchaseDate = purchaseDate;
            Price = price;
        }
        #endregion

        #region Factory
        public static GameLibrary Create(long libraryId, long gameId, decimal price, DateTime purchaseDate)
        {
            return new GameLibrary(libraryId, gameId, price, purchaseDate);
        }
        #endregion

    }
}
