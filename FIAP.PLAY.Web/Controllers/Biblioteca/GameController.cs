using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
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
        public async Task<IActionResult> ObterJogosAsync(CancellationToken cancellationToken)
            => Ok(await _gameService.GetGamesAsync(cancellationToken));

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterJogoPorIdAsync(int id, CancellationToken cancellationToken)
            => Ok(await _gameService.GetGameByIdAsync(id, cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CriarJogoAsync([FromBody] GameRequest request, CancellationToken cancellationToken)
            => Created("api/Jogo", await _gameService.CreateGameAsync(request, cancellationToken));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> AtualizarJogoAsync([FromRoute] long id, [FromBody] GameRequest request, CancellationToken cancellationToken)
            => Ok(await _gameService.UpdateGameAsync(id, request, cancellationToken));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> RemoverJogoAsync(long id, CancellationToken cancellationToken)
        {
            await _gameService.DeleteGameAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
