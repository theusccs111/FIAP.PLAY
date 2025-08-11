using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Promotions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionGameController : ControllerBase
    {
        private readonly IPromotionGameService _promotionGameService;
        public PromotionGameController(IPromotionGameService promotionGameService, CancellationToken cancellationToken)
        {
            _promotionGameService = promotionGameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionGamesAsync(CancellationToken cancellationToken)
        {
            var resultado = await _promotionGameService.GetPromotionGamesAsync(cancellationToken);
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPromotionGameByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var resultado = await _promotionGameService.GetPromotionGameByIdAsync(Id, cancellationToken);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreatePromotionGameAsync([FromBody] PromotionGameRequest request, CancellationToken cancellationToken)
        {
            var dados = await _promotionGameService.CreatePromotionGameAsync(request, cancellationToken);
            return Created("api/PromotionGame", dados);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePromotionGameAsync([FromRoute] long id, [FromBody] PromotionGameRequest request, CancellationToken cancellationToken)
        {
            var dados = await _promotionGameService.UpdatePromotionGameAsync(id, request, cancellationToken);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeletePromotionGameAsync(long id, CancellationToken cancellationToken)
        {
            await _promotionGameService.DeletePromotionGameAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
