using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.Shared.ValueObject;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Infrastructure.Data
{
    public class DBInitializer
    {
        public static void Initialize(FiapPlayContext context)
        {
            context.Database.Migrate();

            if (context.User.Any())
            {
                return;
            }

            User usuario = new User
            {
                PasswordHash = "Admin1!a".Encrypt(),
                Name = "admin",
                Email = Email.Criar("admin@admin.com"),
                Role = ERole.Admin,
                Active = true,
            };

            context.Add(usuario);
            context.SaveChanges();
        }
    }
}
