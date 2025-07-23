using FIAP.PLAY.Domain.UserAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class UserConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
        }
    }
}
