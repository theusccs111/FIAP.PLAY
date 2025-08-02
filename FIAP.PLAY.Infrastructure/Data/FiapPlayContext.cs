using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Infrastructure.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Infrastructure.Data
{
    public class FiapPlayContext : DbContext
    {
        public FiapPlayContext(DbContextOptions<FiapPlayContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                if (entity.GetProperties().Where(p => p.PropertyInfo != null).Any())
                    entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType.Name.Equals("String")).ToList().ForEach(p => p.SetMaxLength(100));

            ApplyConfiguratons(modelBuilder);
        }

        private static void ApplyConfiguratons(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCriacao") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DateCreated").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateCreated").IsModified = false;
                    entry.Property("DateUpdated").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}

