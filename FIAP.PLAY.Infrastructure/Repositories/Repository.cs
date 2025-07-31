using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.PLAY.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>, IDisposable where T : EntidadeBase
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
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        /// <summary>
        /// Obter dbSet
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetDbSet()
        {
            return _context.Set<T>();
        }

        /// <summary>
        /// Obter dados filtrados por expressão lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T Search(params object[] key)
        {
            return _context.Set<T>().Find(key);
        }

        /// <summary>
        /// Obter primeiro registro conforme expressão lambda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T GetFirst(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetById(long id)
        {
            return _context.Set<T>().First(d => d.Id == id);
        }

        public T Create(T entity)
        {
            return _context.Set<T>().Add(entity).Entity;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Func<T, bool> predicate)
        {
            _context.Set<T>().Where(predicate).ToList().ForEach(del => Delete(del));
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }


        public bool Exists(long id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
