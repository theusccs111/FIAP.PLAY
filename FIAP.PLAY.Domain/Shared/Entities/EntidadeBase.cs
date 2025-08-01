namespace FIAP.PLAY.Domain.Shared.Entities
{
    public class EntidadeBase
    {
        public long Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataExclusao { get; set; }
    }
}
