using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.UserAccess
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken)
        {
            var resultado = await _userService.GetUsersAsync(cancellationToken);
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var resultado = await _userService.GetUserByIdAsync(Id, cancellationToken);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            var dados = await _userService.CreateUserAsync(request, cancellationToken);
            return Created("api/User", dados);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] long id, [FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            var dados = await _userService.UpdateUserAsync(id, request, cancellationToken);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
