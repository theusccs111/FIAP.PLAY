using FIAP.PLAY.Domain.Shared.Entities;
using System.ComponentModel;

namespace FIAP.PLAY.Domain.Library.Entities
{
    public class GameLibrary : EntityBase
    {
        #region Properties
        public long GameId { get; private set; }
        public Game Game { get; private set; }
        public long LibraryId { get; private set; }
        public Library Library { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public decimal Price { get; private set; }

        #endregion

        #region Constructors

        private GameLibrary() { }

        private GameLibrary(Game game, Library library)
        {
            GameId = game.Id;
            Game = game;
            LibraryId = library.Id;
            Library = library;
            PurchaseDate = DateTime.UtcNow;
            Price = game.Price;
        }
        #endregion

        #region Factory
        public static GameLibrary Create(Library lib, Game game)
        {
            if (lib is null)
                throw new ArgumentException("A biblioteca não pode ser nula.", nameof(lib));

            if (game is null)
                throw new ArgumentException("O jogo não pode ser nulo.", nameof(game));

            return new GameLibrary(game, lib);

        }
        #endregion

    }
}
