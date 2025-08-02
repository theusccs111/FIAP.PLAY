using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasQueryFilter(c => !c.DateDeleted.HasValue);
        }
    }
}
