using FIAP.PLAY.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class CampaignConfig : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasQueryFilter(c => !c.DateDeleted.HasValue);
        }
    }
}
