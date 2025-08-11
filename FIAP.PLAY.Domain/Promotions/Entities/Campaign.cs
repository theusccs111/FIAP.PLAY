using FIAP.PLAY.Domain.Library.Enums;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Domain.Promotions.Entities
{
    public class Campaign : EntityBase
    {

        #region Properties   
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region Construtor

        private Campaign() { }

        private Campaign(string description, DateTime startDate, DateTime endDate)
        {
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static Campaign Create(string description, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição não pode ser vazio.", nameof(description));
            if (description.Length < 3 || description.Length > 100)
                throw new ArgumentException("Descrição deve ter entre 3 e 100 caracteres.", nameof(description));
            if (startDate < DateTime.Today.Date)
                throw new ArgumentException("Data de início não pode ser menor que hoje.", nameof(startDate));
            if (endDate < startDate)
                throw new ArgumentException("Data final não pode ser menor que data inicial.", nameof(endDate));

            return new Campaign(description, startDate, endDate);
        }
        #endregion

    }
}

