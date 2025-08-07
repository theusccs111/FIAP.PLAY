using FIAP.PLAY.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class PromotionConfig : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
          
            builder.ToTable("Promotions");
         
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
           
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(p => p.StartDate)
                .IsRequired();

            builder.Property(p => p.EndDate)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.CampaignId)
                .IsRequired();

            builder.HasOne<Campaign>()
                .WithMany()
                .HasForeignKey(p => p.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ApplicableGames)
            .WithMany() 
            .UsingEntity<Dictionary<string, object>>(
                "PromotionGames",
                j => j.HasOne<Game>().WithMany().HasForeignKey("GameId"),
                j => j.HasOne<Promotion>().WithMany().HasForeignKey("PromotionId"),
                j => j.HasKey("GameId", "PromotionId"));

            builder.HasQueryFilter(p => !p.DateDeleted.HasValue);         
       }
    }
}

