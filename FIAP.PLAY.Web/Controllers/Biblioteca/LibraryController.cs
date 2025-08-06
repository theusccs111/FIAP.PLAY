using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Biblioteca
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController(ILibraryService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CriarBibliotecaAsync([FromBody] LibraryRequest request)
        {
            var result = await service.CreateLibraryAsync(request);
            return Created("api/Library", result);
        }

        [HttpGet]
        public async Task<IActionResult> ObterBibliotecasAsync()
        {
            var result = await service.GetLibrariesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterBibliotecaPorIdAsync(long id)
        {
            var result = await service.GetLibraryByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverBibliotecaAsync(long id)
        {
            await service.DeleteLibraryAsync(id);
            return NoContent();

        }
    }
}
