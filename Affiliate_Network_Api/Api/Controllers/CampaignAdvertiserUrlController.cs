using Application.Contracts.CampaignAdvertiserUrl;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignAdvertiserUrlController : ControllerBase
    {
        private readonly ICampaignAdvertiserUrlService _campaignAdvertiserUrlService;

        public CampaignAdvertiserUrlController(ICampaignAdvertiserUrlService campaignAdvertiserUrlService)
        {
            _campaignAdvertiserUrlService = campaignAdvertiserUrlService ?? throw new ArgumentNullException(nameof(campaignAdvertiserUrlService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignAdvertiserUrlDto>>> GetAllCampaignAdvertiserUrls()
        {
            try
            {
                var campaignAdvertiserUrls = await _campaignAdvertiserUrlService.GetAllCampaignAdvertiserUrlsAsync();
                return Ok(campaignAdvertiserUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving campaign advertiser URLs: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlById(int id, [FromQuery] bool includeRelated = false)
        {
            try
            {
                var campaignAdvertiserUrl = await _campaignAdvertiserUrlService.GetCampaignAdvertiserUrlByIdAsync(id, includeRelated);
                return Ok(campaignAdvertiserUrl);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Campaign advertiser URL with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the campaign advertiser URL: {ex.Message}");
            }
        }


        [HttpGet("advertiserUrl/{advertiserUrlId}")]
        public async Task<ActionResult<IEnumerable<CampaignAdvertiserUrlDto>>> GetCampaignAdvertiserUrlsByAdvertiserUrlId(int advertiserUrlId)
        {
            try
            {
                var campaignAdvertiserUrls = await _campaignAdvertiserUrlService.GetCampaignAdvertiserUrlsByAdvertiserUrlIdAsync(advertiserUrlId);
                return Ok(campaignAdvertiserUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving campaign advertiser URLs: {ex.Message}");
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<CampaignAdvertiserUrlDto>>> GetActiveCampaignAdvertiserUrls()
        {
            try
            {
                var campaignAdvertiserUrls = await _campaignAdvertiserUrlService.GetActiveCampaignAdvertiserUrlsAsync();
                return Ok(campaignAdvertiserUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving active campaign advertiser URLs: {ex.Message}");
            }
        }

        [HttpGet("status/{isActive}")]
        public async Task<ActionResult<IEnumerable<CampaignAdvertiserUrlDto>>> GetCampaignAdvertiserUrlsByStatus(bool isActive)
        {
            try
            {
                var campaignAdvertiserUrls = await _campaignAdvertiserUrlService.GetCampaignAdvertiserUrlsByStatusAsync(isActive);
                return Ok(campaignAdvertiserUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving campaign advertiser URLs by status: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCampaignAdvertiserUrlCount()
        {
            try
            {
                var count = await _campaignAdvertiserUrlService.GetCampaignAdvertiserUrlCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the count: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<CampaignAdvertiserUrlDto>> CreateCampaignAdvertiserUrl([FromBody] CampaignAdvertiserUrlCreateDto campaignAdvertiserUrlDto)
        {
            try
            {
                var createdCampaignAdvertiserUrl = await _campaignAdvertiserUrlService.CreateCampaignAdvertiserUrlAsync(campaignAdvertiserUrlDto);
                return CreatedAtAction(nameof(GetCampaignAdvertiserUrlById), new { id = createdCampaignAdvertiserUrl.CampaignUrlId }, createdCampaignAdvertiserUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the campaign advertiser URL: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<CampaignAdvertiserUrlDto>> UpdateCampaignAdvertiserUrl([FromBody] CampaignAdvertiserUrlUpdateDto campaignAdvertiserUrlDto)
        {
            try
            {
                var updatedCampaignAdvertiserUrl = await _campaignAdvertiserUrlService.UpdateCampaignAdvertiserUrlAsync(campaignAdvertiserUrlDto);
                return Ok(updatedCampaignAdvertiserUrl);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Campaign advertiser URL with ID {campaignAdvertiserUrlDto.CampaignUrlId} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the campaign advertiser URL: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaignAdvertiserUrl(int id)
        {
            try
            {
                await _campaignAdvertiserUrlService.DeleteCampaignAdvertiserUrlAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Campaign advertiser URL with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the campaign advertiser URL: {ex.Message}");
            }
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<bool>> ActivateCampaignAdvertiserUrl(int id)
        {
            try
            {
                var result = await _campaignAdvertiserUrlService.ActivateCampaignAdvertiserUrlAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Campaign advertiser URL with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while activating the campaign advertiser URL: {ex.Message}");
            }
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivateCampaignAdvertiserUrl(int id)
        {
            try
            {
                var result = await _campaignAdvertiserUrlService.DeactivateCampaignAdvertiserUrlAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Campaign advertiser URL with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deactivating the campaign advertiser URL: {ex.Message}");
            }
        }
    }
}