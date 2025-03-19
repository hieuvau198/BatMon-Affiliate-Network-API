using Application.Contracts.PaymentRequest;
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
    public class PaymentRequestController : ControllerBase
    {
        private readonly IPaymentRequestService _paymentRequestService;

        public PaymentRequestController(IPaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService ?? throw new ArgumentNullException(nameof(paymentRequestService));
        }

        // GET: api/PaymentRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentRequestDto>>> GetAllPaymentRequests([FromQuery] PaymentRequestFilterDto filter)
        {
            var paymentRequests = await _paymentRequestService.GetAllPaymentRequestsAsync(filter);
            return Ok(paymentRequests);
        }

        // GET: api/PaymentRequest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentRequestDto>> GetPaymentRequestById(int id)
        {
            try
            {
                var paymentRequest = await _paymentRequestService.GetPaymentRequestByIdAsync(id);
                return Ok(paymentRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/PaymentRequest/publisher/{publisherId}
        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<PaymentRequestDto>>> GetPaymentRequestsByPublisherId(int publisherId)
        {
            var paymentRequests = await _paymentRequestService.GetPaymentRequestsByPublisherIdAsync(publisherId);
            return Ok(paymentRequests);
        }

        // GET: api/PaymentRequest/status/{status}
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PaymentRequestDto>>> GetPaymentRequestsByStatus(string status)
        {
            var paymentRequests = await _paymentRequestService.GetPaymentRequestsByStatusAsync(status);
            return Ok(paymentRequests);
        }

        // GET: api/PaymentRequest/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPaymentRequestCount([FromQuery] PaymentRequestFilterDto filter)
        {
            var count = await _paymentRequestService.CountPaymentRequestsAsync(filter);
            return Ok(count);
        }

        // POST: api/PaymentRequest
        [HttpPost]
        public async Task<ActionResult<PaymentRequestDto>> CreatePaymentRequest([FromBody] PaymentRequestCreateDto paymentRequestDto)
        {
            try
            {
                var createdPaymentRequest = await _paymentRequestService.CreatePaymentRequestAsync(paymentRequestDto);
                return CreatedAtAction(nameof(GetPaymentRequestById), new { id = createdPaymentRequest.RequestId }, createdPaymentRequest);
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

        // PUT: api/PaymentRequest
        [HttpPut]
        public async Task<ActionResult<PaymentRequestDto>> UpdatePaymentRequest([FromBody] PaymentRequestUpdateDto paymentRequestDto)
        {
            try
            {
                var updatedPaymentRequest = await _paymentRequestService.UpdatePaymentRequestAsync(paymentRequestDto);
                return Ok(updatedPaymentRequest);
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

        // DELETE: api/PaymentRequest/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentRequest(int id)
        {
            var result = await _paymentRequestService.DeletePaymentRequestAsync(id);

            if (!result)
                return NotFound($"PaymentRequest with ID {id} not found");

            return NoContent();
        }

        // PATCH: api/PaymentRequest/{id}/approve
        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<bool>> ApprovePaymentRequest(int id, [FromQuery] int reviewerId)
        {
            var result = await _paymentRequestService.ApprovePaymentRequestAsync(id, reviewerId);

            if (!result)
                return NotFound($"PaymentRequest with ID {id} not found");

            return Ok(result);
        }

        // PATCH: api/PaymentRequest/{id}/reject
        [HttpPatch("{id}/reject")]
        public async Task<ActionResult<bool>> RejectPaymentRequest(int id, [FromQuery] int reviewerId, [FromQuery] string reason)
        {
            var result = await _paymentRequestService.RejectPaymentRequestAsync(id, reviewerId, reason);

            if (!result)
                return NotFound($"PaymentRequest with ID {id} not found");

            return Ok(result);
        }

        // GET: api/PaymentRequest/{id}/exists
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PaymentRequestExists(int id)
        {
            var exists = await _paymentRequestService.ExistsAsync(id);
            return Ok(exists);
        }

        // GET: api/PaymentRequest/total/publisher/{publisherId}
        [HttpGet("total/publisher/{publisherId}")]
        public async Task<ActionResult<decimal>> GetTotalPaymentAmountByPublisher(int publisherId)
        {
            var total = await _paymentRequestService.GetTotalPaymentAmountByPublisherAsync(publisherId);
            return Ok(total);
        }

        // GET: api/PaymentRequest/total/status/{status}
        [HttpGet("total/status/{status}")]
        public async Task<ActionResult<decimal>> GetTotalPaymentAmountByStatus(string status)
        {
            var total = await _paymentRequestService.GetTotalPaymentAmountByStatusAsync(status);
            return Ok(total);
        }
    }
}