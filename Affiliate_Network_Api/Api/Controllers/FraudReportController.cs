using Application.Contracts.FraudReport;
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
    public class FraudReportController : ControllerBase
    {
        private readonly IFraudReportService _fraudReportService;

        public FraudReportController(IFraudReportService fraudReportService)
        {
            _fraudReportService = fraudReportService ?? throw new ArgumentNullException(nameof(fraudReportService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetAllFraudReports()
        {
            var fraudReports = await _fraudReportService.GetAllFraudReportsAsync();
            return Ok(fraudReports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FraudReportDto>> GetFraudReportById(int id, [FromQuery] bool includeRelated = false)
        {
            var fraudReport = await _fraudReportService.GetFraudReportByIdAsync(id, includeRelated);
            return Ok(fraudReport);
        }

        [HttpGet("campaign/{campaignId}")]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetFraudReportsByCampaignId(int campaignId)
        {
            var fraudReports = await _fraudReportService.GetFraudReportsByCampaignIdAsync(campaignId);
            return Ok(fraudReports);
        }

        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetFraudReportsByPublisherId(int publisherId)
        {
            var fraudReports = await _fraudReportService.GetFraudReportsByPublisherIdAsync(publisherId);
            return Ok(fraudReports);
        }

        [HttpGet("advertiser/{advertiserId}")]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetFraudReportsByAdvertiserId(int advertiserId)
        {
            var fraudReports = await _fraudReportService.GetFraudReportsByAdvertiserIdAsync(advertiserId);
            return Ok(fraudReports);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetFraudReportsByStatus(string status)
        {
            var fraudReports = await _fraudReportService.GetFraudReportsByStatusAsync(status);
            return Ok(fraudReports);
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<FraudReportDto>>> GetUnreadFraudReports()
        {
            var fraudReports = await _fraudReportService.GetUnreadFraudReportsAsync();
            return Ok(fraudReports);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetFraudReportCount()
        {
            var count = await _fraudReportService.GetFraudReportCountAsync();
            return Ok(count);
        }

        [HttpGet("count/unread")]
        public async Task<ActionResult<int>> GetUnreadFraudReportCount()
        {
            var count = await _fraudReportService.GetUnreadFraudReportCountAsync();
            return Ok(count);
        }

        [HttpGet("financial-impact")]
        public async Task<ActionResult<decimal>> GetTotalFinancialImpact([FromQuery] int? advertiserId = null, [FromQuery] int? publisherId = null)
        {
            var impact = await _fraudReportService.GetTotalFinancialImpactAsync(advertiserId, publisherId);
            return Ok(impact);
        }

        [HttpPost]
        public async Task<ActionResult<FraudReportDto>> CreateFraudReport([FromBody] FraudReportCreateDto fraudReportDto)
        {
            try
            {
                var createdFraudReport = await _fraudReportService.CreateFraudReportAsync(fraudReportDto);
                return CreatedAtAction(nameof(GetFraudReportById), new { id = createdFraudReport.ReportId }, createdFraudReport);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create fraud report: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<FraudReportDto>> UpdateFraudReport([FromBody] FraudReportUpdateDto fraudReportDto)
        {
            try
            {
                var updatedFraudReport = await _fraudReportService.UpdateFraudReportAsync(fraudReportDto);
                return Ok(updatedFraudReport);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update fraud report: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFraudReport(int id)
        {
            try
            {
                await _fraudReportService.DeleteFraudReportAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete fraud report: {ex.Message}");
            }
        }

        [HttpPatch("{id}/mark-read")]
        public async Task<ActionResult<bool>> MarkFraudReportAsRead(int id)
        {
            try
            {
                var result = await _fraudReportService.MarkFraudReportAsReadAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to mark fraud report as read: {ex.Message}");
            }
        }

        [HttpPatch("{id}/mark-unread")]
        public async Task<ActionResult<bool>> MarkFraudReportAsUnread(int id)
        {
            try
            {
                var result = await _fraudReportService.MarkFraudReportAsUnreadAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to mark fraud report as unread: {ex.Message}");
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> FraudReportExists(int id)
        {
            var exists = await _fraudReportService.FraudReportExistsAsync(id);
            return Ok(exists);
        }
    }
}