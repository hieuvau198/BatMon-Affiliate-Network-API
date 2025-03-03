using Application.Contracts.Advertiser;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisersController : ControllerBase
    {
        private readonly IAdvertiserService _advertiserService;

        public AdvertisersController(IAdvertiserService advertiserService)
        {
            _advertiserService = advertiserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertiserDto>>> GetAllAdvertisers()
        {
            var advertisers = await _advertiserService.GetAllAdvertisersAsync();
            return Ok(advertisers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdvertiserDto>> GetAdvertiserById(int id, [FromQuery] bool includeRelated = false)
        {
            var advertiser = await _advertiserService.GetAdvertiserByIdAsync(id, includeRelated);
            if (advertiser == null)
            {
                return NotFound();
            }
            return Ok(advertiser);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<AdvertiserDto>> GetAdvertiserByEmail(string email)
        {
            var advertiser = await _advertiserService.GetAdvertiserByEmailAsync(email);
            if (advertiser == null)
            {
                return NotFound();
            }
            return Ok(advertiser);
        }

        [HttpPost]
        public async Task<ActionResult<AdvertiserDto>> CreateAdvertiser([FromBody] AdvertiserCreateDto advertiserDto)
        {
            var createdAdvertiser = await _advertiserService.CreateAdvertiserAsync(advertiserDto);
            return CreatedAtAction(nameof(GetAdvertiserById), new { id = createdAdvertiser.AdvertiserId }, createdAdvertiser);
        }

        [HttpPut]
        public async Task<ActionResult<AdvertiserDto>> UpdateAdvertiser([FromBody] AdvertiserUpdateDto advertiserDto)
        {
            var updatedAdvertiser = await _advertiserService.UpdateAdvertiserAsync(advertiserDto);
            return Ok(updatedAdvertiser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdvertiser(int id)
        {
            await _advertiserService.DeleteAdvertiserAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivateAdvertiser(int id)
        {
            var result = await _advertiserService.ActivateAdvertiserAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DeactivateAdvertiser(int id)
        {
            var result = await _advertiserService.DeactivateAdvertiserAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> AdvertiserExists(int id)
        {
            var exists = await _advertiserService.AdvertiserExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet("email/{email}/exists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _advertiserService.EmailExistsAsync(email);
            return Ok(exists);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetAdvertiserCount()
        {
            var count = await _advertiserService.GetAdvertiserCountAsync();
            return Ok(count);
        }
    }
}
