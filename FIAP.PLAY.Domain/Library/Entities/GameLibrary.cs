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

        private GameLibrary(long libraryId, long gameId, decimal price)
        {
            GameId = gameId;
            LibraryId = libraryId;
            PurchaseDate = DateTime.UtcNow;
            Price = price;
        }
        #endregion

        #region Factory
        public static GameLibrary Create(Library lib, Game game)
        {
            if (lib is null)
                throw new ArgumentException("A biblioteca não pode ser nula.", nameof(lib));

            if (game is null)
                throw new ArgumentException("O jogo não pode ser nulo.", nameof(game));

            return new GameLibrary(lib.Id, game.Id, game.Price);

        }
        #endregion

    }
}
