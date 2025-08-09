using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController(ILibraryService service) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CriarBibliotecaAsync([FromBody] LibraryRequest request)
        {
            var result = await service.CreateLibraryAsync(request);
            return Created("api/Library", result);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ObterBibliotecasAsync()
        {
            var result = await service.GetLibrariesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterBibliotecaPorIdAsync(long id)
        {
            var result = await service.GetLibraryByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoverBibliotecaAsync(long id)
        {
            await service.DeleteLibraryAsync(id);
            return NoContent();

        }
    }
}
