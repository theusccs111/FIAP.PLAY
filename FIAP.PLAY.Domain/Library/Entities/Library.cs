using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Domain.Library.Entities
{
    public class Library : EntityBase
    {
        #region Fields
        private readonly List<GameLibrary> _games = [];

        #endregion

        #region Construtors
        private Library() { }


        private Library(long userId)
        {
            UserId = userId;
        }
        #endregion

        #region Factory
        public static Library Create(long userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Usuário inválido.", nameof(userId));
            return new Library(userId);
        }
        #endregion

        #region Properties
        public long UserId { get; private set; }
        public IReadOnlyCollection<GameLibrary> Games => _games.AsReadOnly();

        #endregion

        
        #region Métodos
        public void AdicionarJogo(Game game)
        {
            if (game is null)
                throw new ArgumentException("O jogo não pode ser nulo.");

            if (_games.Any(x => x.GameId == game.Id))
                throw new ArgumentException("Este jogo já foi adicionado");

            _games.Add(GameLibrary.Create(this.Id, game.Id, game.Price, DateTime.UtcNow));
        }
        #endregion

    }
}
