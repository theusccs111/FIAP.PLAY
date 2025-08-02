using FIAP.PLAY.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class GameLibraryConfig : IEntityTypeConfiguration<GameLibrary>
    {
        public void Configure(EntityTypeBuilder<GameLibrary> builder)
        {
            builder.HasKey(bj => bj.Id);

            builder.Property(b => b.LibraryId)
                .IsRequired();

            builder.Property(b => b.GameId)
                .IsRequired();

            builder.Property(b => b.PurchaseDate)
                .IsRequired();

            builder.Property(b => b.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(gl => gl.Game)
                .WithMany()
                .HasForeignKey(gl => gl.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gl => gl.Library)
                .WithMany(lib => lib.Games)
                .HasForeignKey(gl => gl.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
