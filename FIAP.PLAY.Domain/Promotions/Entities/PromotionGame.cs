using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Promotions.Entities
{
    public class PromotionGame : EntityBase
    {
        public long PromotionId { get; private set; }
        public virtual Promotion Promotion { get; private set; }
        public long GameId { get; private set; }
        public virtual Game Game { get; private set; }
    }
}
