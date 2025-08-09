using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [Route("api/[controller]")]
    public class GameLibraryController(IGameLibraryService service) : ControllerBase
    {
        [HttpPost("{libraryId}/games/{gameId}")]
        [Authorize]
        public async Task<IActionResult> AddGameToLibraryAsync(long libraryId, long gameId)
        {
            var result = await service.AddGameToLibraryAsync(libraryId, gameId);
            return Created($"api/GameLibrary/{libraryId}/games/{gameId}", result);
        }

        [HttpDelete("{libraryId}/games/{gameId}")]
        [Authorize]
        public async Task<IActionResult> RemoveGameFromLibraryAsync(long libraryId, long gameId)
        {
            await service.RemoveGameFromLibraryAsync(libraryId, gameId);
            return NoContent();
        }

        [HttpGet("{libraryId}/games")]
        [Authorize]
        public async Task<IActionResult> GetGamesByLibraryIdAsync(long libraryId)
        {
            var result = await service.GetGamesByLibraryIdAsync(libraryId);
            return Ok(result);
        }

        [HttpGet("{libraryId}/games/{gameId}")]
        [Authorize]
        public async Task<IActionResult> GetGameInLibraryAsync(long libraryId, long gameId)
        {
            var result = await service.GetGameInLibraryAsync(libraryId, gameId);
            return Ok(result);
        }

    }
}
