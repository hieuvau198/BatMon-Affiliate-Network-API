// AdvertiserBalanceController.cs
using Application.Contracts.AdvertiserBalance;
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
    public class AdvertiserBalanceController : ControllerBase
    {
        private readonly IAdvertiserBalanceService _advertiserBalanceService;

        public AdvertiserBalanceController(IAdvertiserBalanceService advertiserBalanceService)
        {
            _advertiserBalanceService = advertiserBalanceService ?? throw new ArgumentNullException(nameof(advertiserBalanceService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvertiserBalanceDto>>> GetAllAdvertiserBalances()
        {
            var balances = await _advertiserBalanceService.GetAllAdvertiserBalancesAsync();
            return Ok(balances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdvertiserBalanceDto>> GetAdvertiserBalanceById(int id)
        {
            try
            {
                var balance = await _advertiserBalanceService.GetAdvertiserBalanceByIdAsync(id);
                return Ok(balance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("advertiser/{advertiserId}")]
        public async Task<ActionResult<IEnumerable<AdvertiserBalanceDto>>> GetAdvertiserBalancesByAdvertiserId(int advertiserId)
        {
            var balances = await _advertiserBalanceService.GetAdvertiserBalancesByAdvertiserIdAsync(advertiserId);
            return Ok(balances);
        }

        [HttpPost]
        public async Task<ActionResult<AdvertiserBalanceDto>> CreateAdvertiserBalance([FromBody] AdvertiserBalanceCreateDto balanceDto)
        {
            try
            {
                var createdBalance = await _advertiserBalanceService.CreateAdvertiserBalanceAsync(balanceDto);
                return CreatedAtAction(nameof(GetAdvertiserBalanceById), new { id = createdBalance.BalanceId }, createdBalance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<AdvertiserBalanceDto>> UpdateAdvertiserBalance([FromBody] AdvertiserBalanceUpdateDto balanceDto)
        {
            try
            {
                var updatedBalance = await _advertiserBalanceService.UpdateAdvertiserBalanceAsync(balanceDto);
                return Ok(updatedBalance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdvertiserBalance(int id)
        {
            try
            {
                await _advertiserBalanceService.DeleteAdvertiserBalanceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> AdvertiserBalanceExists(int id)
        {
            var exists = await _advertiserBalanceService.AdvertiserBalanceExistsAsync(id);
            return Ok(exists);
        }
    }
}