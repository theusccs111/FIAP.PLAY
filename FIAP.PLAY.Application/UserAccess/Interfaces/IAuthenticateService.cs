using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Resource.Response;

namespace FIAP.PLAY.Application.UserAccess.Interfaces
{
    public interface IAuthenticateService
    {
      
        Resultado<LoginResponse> Login(AutenticarRequest autenticarRequest);
        Resultado<LoginResponse> GetLoggedUser();
    }
}
