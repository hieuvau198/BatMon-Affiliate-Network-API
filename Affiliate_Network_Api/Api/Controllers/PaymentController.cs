using Application.Contracts.Payment;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllPayments([FromQuery] PaymentFilterDto filter)
        {
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpGet("method/{paymentMethodId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByMethodId(int paymentMethodId)
        {
            var filter = new PaymentFilterDto { PaymentMethodId = paymentMethodId };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByStatus(string status)
        {
            var filter = new PaymentFilterDto { Status = status };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("currency/{currencyCode}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByCurrency(string currencyCode)
        {
            var filter = new PaymentFilterDto { CurrencyCode = currencyCode };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("type/{requestType}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByRequestType(string requestType)
        {
            var filter = new PaymentFilterDto { RequestType = requestType };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByDateRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            var filter = new PaymentFilterDto { StartDate = startDate, EndDate = endDate };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("request/{requestId}")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByRequestId(int requestId)
        {
            var filter = new PaymentFilterDto { RequestId = requestId };
            var payments = await _paymentService.GetAllPaymentsAsync(filter);
            return Ok(payments);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPaymentCount([FromQuery] PaymentFilterDto filter)
        {
            var count = await _paymentService.CountPaymentsAsync(filter);
            return Ok(count);
        }

        [HttpGet("count/method/{paymentMethodId}")]
        public async Task<ActionResult<int>> GetPaymentCountByMethod(int paymentMethodId)
        {
            var filter = new PaymentFilterDto { PaymentMethodId = paymentMethodId };
            var count = await _paymentService.CountPaymentsAsync(filter);
            return Ok(count);
        }

        [HttpGet("count/status/{status}")]
        public async Task<ActionResult<int>> GetPaymentCountByStatus(string status)
        {
            var filter = new PaymentFilterDto { Status = status };
            var count = await _paymentService.CountPaymentsAsync(filter);
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPayment = await _paymentService.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentId }, createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDto>> UpdatePayment(int id, [FromBody] UpdatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _paymentService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var updatedPayment = await _paymentService.UpdatePaymentAsync(id, paymentDto);
            return Ok(updatedPayment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            var result = await _paymentService.DeletePaymentAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<bool>> ApprovePayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            var updateDto = new UpdatePaymentDto
            {
                Status = "Approved",
                CurrencyCode = payment.CurrencyCode
            };

            var result = await _paymentService.UpdatePaymentAsync(id, updateDto);
            return Ok(result != null);
        }

        [HttpPatch("{id}/reject")]
        public async Task<ActionResult<bool>> RejectPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            var updateDto = new UpdatePaymentDto
            {
                Status = "Rejected",
                CurrencyCode = payment.CurrencyCode
            };

            var result = await _paymentService.UpdatePaymentAsync(id, updateDto);
            return Ok(result != null);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PaymentExists(int id)
        {
            var exists = await _paymentService.ExistsAsync(id);
            return Ok(exists);
        }
    }
}