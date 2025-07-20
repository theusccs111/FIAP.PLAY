using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Resource.Request;
using FIAP.PLAY.Domain.Shared.Resource.Response;

namespace FIAP.PLAY.Application.UserAccess.Interfaces.Services
{
    public interface IUserService
    {
        Resultado<LoginResponse> Autenticar(AutenticarRequest autenticarRequest);
        Resultado<LoginResponse> ObterUserLogado();
        Resultado<IEnumerable<UsuarioResponse>> Get();
        Resultado<UsuarioRequest> Add(UsuarioRequest request);
        Resultado<UsuarioRequest[]> AddMany(UsuarioRequest[] request);
        Resultado<UsuarioRequest> Update(UsuarioRequest request);
        Resultado<UsuarioRequest[]> UpdateMany(UsuarioRequest[] request);
        Resultado<UsuarioRequest> Delete(UsuarioRequest request);
        Resultado<UsuarioRequest[]> DeleteMany(UsuarioRequest[] request);
    }
}
