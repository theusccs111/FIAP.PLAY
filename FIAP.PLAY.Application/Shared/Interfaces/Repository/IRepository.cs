using FIAP.PLAY.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.PLAY.Application.Shared.Interfaces.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        DbSet<T> GetDbSet();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> SearchAsync(params object[] key);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Func<T, bool> predicate);
        Task DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
        Task CommitAsync();
        void Dispose();
    }
}
