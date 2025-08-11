using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIAP.PLAY.Domain.Promotions.Entities
{
    public class Promotion : EntityBase
    {
        #region Properties
        public decimal DiscountPercentage { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public long CampaignId { get; private set; }
        public virtual Campaign Campaign { get; private set; }
        public ICollection<Game> ApplicableGames { get; private set; } = new HashSet<Game>();
        #endregion

        #region Constructor
        private Promotion() { }

        private Promotion(decimal discountPercentage,
                        DateTime startDate, DateTime endDate, long campaignId)
        {
        
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            CampaignId = campaignId;
        }
        #endregion

        #region Factory Method
        public static Promotion Create(decimal discountPercentage, DateTime startDate, DateTime endDate,long campaignId)
        {
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Percentual de desconto deve ser entre 0.01 e 100.", nameof(discountPercentage));
            if (startDate < DateTime.Today.Date)
                throw new ArgumentException("Data de início não pode ser menor que hoje.", nameof(startDate));
            if (endDate < startDate)
                throw new ArgumentException("Data final não pode ser menor que data inicial.", nameof(endDate));
            if (campaignId <= 0)
                throw new ArgumentException("ID da campanha deve ser maior que zero.", nameof(campaignId));

            return new Promotion(discountPercentage, startDate,  endDate,  campaignId);
        }
        #endregion
    }
}