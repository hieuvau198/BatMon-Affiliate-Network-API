using Application.Contracts.Publisher;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAllPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisherById(int id, [FromQuery] bool includeRelated = false)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id, includeRelated);
            return Ok(publisher);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<PublisherDto>> GetPublisherByEmail(string email)
        {
            var publisher = await _publisherService.GetPublisherByEmailAsync(email);
            return Ok(publisher);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<PublisherDto>> GetPublisherByUsername(string username)
        {
            var publisher = await _publisherService.GetPublisherByUsernameAsync(username);
            return Ok(publisher);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPublisherCount()
        {
            var count = await _publisherService.GetPublisherCountAsync();
            return Ok(count);
        }

        [HttpGet("referrals/{referralCode}")]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishersByReferrerCode(string referralCode)
        {
            var publishers = await _publisherService.GetPublishersByReferrerCodeAsync(referralCode);
            return Ok(publishers);
        }

        [HttpGet("referrals/count/{referralCode}")]
        public async Task<ActionResult<int>> GetReferralCount(string referralCode)
        {
            var count = await _publisherService.GetReferralCountAsync(referralCode);
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] PublisherCreateDto publisherDto)
        {
            var createdPublisher = await _publisherService.CreatePublisherAsync(publisherDto);
            return CreatedAtAction(nameof(GetPublisherById), new { id = createdPublisher.PublisherId }, createdPublisher);
        }

        [HttpPut]
        public async Task<ActionResult<PublisherDto>> UpdatePublisher([FromBody] PublisherUpdateDto publisherDto)
        {
            var updatedPublisher = await _publisherService.UpdatePublisherAsync(publisherDto);
            return Ok(updatedPublisher);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublisher(int id)
        {
            await _publisherService.DeletePublisherAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<bool>> ActivatePublisher(int id)
        {
            var result = await _publisherService.ActivatePublisherAsync(id);
            return Ok(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivatePublisher(int id)
        {
            var result = await _publisherService.DeactivatePublisherAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PublisherExists(int id)
        {
            var exists = await _publisherService.PublisherExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet("email/exists/{email}")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _publisherService.EmailExistsAsync(email);
            return Ok(exists);
        }

        [HttpGet("username/exists/{username}")]
        public async Task<ActionResult<bool>> UsernameExists(string username)
        {
            var exists = await _publisherService.UsernameExistsAsync(username);
            return Ok(exists);
        }
    }
}
