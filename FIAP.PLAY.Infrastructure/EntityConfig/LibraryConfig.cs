using FIAP.PLAY.Domain.Library.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class LibraryConfig : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.HasMany(lib => lib.Games)
                .WithOne(gl => gl.Library)
                .HasForeignKey(g => g.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(library => !library.DateDeleted.HasValue);
        }
    }
}
