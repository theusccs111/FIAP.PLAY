using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Application.Shared.Interfaces.Services
{
    public interface IService<T, R> where T : EntidadeBase where R : ResourceBase
    {
        Resultado<IEnumerable<R>> Get();
        Resultado<R> GetById(int id);
        Resultado<R> Add(R request);
        Resultado<R[]> AddMany(R[] request);
        Resultado<R> Update(R request);
        Resultado<R[]> UpdateMany(R[] request);
        Resultado<R> Delete(long id);
        Resultado<R[]> DeleteMany(long[] ids);
        void Complete();
    }
}
