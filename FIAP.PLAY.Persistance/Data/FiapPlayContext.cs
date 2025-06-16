using FIAP.PLAY.Domain.Entities;
using FIAP.PLAY.Persistance.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace FIAP.PLAY.Persistance.Data
{
    public class FiapPlayContext : DbContext
    {
        public FiapPlayContext(DbContextOptions<FiapPlayContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.GetProperties().Where(p => p.PropertyInfo != null).Any())
                {
                    entity.GetProperties().Where(p => p.PropertyInfo != null && p.PropertyInfo.PropertyType.Name.Equals("String")).ToList().ForEach(p => p.SetMaxLength(100));
                }

            }

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
                    entry.Property("DataCriacao").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCriacao").IsModified = false;
                    entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}

