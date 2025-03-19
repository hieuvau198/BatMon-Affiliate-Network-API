using Application.Contracts.PaymentMethod;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetAllPaymentMethods([FromQuery] PaymentMethodFilterDto filter)
        {
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync(filter);
            return Ok(paymentMethods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodDto>> GetPaymentMethodById(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return Ok(paymentMethod);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetActivePaymentMethods()
        {
            var paymentMethods = await _paymentMethodService.GetActivePaymentMethodsAsync();
            return Ok(paymentMethods);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetPaymentMethodsByType(string type)
        {
            var filter = new PaymentMethodFilterDto { Type = type };
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync(filter);
            return Ok(paymentMethods);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<PaymentMethodDto>>> GetPaymentMethodsByName(string name)
        {
            var filter = new PaymentMethodFilterDto { Name = name };
            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync(filter);
            return Ok(paymentMethods);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPaymentMethodCount([FromQuery] PaymentMethodFilterDto filter)
        {
            var count = await _paymentMethodService.CountPaymentMethodsAsync(filter);
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentMethodDto>> CreatePaymentMethod([FromBody] CreatePaymentMethodDto paymentMethodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(paymentMethodDto);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = createdPaymentMethod.MethodId }, createdPaymentMethod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentMethodDto>> UpdatePaymentMethod(int id, [FromBody] UpdatePaymentMethodDto paymentMethodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _paymentMethodService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var updatedPaymentMethod = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethodDto);
            return Ok(updatedPaymentMethod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentMethod(int id)
        {
            var result = await _paymentMethodService.DeletePaymentMethodAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<bool>> ActivatePaymentMethod(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            var updateDto = new UpdatePaymentMethodDto
            {
                Type = paymentMethod.Type,
                Name = paymentMethod.Name,
                Description = paymentMethod.Description,
                IsActive = true
            };

            var result = await _paymentMethodService.UpdatePaymentMethodAsync(id, updateDto);
            return Ok(result != null);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivatePaymentMethod(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            var updateDto = new UpdatePaymentMethodDto
            {
                Type = paymentMethod.Type,
                Name = paymentMethod.Name,
                Description = paymentMethod.Description,
                IsActive = false
            };

            var result = await _paymentMethodService.UpdatePaymentMethodAsync(id, updateDto);
            return Ok(result != null);
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> PaymentMethodExists(int id)
        {
            var exists = await _paymentMethodService.ExistsAsync(id);
            return Ok(exists);
        }
    }
}
