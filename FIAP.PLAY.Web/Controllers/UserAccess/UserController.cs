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
        public async Task<IActionResult> GetUsersAsync()
        {
            var resultado = await _userService.GetUsersAsync();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(int Id)
        {
            var resultado = await _userService.GetUserByIdAsync(Id);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request)
        {
            var dados = await _userService.CreateUserAsync(request);
            return Created("api/User", dados);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] long id, [FromBody] UserRequest request)
        {
            var dados = await _userService.UpdateUserAsync(id, request);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeleteUserAsync(long id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
