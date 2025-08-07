using FIAP.PLAY.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class PromotionConfig : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasQueryFilter(p => !p.DateDeleted.HasValue);
        }
    }
}

