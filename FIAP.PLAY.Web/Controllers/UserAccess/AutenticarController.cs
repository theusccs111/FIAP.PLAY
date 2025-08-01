using FIAP.PLAY.Application.UserAccess.Interfaces.Services;
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
        public async Task<IActionResult> LoginAsync(AutenticarRequest autenticarRequest)
        {
            var response = await _usuarioService.LoginAsync(autenticarRequest);
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
