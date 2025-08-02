using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.UserAccess
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticarController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        public AutenticarController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(AutenticarRequest autenticarRequest)
        {
            var response = _authenticateService.Login(autenticarRequest);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetLoggedUser()
        {
            var user = _authenticateService.GetLoggedUser();
            return Ok(user);
        }
    }
}
