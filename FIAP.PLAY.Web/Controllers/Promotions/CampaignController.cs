using FIAP.PLAY.Application.Promotions.Interfaces;
using FIAP.PLAY.Application.Promotions.Resources.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

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
        public async Task<IActionResult> GetCampaignsAsync(CancellationToken cancellationToken)
        {
            var resultado = await _campaignService.GetCampaignsAsync(cancellationToken);
            return Ok(resultado);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCampaignByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var resultado = await _campaignService.GetCampaignByIdAsync(Id, cancellationToken);
            return Ok(resultado);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] CampaignRequest request, CancellationToken cancellationToken)
        {
            var dados = await _campaignService.CreateCampaignAsync(request, cancellationToken);
            return Created("api/Campaign", dados);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCampaignAsync([FromRoute] long id, [FromBody] CampaignRequest request, CancellationToken cancellationToken)
        {
            var dados = await _campaignService.UpdateCampaignAsync(id, request, cancellationToken);
            return Ok(dados);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> DeleteCampaignAsync(long id, CancellationToken cancellationToken)
        {
            await _campaignService.DeleteCampaignAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
