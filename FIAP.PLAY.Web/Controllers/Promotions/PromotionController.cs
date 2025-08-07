using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Promotions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotionsAsync()
        {
            var resultado = await _promotionService.GetPromotionsAsync();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPromotionByIdAsync(int Id)
        {
            var resultado = await _promotionService.GetPromotionByIdAsync(Id);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreatePromotionAsync([FromBody] PromotionRequest request)
        {
            var dados = await _promotionService.CreatePromotionAsync(request);
            return Created("api/Promotion", dados);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePromotionAsync([FromRoute] long id, [FromBody] PromotionRequest request)
        {
            var dados = await _promotionService.UpdatePromotionAsync(id, request);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeletePromotionAsync(long id)
        {
            await _promotionService.DeletePromotionAsync(id);
            return NoContent();
        }
    }
}
