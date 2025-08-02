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
        public async Task<IActionResult> ObterJogosAsync()
            => Ok(await _jogoService.ObterJogosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterJogoPorIdAsync(int id)
            => Ok(await _jogoService.ObterJogoPorIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CriarJogoAsync([FromBody] JogoRequest request)
            => Created("api/Jogo", await _jogoService.CriarJogoAsync(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarJogoAsync([FromRoute] long id, [FromBody] JogoRequest request)
            => Ok(await _jogoService.AtualizarJogoAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverJogoAsync(long id)
        {
            await _jogoService.DeletarJogoAsync(id);
            return NoContent();
        }
    }
}
