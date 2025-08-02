using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(IGameService _gameService) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterJogosAsync()
            => Ok(await _gameService.GetGamesAsync());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterJogoPorIdAsync(int id)
            => Ok(await _gameService.GetGameByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CriarJogoAsync([FromBody] GameRequest request)
            => Created("api/Jogo", await _gameService.CreateGameAsync(request));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> AtualizarJogoAsync([FromRoute] long id, [FromBody] GameRequest request)
            => Ok(await _gameService.UpdateGameAsync(id, request));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> RemoverJogoAsync(long id)
        {
            await _gameService.DeleteGameAsync(id);
            return NoContent();
        }
    }
}
