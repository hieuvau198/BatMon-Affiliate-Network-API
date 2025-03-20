using Application.Contracts.PayoutRule;
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
    public class PayoutRuleController : ControllerBase
    {
        private readonly IPayoutRuleService _payoutRuleService;

        public PayoutRuleController(IPayoutRuleService payoutRuleService)
        {
            _payoutRuleService = payoutRuleService ?? throw new ArgumentNullException(nameof(payoutRuleService));
        }

        // GET: api/PayoutRule
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PayoutRuleDto>>> GetPayoutRules([FromQuery] PayoutRuleFilterDto filter)
        {
            var payoutRules = await _payoutRuleService.GetAllPayoutRulesAsync(filter);
            return Ok(payoutRules);
        }

        // GET: api/PayoutRule/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PayoutRuleDto>> GetPayoutRuleById(int id)
        {
            try
            {
                var payoutRule = await _payoutRuleService.GetPayoutRuleByIdAsync(id);
                return Ok(payoutRule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/PayoutRule/currency/{currency}
        [HttpGet("currency/{currency}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PayoutRuleDto>>> GetPayoutRulesByCurrency(string currency)
        {
            var payoutRules = await _payoutRuleService.GetPayoutRulesByCurrencyAsync(currency);
            return Ok(payoutRules);
        }

        // GET: api/PayoutRule/count
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetPayoutRuleCount([FromQuery] PayoutRuleFilterDto filter)
        {
            var count = await _payoutRuleService.CountPayoutRulesAsync(filter);
            return Ok(count);
        }

        // POST: api/PayoutRule
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PayoutRuleDto>> CreatePayoutRule([FromBody] CreatePayoutRuleDto payoutRuleDto)
        {
            try
            {
                var createdPayoutRule = await _payoutRuleService.CreatePayoutRuleAsync(payoutRuleDto);
                return CreatedAtAction(nameof(GetPayoutRuleById), new { id = createdPayoutRule.RuleId }, createdPayoutRule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/PayoutRule/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PayoutRuleDto>> UpdatePayoutRule(int id, [FromBody] UpdatePayoutRuleDto payoutRuleDto)
        {
            try
            {
                var updatedPayoutRule = await _payoutRuleService.UpdatePayoutRuleAsync(id, payoutRuleDto);
                return Ok(updatedPayoutRule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/PayoutRule/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePayoutRule(int id)
        {
            var result = await _payoutRuleService.DeletePayoutRuleAsync(id);

            if (!result)
                return NotFound($"PayoutRule with ID {id} not found");

            return NoContent();
        }

        // GET: api/PayoutRule/{id}/exists
        [HttpGet("{id}/exists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> PayoutRuleExists(int id)
        {
            var exists = await _payoutRuleService.PayoutRuleExistsAsync(id);
            return Ok(exists);
        }
    }
}