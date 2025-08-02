using FIAP.PLAY.Application.UserAccess.Resource.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FIAP.PLAY.Application.Shared.Services
{
    public abstract class Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Service(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId() =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string GetUserName() =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        public string GetUserEmail() =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string GetUserRole() =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;


    }
}
