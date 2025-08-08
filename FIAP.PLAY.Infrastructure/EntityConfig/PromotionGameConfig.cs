using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    internal class PromotionGameConfig : IEntityTypeConfiguration<PromotionGame>
    {
        public void Configure(EntityTypeBuilder<PromotionGame> builder)
        {
            builder.HasQueryFilter(c => !c.DateDeleted.HasValue);
        }
    }
}
