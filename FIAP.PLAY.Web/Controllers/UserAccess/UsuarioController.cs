using FIAP.PLAY.Application.UserAccess.Interfaces.Services;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.UserAccess
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _usuarioService;
        public UserController(IUserService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //[HttpGet]
        //[Authorize]
        //public IActionResult Get()
        //{
        //    var resultado = _usuarioService.Get();
        //    return Ok(resultado);
        //}

        //[HttpGet("{Id}")]
        //[Authorize]
        //public IActionResult GetById(int Id)
        //{
        //    var resultado = _usuarioService.GetById(Id);
        //    return Ok(resultado);
        //}

        //[HttpPost]
        //[Authorize]
        //public IActionResult Post([FromBody] UsuarioRequest request)
        //{
        //    var dados = _usuarioService.Add(request);
        //    return Created("api/User", dados);
        //}

        //[HttpPut]
        //[Authorize]
        //public IActionResult Put(UsuarioRequest request)
        //{
        //    var dados = _usuarioService.Update(request);
        //    return Ok(dados);
        //}

        //[HttpDelete("{id}")]
        //[Authorize]
        //public IActionResult Delete(long id)
        //{
        //    var dados = _usuarioService.Delete(id);
        //    return NoContent();
        //}
    }
}
