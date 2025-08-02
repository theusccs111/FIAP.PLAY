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
        private readonly IUserService _usuarioService;
        public AutenticarController(IUserService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(AutenticarRequest autenticarRequest)
        {
            var response = _usuarioService.Login(autenticarRequest);
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ObterUserLogado()
        {
            var user = _usuarioService.ObterUserLogado();
            return Ok(user);
        }
    }
}
