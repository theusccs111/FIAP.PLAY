using FIAP.PLAY.Domain.Library.Enums;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Domain.Library.Entities
{
    public class Game : EntityBase
    {     

        #region Properties
        public string Title { get; set; }
        public decimal Price { get; set; }
        public EGenre Genre { get; set; }
        public int YearLaunch { get; set; }
        public string Developer { get; set; }
        #endregion

        #region Construtor

        private Game() { }

        private Game(string title, decimal price, EGenre genre, int yearLaunch, string developer)
        {
            Title = title;
            Price = price;
            Genre = genre;
            YearLaunch = yearLaunch;
            Developer = developer;
        }

        public static Game Criar(string title, decimal price, EGenre genre, int yearLaunch, string developer)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Título não pode ser vazio.", nameof(title));
            if (title.Length < 3 || title.Length > 100)
                throw new ArgumentException("Título deve ter entre 3 e 100 caracteres.", nameof(title));
            if (price <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.", nameof(price));
            if (!Enum.IsDefined(typeof(EGenre), genre))
                throw new ArgumentException("Gênero inválido.", nameof(genre));
            if (yearLaunch < 1950 || yearLaunch > DateTime.Now.Year)
                throw new ArgumentException("Ano de lançamento inválido.", nameof(yearLaunch));
            if (string.IsNullOrWhiteSpace(developer))
                throw new ArgumentException("Desenvolvedora não pode ser vazia.", nameof(developer));
            if (developer.Length < 3 || developer.Length > 100)
                throw new ArgumentException("Desenvolvedora deve ter entre 3 e 100 caracteres.", nameof(developer));

            return new Game(title, price, genre, yearLaunch, developer);
        }
        #endregion
         
    }
}

