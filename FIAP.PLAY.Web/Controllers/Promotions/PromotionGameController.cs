using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.PromotionGames
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionGameController : ControllerBase
    {
        private readonly IPromotionGameService _promotionGameService;
        public PromotionGameController(IPromotionGameService promotionGameService)
        {
            _promotionGameService = promotionGameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionGamesAsync()
        {
            var resultado = await _promotionGameService.GetPromotionGamesAsync();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPromotionGameByIdAsync(int Id)
        {
            var resultado = await _promotionGameService.GetPromotionGameByIdAsync(Id);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreatePromotionGameAsync([FromBody] PromotionGameRequest request)
        {
            var dados = await _promotionGameService.CreatePromotionGameAsync(request);
            return Created("api/PromotionGame", dados);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePromotionGameAsync([FromRoute] long id, [FromBody] PromotionGameRequest request)
        {
            var dados = await _promotionGameService.UpdatePromotionGameAsync(id, request);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeletePromotionGameAsync(long id)
        {
            await _promotionGameService.DeletePromotionGameAsync(id);
            return NoContent();
        }
    }
}
