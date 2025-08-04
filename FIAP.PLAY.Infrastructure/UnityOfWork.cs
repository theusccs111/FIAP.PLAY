using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Infrastructure.Data;
using FIAP.PLAY.Infrastructure.Repositories;

namespace FIAP.PLAY.Infrastructure
{
    public class UnityOfWork : IUnityOfWork, IDisposable
    {
        private readonly FiapPlayContext _context;
        private Dictionary<string, object> repositories;

        public IRepository<User> Users { get { return new Repository<User>(_context); } }
        public IRepository<Game> Games { get { return new Repository<Game>(_context); } }
        public IRepository<Library> Libraries { get { return new Repository<Library>(_context); } }
        public IRepository<GameLibrary> GameLibraries { get { return new Repository<GameLibrary>(_context); } }


        public UnityOfWork(FiapPlayContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> Repository<T>() where T : EntityBase
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositorioType = typeof(Repository<>);
                var repositorioInstancia = Activator.CreateInstance(repositorioType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositorioInstancia);
            }

            return (Repository<T>)repositories[type];
        }
    }
}
