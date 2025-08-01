using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController(IJogoService _jogoService) : ControllerBase
    {

        [HttpGet]
        public IActionResult ObterJogos()
            => Ok(_jogoService.ObterJogos());

        [HttpGet("{id}")]
        public IActionResult ObterJogoPorId(int id)
            => Ok(_jogoService.ObterJogoPorId(id));

        [HttpPost]
        public IActionResult CriarJogo([FromBody] JogoRequest request)
            => Created("api/Jogo", _jogoService.CriarJogo(request));

        [HttpPut("{id}")]
        public IActionResult AtualizarJogo([FromRoute] long id, [FromBody] JogoRequest request)
            => Ok(_jogoService.AtualizarJogo(id,request));

        //[HttpDelete("{id}")]
        //public IActionResult RemoverJogo(long id)
        //{
        //    _jogoService.Delete(id);
        //    return NoContent();
        //}
    }
}
