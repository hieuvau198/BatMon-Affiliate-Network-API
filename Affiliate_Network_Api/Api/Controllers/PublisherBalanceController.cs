// Api/Controllers/PublisherBalanceController.cs
using Application.Contracts.PublisherBalance;
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
    public class PublisherBalanceController : ControllerBase
    {
        private readonly IPublisherBalanceService _publisherBalanceService;

        public PublisherBalanceController(IPublisherBalanceService publisherBalanceService)
        {
            _publisherBalanceService = publisherBalanceService ?? throw new ArgumentNullException(nameof(publisherBalanceService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherBalanceDto>>> GetAllPublisherBalances()
        {
            var balances = await _publisherBalanceService.GetAllPublisherBalancesAsync();
            return Ok(balances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherBalanceDto>> GetPublisherBalanceById(int id)
        {
            var balance = await _publisherBalanceService.GetPublisherBalanceByIdAsync(id);
            return Ok(balance);
        }

        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<PublisherBalanceDto>> GetPublisherBalanceByPublisherId(int publisherId)
        {
            var balance = await _publisherBalanceService.GetPublisherBalanceByPublisherIdAsync(publisherId);
            return Ok(balance);
        }

        [HttpPost]
        public async Task<ActionResult<PublisherBalanceDto>> CreatePublisherBalance([FromBody] PublisherBalanceCreateDto publisherBalanceDto)
        {
            try
            {
                var createdBalance = await _publisherBalanceService.CreatePublisherBalanceAsync(publisherBalanceDto);
                return CreatedAtAction(nameof(GetPublisherBalanceById), new { id = createdBalance.BalanceId }, createdBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<PublisherBalanceDto>> UpdatePublisherBalance([FromBody] PublisherBalanceUpdateDto publisherBalanceDto)
        {
            try
            {
                var updatedBalance = await _publisherBalanceService.UpdatePublisherBalanceAsync(publisherBalanceDto);
                return Ok(updatedBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublisherBalance(int id)
        {
            try
            {
                await _publisherBalanceService.DeletePublisherBalanceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PublisherBalanceExists(int id)
        {
            var exists = await _publisherBalanceService.PublisherBalanceExistsAsync(id);
            return Ok(exists);
        }
    }
}