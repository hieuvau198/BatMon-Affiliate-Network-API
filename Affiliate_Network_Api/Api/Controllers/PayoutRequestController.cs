using Application.Contracts.PayoutRequest;
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
    public class PayoutRequestController : ControllerBase
    {
        private readonly IPayoutRequestService _payoutRequestService;

        public PayoutRequestController(IPayoutRequestService payoutRequestService)
        {
            _payoutRequestService = payoutRequestService ?? throw new ArgumentNullException(nameof(payoutRequestService));
        }

        // GET: api/PayoutRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayoutRequestDto>>> GetAllPayoutRequests([FromQuery] PayoutRequestFilterDto filter)
        {
            var payoutRequests = await _payoutRequestService.GetAllPayoutRequestsAsync(filter);
            return Ok(payoutRequests);
        }

        // GET: api/PayoutRequest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PayoutRequestDto>> GetPayoutRequestById(int id)
        {
            try
            {
                var payoutRequest = await _payoutRequestService.GetPayoutRequestByIdAsync(id);
                return Ok(payoutRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/PayoutRequest/publisher/{publisherId}
        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<PayoutRequestDto>>> GetPayoutRequestsByPublisherId(int publisherId)
        {
            var payoutRequests = await _payoutRequestService.GetPayoutRequestsByPublisherIdAsync(publisherId);
            return Ok(payoutRequests);
        }

        // GET: api/PayoutRequest/status/{status}
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PayoutRequestDto>>> GetPayoutRequestsByStatus(string status)
        {
            var payoutRequests = await _payoutRequestService.GetPayoutRequestsByStatusAsync(status);
            return Ok(payoutRequests);
        }

        // GET: api/PayoutRequest/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPayoutRequestCount([FromQuery] PayoutRequestFilterDto filter)
        {
            var count = await _payoutRequestService.CountPayoutRequestsAsync(filter);
            return Ok(count);
        }

        // POST: api/PayoutRequest
        [HttpPost]
        public async Task<ActionResult<PayoutRequestDto>> CreatePayoutRequest([FromBody] PayoutRequestCreateDto payoutRequestDto)
        {
            try
            {
                var createdPayoutRequest = await _payoutRequestService.CreatePayoutRequestAsync(payoutRequestDto);
                return CreatedAtAction(nameof(GetPayoutRequestById), new { id = createdPayoutRequest.RequestId }, createdPayoutRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/PayoutRequest
        [HttpPut]
        public async Task<ActionResult<PayoutRequestDto>> UpdatePayoutRequest([FromBody] PayoutRequestUpdateDto payoutRequestDto)
        {
            try
            {
                var updatedPayoutRequest = await _payoutRequestService.UpdatePayoutRequestAsync(payoutRequestDto);
                return Ok(updatedPayoutRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/PayoutRequest/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayoutRequest(int id)
        {
            var result = await _payoutRequestService.DeletePayoutRequestAsync(id);

            if (!result)
                return NotFound($"PayoutRequest with ID {id} not found");

            return NoContent();
        }

        // PATCH: api/PayoutRequest/{id}/approve
        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<bool>> ApprovePayoutRequest(int id, [FromQuery] int reviewerId)
        {
            var result = await _payoutRequestService.ApprovePayoutRequestAsync(id, reviewerId);

            if (!result)
                return NotFound($"PayoutRequest with ID {id} not found");

            return Ok(result);
        }

        // PATCH: api/PayoutRequest/{id}/reject
        [HttpPatch("{id}/reject")]
        public async Task<ActionResult<bool>> RejectPayoutRequest(int id, [FromQuery] int reviewerId, [FromQuery] string reason)
        {
            var result = await _payoutRequestService.RejectPayoutRequestAsync(id, reviewerId, reason);

            if (!result)
                return NotFound($"PayoutRequest with ID {id} not found");

            return Ok(result);
        }

        // GET: api/PayoutRequest/{id}/exists
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PayoutRequestExists(int id)
        {
            var exists = await _payoutRequestService.ExistsAsync(id);
            return Ok(exists);
        }

        // GET: api/PayoutRequest/total/publisher/{publisherId}
        [HttpGet("total/publisher/{publisherId}")]
        public async Task<ActionResult<decimal>> GetTotalPayoutAmountByPublisher(int publisherId)
        {
            var total = await _payoutRequestService.GetTotalPayoutAmountByPublisherAsync(publisherId);
            return Ok(total);
        }

        // GET: api/PayoutRequest/total/status/{status}
        [HttpGet("total/status/{status}")]
        public async Task<ActionResult<decimal>> GetTotalPayoutAmountByStatus(string status)
        {
            var total = await _payoutRequestService.GetTotalPayoutAmountByStatusAsync(status);
            return Ok(total);
        }
    }
}