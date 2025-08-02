using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.UserAccess.Entities;

namespace FIAP.PLAY.Application.Shared.Interfaces
{
    public interface IUnityOfWork : IDisposable
    {
        IRepository<User> Users { get;}
        IRepository<Game> Games { get; }
        Task CompleteAsync();
        void Complete();
        IRepository<T> Repository<T>() where T : EntityBase;
    }
}
