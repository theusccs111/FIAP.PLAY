using FIAP.PLAY.Application.UserAccess.Helpers;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using Microsoft.AspNetCore.Http;

namespace FIAP.PLAY.Application.Shared.Services
{
    public abstract class Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor { get { return _httpContextAccessor; } }
        private LoginResponse _usuario;
        protected LoginResponse Usuario { get { return _usuario; } set { _usuario = value; } }
        public Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _usuario = LoginResponseHelper.ObterLoginResponse(_httpContextAccessor);
        }

       
    }
}
