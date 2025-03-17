using Application.Contracts.AdvertiserUrl;
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
    public class AdvertiserUrlController : ControllerBase
    {
        private readonly IAdvertiserUrlService _advertiserUrlService;

        public AdvertiserUrlController(IAdvertiserUrlService advertiserUrlService)
        {
            _advertiserUrlService = advertiserUrlService ?? throw new ArgumentNullException(nameof(advertiserUrlService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertiserUrlDto>>> GetAllAdvertiserUrls()
        {
            var advertiserUrls = await _advertiserUrlService.GetAllAdvertiserUrlsAsync();
            return Ok(advertiserUrls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdvertiserUrlDto>> GetAdvertiserUrlById(int id, [FromQuery] bool includeRelated = false)
        {
            var advertiserUrl = await _advertiserUrlService.GetAdvertiserUrlByIdAsync(id, includeRelated);
            return Ok(advertiserUrl);
        }

        [HttpGet("advertiser/{advertiserId}")]
        public async Task<ActionResult<IEnumerable<AdvertiserUrlDto>>> GetAdvertiserUrlsByAdvertiserId(int advertiserId)
        {
            var advertiserUrls = await _advertiserUrlService.GetAdvertiserUrlsByAdvertiserIdAsync(advertiserId);
            return Ok(advertiserUrls);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<AdvertiserUrlDto>>> GetActiveAdvertiserUrls()
        {
            var advertiserUrls = await _advertiserUrlService.GetActiveAdvertiserUrlsAsync();
            return Ok(advertiserUrls);
        }

        [HttpGet("status/{isActive}")]
        public async Task<ActionResult<IEnumerable<AdvertiserUrlDto>>> GetAdvertiserUrlsByStatus(bool isActive)
        {
            var advertiserUrls = await _advertiserUrlService.GetAdvertiserUrlsByStatusAsync(isActive);
            return Ok(advertiserUrls);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetAdvertiserUrlCount()
        {
            var count = await _advertiserUrlService.GetAdvertiserUrlCountAsync();
            return Ok(count);
        }

        [HttpGet("count/advertiser/{advertiserId}")]
        public async Task<ActionResult<int>> GetAdvertiserUrlCountByAdvertiser(int advertiserId)
        {
            var count = await _advertiserUrlService.GetAdvertiserUrlCountByAdvertiserAsync(advertiserId);
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<AdvertiserUrlDto>> CreateAdvertiserUrl([FromBody] AdvertiserUrlCreateDto advertiserUrlDto)
        {
            var createdAdvertiserUrl = await _advertiserUrlService.CreateAdvertiserUrlAsync(advertiserUrlDto);
            return CreatedAtAction(nameof(GetAdvertiserUrlById), new { id = createdAdvertiserUrl.UrlId }, createdAdvertiserUrl);
        }

        [HttpPut]
        public async Task<ActionResult<AdvertiserUrlDto>> UpdateAdvertiserUrl([FromBody] AdvertiserUrlUpdateDto advertiserUrlDto)
        {
            var updatedAdvertiserUrl = await _advertiserUrlService.UpdateAdvertiserUrlAsync(advertiserUrlDto);
            return Ok(updatedAdvertiserUrl);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdvertiserUrl(int id)
        {
            await _advertiserUrlService.DeleteAdvertiserUrlAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<bool>> ActivateAdvertiserUrl(int id)
        {
            var result = await _advertiserUrlService.ActivateAdvertiserUrlAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivateAdvertiserUrl(int id)
        {
            var result = await _advertiserUrlService.DeactivateAdvertiserUrlAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> AdvertiserUrlExists(int id)
        {
            var exists = await _advertiserUrlService.AdvertiserUrlExistsAsync(id);
            return Ok(exists);
        }
    }
}