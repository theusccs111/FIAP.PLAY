using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.PLAY.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>, IDisposable where T : EntityBase
    {
        private readonly FiapPlayContext _context;

        public Repository(FiapPlayContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter todos os dados
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>()
                .ToListAsync();

        /// <summary>
        /// Obter dbSet
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetDbSet() => _context.Set<T>();

        /// <summary>
        /// Obter dados filtrados por expressão lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().Where(predicate).ToListAsync();

        public async Task<T> SearchAsync(params object[] key)
            => await _context.Set<T>().FindAsync(key);

        /// <summary>
        /// Obter primeiro registro conforme expressão lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<T> GetByIdAsync(long id)
            => await _context.Set<T>().FirstAsync(d => d.Id == id);

        public async Task<T> CreateAsync(T entity)
        {
            var result = await _context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        public Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Func<T, bool> predicate)
        {
            _context.Set<T>().Where(predicate).ToList().ForEach(async del => await DeleteAsync(del.Id));
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(long id)
        {
            var entidade = await GetByIdAsync(id);
            entidade.DateDeleted = DateTime.Now;
            _context.Entry(entidade).State = EntityState.Modified;
        }


        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
