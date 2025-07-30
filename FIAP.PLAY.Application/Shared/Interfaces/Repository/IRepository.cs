using FIAP.PLAY.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.PLAY.Application.Shared.Interfaces.Repository
{
    public interface IRepository<T> where T : EntidadeBase
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        DbSet<T> GetDbSet();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        T Search(params object[] key);
        T GetFirst(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Update(T entity);
        void Delete(Func<T, bool> predicate);
        void Delete(T entity);
        bool Exists(long id);
        void Commit();
        void Dispose();
    }
}
