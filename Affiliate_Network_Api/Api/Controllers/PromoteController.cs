using Microsoft.AspNetCore.Http;
using Application.Contracts.Promote;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoteController : ControllerBase
    {
        private readonly IPromoteService _promoteService;

        public PromoteController(IPromoteService promoteService)
        {
            _promoteService = promoteService ?? throw new ArgumentNullException(nameof(promoteService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoteDto>>> GetAllPromotes()
        {
            var promotes = await _promoteService.GetAllPromotesAsync();
            return Ok(promotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromoteDto>> GetPromoteById(int id, [FromQuery] bool includeRelated = false)
        {
            try
            {
                var promote = await _promoteService.GetPromoteByIdAsync(id, includeRelated);
                return Ok(promote);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<PromoteDto>>> GetPromotesByPublisherId(int publisherId)
        {
            try
            {
                var promotes = await _promoteService.GetPromotesByPublisherIdAsync(publisherId);
                return Ok(promotes);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("campaign/{campaignId}")]
        public async Task<ActionResult<IEnumerable<PromoteDto>>> GetPromotesByCampaignId(int campaignId)
        {
            try
            {
                var promotes = await _promoteService.GetPromotesByCampaignIdAsync(campaignId);
                return Ok(promotes);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PromoteDto>> CreatePromote([FromBody] PromoteCreateDto promoteDto)
        {
            try
            {
                var createdPromote = await _promoteService.CreatePromoteAsync(promoteDto);
                return CreatedAtAction(nameof(GetPromoteById), new { id = createdPromote.PromoteId }, createdPromote);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<PromoteDto>> UpdatePromote([FromBody] PromoteUpdateDto promoteDto)
        {
            try
            {
                var updatedPromote = await _promoteService.UpdatePromoteAsync(promoteDto);
                return Ok(updatedPromote);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromote(int id)
        {
            try
            {
                await _promoteService.DeletePromoteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/approve")]
        public async Task<ActionResult> ApprovePromote(int id)
        {
            try
            {
                var result = await _promoteService.ApprovePromoteAsync(id);
                return Ok(new { success = result, message = "Promote approved successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/reject")]
        public async Task<ActionResult> RejectPromote(int id)
        {
            try
            {
                var result = await _promoteService.RejectPromoteAsync(id);
                return Ok(new { success = result, message = "Promote rejected successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPromoteCount()
        {
            var count = await _promoteService.GetPromoteCountAsync();
            return Ok(count);
        }

        [HttpGet("count/publisher/{publisherId}")]
        public async Task<ActionResult<int>> GetPromoteCountByPublisher(int publisherId)
        {
            var count = await _promoteService.GetPromoteCountByPublisherAsync(publisherId);
            return Ok(count);
        }

        [HttpGet("count/campaign/{campaignId}")]
        public async Task<ActionResult<int>> GetPromoteCountByCampaign(int campaignId)
        {
            var count = await _promoteService.GetPromoteCountByCampaignAsync(campaignId);
            return Ok(count);
        }
    }
}