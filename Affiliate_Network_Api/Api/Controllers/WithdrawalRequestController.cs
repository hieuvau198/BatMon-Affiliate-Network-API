using Application.Contracts.WithdrawalRequest;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WithdrawalRequestController : ControllerBase
    {
        private readonly IWithdrawalRequestService _withdrawalRequestService;

        public WithdrawalRequestController(IWithdrawalRequestService withdrawalRequestService)
        {
            _withdrawalRequestService = withdrawalRequestService ?? throw new ArgumentNullException(nameof(withdrawalRequestService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WithdrawalRequestDto>>> GetAllWithdrawalRequests()
        {
            var withdrawalRequests = await _withdrawalRequestService.GetAllWithdrawalRequestsAsync();
            return Ok(withdrawalRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WithdrawalRequestDto>> GetWithdrawalRequestById(int id)
        {
            try
            {
                var withdrawalRequest = await _withdrawalRequestService.GetWithdrawalRequestByIdAsync(id);
                return Ok(withdrawalRequest);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("advertiser/{advertiserId}")]
        public async Task<ActionResult<IEnumerable<WithdrawalRequestDto>>> GetWithdrawalRequestsByAdvertiserId(int advertiserId)
        {
            try
            {
                var withdrawalRequests = await _withdrawalRequestService.GetWithdrawalRequestsByAdvertiserIdAsync(advertiserId);
                return Ok(withdrawalRequests);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<WithdrawalRequestDto>> CreateWithdrawalRequest(CreateWithdrawalRequestDto createDto)
        {
            try
            {
                var withdrawalRequest = await _withdrawalRequestService.CreateWithdrawalRequestAsync(createDto);
                return CreatedAtAction(nameof(GetWithdrawalRequestById), new { id = withdrawalRequest.RequestId }, withdrawalRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Admin tìm kiếm và lọc yêu cầu rút tiền
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<WithdrawalRequestDto>>> GetFilteredWithdrawalRequests(WithdrawalRequestFilterDto filterDto)
        {
            var withdrawalRequests = await _withdrawalRequestService.GetFilteredWithdrawalRequestsAsync(filterDto);
            return Ok(withdrawalRequests);
        }

        //Admin phê duyệt yêu cầu
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveWithdrawalRequest(int id)
        {
            try
            {
                int adminId = 0; // Default value since we're not getting it from User claims anymore
                await _withdrawalRequestService.ApproveWithdrawalRequestAsync(id, adminId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Nhà quảng cáo hủy yêu cầu rút tiền đang chờ xử lý của họ
        // Admin từ chối yêu cầu với lý do
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectWithdrawalRequest(int id, [FromBody] string rejectionReason)
        {
            try
            {
                int adminId = 0; // Default value since we're not getting it from User claims anymore
                await _withdrawalRequestService.RejectWithdrawalRequestAsync(id, adminId, rejectionReason);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/WithdrawalRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWithdrawalRequest(int id)
        {
            try
            {
                var result = await _withdrawalRequestService.DeleteWithdrawalRequestAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Nhà quảng cáo kiểm tra tổng số tiền đang chờ rút
        [HttpGet("pending-total")]
        public async Task<ActionResult<decimal>> GetTotalPendingWithdrawalAmount([FromQuery] int advertiserId, [FromQuery] string currencyCode)
        {
            try
            {
                var totalPending = await _withdrawalRequestService.GetTotalPendingWithdrawalAmountAsync(advertiserId, currencyCode);
                return Ok(totalPending);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("total-withdrawn")]
        public async Task<ActionResult<decimal>> GetTotalWithdrawnAmount([FromQuery] int advertiserId, [FromQuery] string currencyCode)
        {
            try
            {
                var totalWithdrawn = await _withdrawalRequestService.GetTotalWithdrawnAmountAsync(advertiserId, currencyCode);
                return Ok(totalWithdrawn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}