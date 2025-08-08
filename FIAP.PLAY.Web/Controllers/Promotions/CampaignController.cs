using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.PLAY.Web.Controllers.Promotions
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaignsAsync()
        {
            var resultado = await _campaignService.GetCampaignsAsync();
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCampaignByIdAsync(int Id)
        {
            var resultado = await _campaignService.GetCampaignByIdAsync(Id);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] CampaignRequest request)
        {
            var dados = await _campaignService.CreateCampaignAsync(request);
            return Created("api/Campaign", dados);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCampaignAsync([FromRoute] long id, [FromBody] CampaignRequest request)
        {
            var dados = await _campaignService.UpdateCampaignAsync(id, request);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeleteCampaignAsync(long id)
        {
            await _campaignService.DeleteCampaignAsync(id);
            return NoContent();
        }
    }
}
