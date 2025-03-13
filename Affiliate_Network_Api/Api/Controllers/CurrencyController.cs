using Application.Contracts.Currency;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }

        // GET: api/Currency
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetCurrencies()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }

        // GET: api/Currency/{code}
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyDto>> GetCurrency(string code)
        {
            try
            {
                var currency = await _currencyService.GetCurrencyByCodeAsync(code);
                return Ok(currency);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Currency
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CurrencyDto>> CreateCurrency([FromBody] CreateCurrencyDto currencyDto)
        {
            try
            {
                var createdCurrency = await _currencyService.CreateCurrencyAsync(currencyDto);
                return CreatedAtAction(nameof(GetCurrency), new { code = createdCurrency.CurrencyCode }, createdCurrency);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Currency/{code}
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurrencyDto>> UpdateCurrency(string code, [FromBody] UpdateCurrencyDto currencyDto)
        {
            try
            {
                var updatedCurrency = await _currencyService.UpdateCurrencyAsync(code, currencyDto);
                return Ok(updatedCurrency);
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

        // DELETE: api/Currency/{code}
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCurrency(string code)
        {
            var result = await _currencyService.DeleteCurrencyAsync(code);
            if (!result)
                return NotFound($"Currency with code {code} not found.");

            return NoContent();
        }
    }
}
