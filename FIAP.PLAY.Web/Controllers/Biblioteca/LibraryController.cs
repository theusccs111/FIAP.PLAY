using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController(ILibraryService service) : ControllerBase
    {

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateLibraryAsync([FromBody] LibraryRequest request, CancellationToken cancellationToken)
        {
            var result = await service.CreateLibraryAsync(request, cancellationToken);
            return Created("api/Library", result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetLibrariesAsync(CancellationToken cancellationToken)
        {
            var result = await service.GetLibrariesAsync(cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetLibraryByIdAsync(long id, CancellationToken cancellationToken)
        {
            var result = await service.GetLibraryByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("/getUser")]
        public async Task<IActionResult> GetLibraryByUserIdAsync(CancellationToken cancellationToken)
        {           
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userId, out long id))
            {
                return BadRequest("Id de usuário inválido.");
            }

            var result = await service.GetLibraryByUserIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLibraryAsync(long id, CancellationToken cancellationToken)
        {
            await service.DeleteLibraryAsync(id, cancellationToken);
            return NoContent();

        }
    }
}
