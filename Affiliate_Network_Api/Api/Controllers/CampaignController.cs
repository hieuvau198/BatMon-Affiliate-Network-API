using Application.Contracts.Campaign;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService ?? throw new ArgumentNullException(nameof(campaignService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetAllCampaigns()
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync();
            return Ok(campaigns);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignDto>> GetCampaignById(int id, [FromQuery] bool includeRelated = false)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id, includeRelated);
            return Ok(campaign);
        }

        [HttpGet("advertiser/{advertiserId}")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaignsByAdvertiserId(int advertiserId)
        {
            var campaigns = await _campaignService.GetCampaignsByAdvertiserIdAsync(advertiserId);
            return Ok(campaigns);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetActiveCampaigns()
        {
            var campaigns = await _campaignService.GetActiveCampaignsAsync();
            return Ok(campaigns);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaignsByStatus(string status)
        {
            var campaigns = await _campaignService.GetCampaignsByStatusAsync(status);
            return Ok(campaigns);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCampaignCount()
        {
            var count = await _campaignService.GetCampaignCountAsync();
            return Ok(count);
        }

        [HttpGet("count/advertiser/{advertiserId}")]
        public async Task<ActionResult<int>> GetCampaignCountByAdvertiser(int advertiserId)
        {
            var count = await _campaignService.GetCampaignCountByAdvertiserAsync(advertiserId);
            return Ok(count);
        }

        [HttpGet("budget/advertiser/{advertiserId}")]
        public async Task<ActionResult<decimal>> GetTotalBudgetByAdvertiser(int advertiserId)
        {
            var budget = await _campaignService.GetTotalBudgetByAdvertiserAsync(advertiserId);
            return Ok(budget);
        }

        [HttpPost]
        public async Task<ActionResult<CampaignDto>> CreateCampaign([FromBody] CampaignCreateDto campaignDto)
        {
            var createdCampaign = await _campaignService.CreateCampaignAsync(campaignDto);
            return CreatedAtAction(nameof(GetCampaignById), new { id = createdCampaign.CampaignId }, createdCampaign);
        }

        [HttpPut]
        public async Task<ActionResult<CampaignDto>> UpdateCampaign([FromBody] CampaignUpdateDto campaignDto)
        {
            var updatedCampaign = await _campaignService.UpdateCampaignAsync(campaignDto);
            return Ok(updatedCampaign);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaign(int id)
        {
            await _campaignService.DeleteCampaignAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<bool>> ActivateCampaign(int id)
        {
            var result = await _campaignService.ActivateCampaignAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivateCampaign(int id)
        {
            var result = await _campaignService.DeactivateCampaignAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> CampaignExists(int id)
        {
            var exists = await _campaignService.CampaignExistsAsync(id);
            return Ok(exists);
        }
    }
}
