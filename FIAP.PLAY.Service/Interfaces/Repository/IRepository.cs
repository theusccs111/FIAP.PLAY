using FIAP.PLAY.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.PLAY.Service.Interfaces.Repository
{
    public interface IRepository<T> where T : EntidadeBase
    {
        IQueryable<T> GetAll();
        DbSet<T> GetDbSet();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T Search(params object[] key);
        T GetFirst(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Update(T entity);
        void Delete(Func<T, bool> predicate);
        void Delete(T obj);
        bool Exists(long id);
        void Commit();
        void Dispose();
    }
}
