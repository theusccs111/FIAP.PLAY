using FIAP.PLAY.Domain.UserAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasQueryFilter(c => !c.DateDeleted.HasValue);

            builder.OwnsOne(u => u.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Address) // ou o nome real da propriedade string do Email
                    .HasColumnName("Email")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(150)
                    .IsRequired(true);
            });
        }
    }
}
