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
        public IActionResult Get()
        {
            var jogos = _jogoService.Get();
            return Ok(jogos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jogo = _jogoService.GetById(id);

            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        [HttpPost]
        public IActionResult Post([FromBody] JogoRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            var jogo = _jogoService.Add(request);
            return Created("api/Jogo", jogo);
        }

        [HttpPut]
        public IActionResult Put(JogoRequest request)
        {
            if (request is null)
                return BadRequest("Invalid request");

            var jogo = _jogoService.Update(request);

            if (jogo is null)
                return NotFound();

            return Ok(jogo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _jogoService.Delete(id);
           
            return NoContent();
        }
    }
}
