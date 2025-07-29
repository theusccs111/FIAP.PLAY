using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.UserAccess.Entities;

namespace FIAP.PLAY.Application.UserAccess.Interfaces.Services
{
    public interface IUserService : IService<Usuario, UsuarioRequest, UsuarioResponse>
    {
        Resultado<LoginResponse> Login(AutenticarRequest autenticarRequest);
        Resultado<LoginResponse> ObterUserLogado();
    }
}
