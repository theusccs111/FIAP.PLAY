using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FIAP.PLAY.Domain.Shared.Enums
{
    public enum Field
    {
        [Description("Nome")]
        Name = 0,
        [Description("Valor")]
        Value = 1,
        [Description("Descrição")]
        Description = 2,
        [Description("E-mail")]
        Email = 3,
        [Description("Data de Inicio")]
        StartDate = 4,
        [Description("Data de Fim")]
        EndDate = 5,
        [Description("Id da campanha")]
        CampaignId = 6,
        [Description("Id da promoção")]
        PromotionId = 7,
        [Description("Id do jogo")]
        GameId = 8
    }
}
