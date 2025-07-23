using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.PLAY.Infrastructure.EntityConfig
{
    public class JogoConfig : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
           
        }
    }
}
