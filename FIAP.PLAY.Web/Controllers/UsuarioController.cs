using FIAP.PLAY.Domain.Resource.Request;
using FIAP.PLAY.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _usuarioService;
        public UserController(UserService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Autenticar")]
        [AllowAnonymous]
        public IActionResult Autenticar(AutenticarRequest autenticarRequest)
        {
            var response = _usuarioService.Autenticar(autenticarRequest);
            return Ok(response);
        }

        [HttpGet("obter-usuario-logado")]
        [Authorize]
        public IActionResult ObterUserLogado()
        {
            var user = _usuarioService.ObterUserLogado();
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var resultado = _usuarioService.Get();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        [Authorize]
        public IActionResult GetById(int Id)
        {
            var resultado = _usuarioService.GetById(Id);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(UsuarioRequest request)
        {
            var dados = _usuarioService.Add(request);
            return Ok(dados);
        }

        [HttpPost("Many")]
        [Authorize]
        public IActionResult PostMany(UsuarioRequest[] request)
        {
            var dados = _usuarioService.AddMany(request);
            return Ok(dados);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put(UsuarioRequest request)
        {
            var dados = _usuarioService.Update(request);
            return Ok(dados);
        }

        [HttpPut("Many")]
        [Authorize]
        public IActionResult PutMany(UsuarioRequest[] request)
        {
            var dados = _usuarioService.UpdateMany(request);
            return Ok(dados);
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(UsuarioRequest request)
        {
            var dados = _usuarioService.Delete(request);
            return Ok(dados);
        }

        [HttpDelete("Many")]
        [Authorize]
        public IActionResult DeleteMany(UsuarioRequest[] request)
        {
            var dados = _usuarioService.DeleteMany(request);
            return Ok(dados);
        }
    }
}
