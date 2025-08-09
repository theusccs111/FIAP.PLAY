using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [Route("api/[controller]")]
    public class GameLibraryController(IGameLibraryService service) : ControllerBase
    {
        [HttpPost("{libraryId}/games/{gameId}")]
        public async Task<IActionResult> AddGameToLibraryAsync(long libraryId, long gameId)
        {
            var result = await service.AddGameToLibraryAsync(libraryId, gameId);
            return Created($"api/GameLibrary/{libraryId}/games/{gameId}", result);
        }

        [HttpDelete("{libraryId}/games/{gameId}")]
        public async Task<IActionResult> RemoveGameFromLibraryAsync(long libraryId, long gameId)
        {
            await service.RemoveGameFromLibraryAsync(libraryId, gameId);
            return NoContent();
        }

        [HttpGet("{libraryId}/games")]
        public async Task<IActionResult> GetGamesByLibraryIdAsync(long libraryId)
        {
            var result = await service.GetGamesByLibraryIdAsync(libraryId);
            return Ok(result);
        }

        [HttpGet("{libraryId}/games/{gameId}")]
        public async Task<IActionResult> GetGameInLibraryAsync(long libraryId, long gameId)
        {
            var result = await service.GetGameInLibraryAsync(libraryId, gameId);
            return Ok(result);
        }        

    }
}
