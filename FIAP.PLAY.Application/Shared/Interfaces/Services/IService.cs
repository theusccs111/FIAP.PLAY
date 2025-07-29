using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Entities;

namespace FIAP.PLAY.Application.Shared.Interfaces.Services
{
    public interface IService<Entity, Request, Response> where Entity : EntidadeBase where Request : RequestBase where Response : ResponseBase
    {
        Resultado<IEnumerable<Response>> Get();
        Resultado<Response> GetById(int id);
        Resultado<Response> Add(Request request);
        Resultado<Response[]> AddMany(Request[] request);
        Resultado<Response> Update(Request request);
        Resultado<Response[]> UpdateMany(Request[] request);
        Resultado<Response> Delete(long id);
        Resultado<Response[]> DeleteMany(long[] ids);
        void Complete();
    }
}
