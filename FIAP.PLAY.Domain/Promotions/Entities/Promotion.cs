using FIAP.PLAY.Domain.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIAP.PLAY.Domain.Library.Entities
{
    public class Promotion : EntityBase
    {
        #region Properties
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsActive { get; private set; }
        public long CampaignId { get; private set; }
        public ICollection<Game> ApplicableGames { get; private set; } = new HashSet<Game>();
        #endregion

        #region Constructor
        // Construtor privado para EF Core ou serialização
        private Promotion() { }

        private Promotion(string name, string description, decimal discountPercentage,
                        DateTime startDate, DateTime endDate, bool isActive, long campaignId,
                        ICollection<Game> applicableGames)
        {
        
            Name = name;
            Description = description;
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            CampaignId = campaignId;
            ApplicableGames = new HashSet<Game>(applicableGames);
        }
        #endregion

        #region Factory Method
        public static Promotion Create(string name, string description, decimal discountPercentage, DateTime startDate, DateTime endDate, bool isActive,long campaignId, ICollection<Game> applicableGames)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(name));
            if (name.Length < 3 || name.Length > 100)
                throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres.", nameof(name));
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Percentual de desconto deve ser entre 0.01 e 100.", nameof(discountPercentage));
            if (startDate < DateTime.Today.Date)
                throw new ArgumentException("Data de início não pode ser menor que hoje.", nameof(startDate));
            if (endDate < startDate)
                throw new ArgumentException("Data final não pode ser menor que data inicial.", nameof(endDate));
            if (applicableGames == null)
                throw new ArgumentNullException(nameof(applicableGames), "Lista de jogos não pode ser nula.");
            if (applicableGames.Any(g => g == null))
                throw new ArgumentException("Lista de jogos não pode conter itens nulos.", nameof(applicableGames));
            if (campaignId <= 0)
                throw new ArgumentException("ID da campanha deve ser maior que zero.", nameof(campaignId));

            return new Promotion(name, description,discountPercentage, startDate,  endDate, isActive: ShouldActivatePromotion(isActive, startDate, endDate),  campaignId,  applicableGames);
        }

        #endregion

        private static bool ShouldActivatePromotion(bool isActive, DateTime startDate, DateTime endDate)
        {
            var today = DateTime.Today;
            return isActive && today >= startDate && today <= endDate;
        }


        #region Public Methods
        public void AddGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (!ApplicableGames.Contains(game))
            {
                ApplicableGames.Add(game);
            }
        }

        public void UpdateStatus(bool newStatus)
        {
            var today = DateTime.Today;
            if (newStatus && (today < StartDate || today > EndDate))
                throw new InvalidOperationException("Promoção só pode ser ativada dentro do período válido.");

            IsActive = newStatus;
        }
        #endregion
    }
}