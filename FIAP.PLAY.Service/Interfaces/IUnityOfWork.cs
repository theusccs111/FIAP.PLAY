using FIAP.PLAY.Domain.Entities;
using FIAP.PLAY.Domain.Entities.Base;
using FIAP.PLAY.Service.Interfaces.Repository;

namespace FIAP.PLAY.Service.Interfaces
{
    public interface IUnityOfWork : IDisposable
    {
        IRepository<User> Users { get;}
        
        Task CompleteAsync();
        void Complete();
        IRepository<T> Repository<T>() where T : EntidadeBase;
    }
}
