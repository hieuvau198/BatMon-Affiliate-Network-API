using Application.Contracts.CampaignPublisherCommission;
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
    public class CampaignPublisherCommissionController : ControllerBase
    {
        private readonly ICampaignPublisherCommissionService _commissionService;

        public CampaignPublisherCommissionController(ICampaignPublisherCommissionService commissionService)
        {
            _commissionService = commissionService ?? throw new ArgumentNullException(nameof(commissionService));
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<CampaignPublisherCommissionDto>>> GetAllCommissions()
        {
            var commissions = await _commissionService.GetAllCommissionsAsync();
            return Ok(commissions);
        }

        [HttpGet("{id}")]
     
        public async Task<ActionResult<CampaignPublisherCommissionDto>> GetCommissionById(int id)
        {
            try
            {
                var commission = await _commissionService.GetCommissionByIdAsync(id);
                return Ok(commission);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<CampaignPublisherCommissionDto>>> GetCommissionsByPublisherId(int publisherId)
        {
            try
            {
                var commissions = await _commissionService.GetCommissionsByPublisherIdAsync(publisherId);
                return Ok(commissions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("campaign/{campaignId}")]
        public async Task<ActionResult<IEnumerable<CampaignPublisherCommissionDto>>> GetCommissionsByCampaignId(int campaignId)
        {
            try
            {
                var commissions = await _commissionService.GetCommissionsByCampaignIdAsync(campaignId);
                return Ok(commissions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampaignPublisherCommissionDto>> CreateCommission([FromBody] CampaignPublisherCommissionCreateDto commissionDto)
        {
            try
            {
                var createdCommission = await _commissionService.CreateCommissionAsync(commissionDto);
                return CreatedAtAction(nameof(GetCommissionById), new { id = createdCommission.CommissionId }, createdCommission);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<CampaignPublisherCommissionDto>> UpdateCommission([FromBody] CampaignPublisherCommissionUpdateDto commissionDto)
        {
            try
            {
                var updatedCommission = await _commissionService.UpdateCommissionAsync(commissionDto);
                return Ok(updatedCommission);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status/{status}")]
        public async Task<ActionResult<CampaignPublisherCommissionDto>> UpdateCommissionStatus(int id, string status)
        {
            try
            {
                var updatedCommission = await _commissionService.UpdateCommissionStatusAsync(id, status);
                return Ok(updatedCommission);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommission(int id)
        {
            try
            {
                await _commissionService.DeleteCommissionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> CommissionExists(int id)
        {
            var exists = await _commissionService.CommissionExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet("publisher/{publisherId}/pending/{currencyCode}")]
   
        public async Task<ActionResult<decimal>> GetTotalPendingCommissionsByPublisher(int publisherId, string currencyCode)
        {
            try
            {
                var totalPending = await _commissionService.GetTotalPendingCommissionsByPublisherAsync(publisherId, currencyCode);
                return Ok(totalPending);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("publisher/{publisherId}/approved/{currencyCode}")]
        public async Task<ActionResult<decimal>> GetTotalApprovedCommissionsByPublisher(int publisherId, string currencyCode)
        {
            try
            {
                var totalApproved = await _commissionService.GetTotalApprovedCommissionsByPublisherAsync(publisherId, currencyCode);
                return Ok(totalApproved);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}