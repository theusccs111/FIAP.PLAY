using FIAP.PLAY.Domain.Biblioteca.Jogo.Enums;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Domain.Biblioteca.Jogo.Entities
{
    public class Jogo : EntidadeBase
    {
       
        #region Propriedades
        public string Titulo { get; private set; }
        public decimal Preco { get; private set; }
        public EGenero Genero { get; private set; }
        public int AnoLancamento { get; private set; }
        public string Desenvolvedora { get; private set; }
        #endregion

        #region Construtor
        private Jogo(string titulo, decimal preco, EGenero genero, int anoLancamento, string desenvolvedora)
        {
            Titulo = titulo;
            Preco = preco;
            Genero = genero;
            AnoLancamento = anoLancamento;
            Desenvolvedora = desenvolvedora;
        }
        #endregion

        #region Fábrica
        public static Jogo Criar(string titulo, decimal preco, EGenero genero, int anoLancamento, string desenvolvedora)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título não pode ser vazio.", nameof(titulo));
            if(titulo.Length < 3 || titulo.Length > 100)
                throw new ArgumentException("Título deve ter entre 3 e 100 caracteres.", nameof(titulo));
            if (preco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.", nameof(preco));
            if (!Enum.IsDefined(typeof(EGenero), genero))
                throw new ArgumentException("Gênero inválido.", nameof(genero));
            if (anoLancamento < 1950 || anoLancamento > DateTime.Now.Year)
                throw new ArgumentException("Ano de lançamento inválido.", nameof(anoLancamento));
            if (string.IsNullOrWhiteSpace(desenvolvedora))
                throw new ArgumentException("Desenvolvedora não pode ser vazia.", nameof(desenvolvedora));
            if (desenvolvedora.Length < 3 || desenvolvedora.Length > 100)
                throw new ArgumentException("Desenvolvedora deve ter entre 3 e 100 caracteres.", nameof(desenvolvedora));
            return new Jogo(titulo, preco, genero, anoLancamento, desenvolvedora);
        }
        #endregion       
    }
}

