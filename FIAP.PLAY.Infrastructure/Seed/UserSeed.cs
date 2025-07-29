using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Infrastructure.Seed
{
    public static class UserSeed
    {
        public static void CreateAdminUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Senha = "admin",
                    Nome = "admin",
                    Email = "admin@admin.com",
                    Perfil = TipoPerfil.Administrador,
                    Ativo = true,
                    DataCriacao = new DateTime(2025, 07, 31, 0, 0, 0, 0, 0),
                    DataAlteracao = new DateTime(2025, 07, 31, 0, 0, 0, 0, 0)
                }
            );
        }
    }
}
