// Conversion Controller
using Application.Contracts.Conversion;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversionDto>>> GetAllConversions()
        {
            var conversions = await _conversionService.GetAllConversionsAsync();
            return Ok(conversions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversionDto>> GetConversionById(int id)
        {
            var conversion = await _conversionService.GetConversionByIdAsync(id);
            return Ok(conversion);
        }

        [HttpGet("promote/{promoteId}")]
        public async Task<ActionResult<IEnumerable<ConversionDto>>> GetConversionsByPromoteId(int promoteId)
        {
            var conversions = await _conversionService.GetConversionsByPromoteIdAsync(promoteId);
            return Ok(conversions);
        }

        [HttpPost]
        public async Task<ActionResult<ConversionDto>> CreateConversion([FromBody] ConversionCreateDto conversionDto)
        {
            try
            {
                var createdConversion = await _conversionService.CreateConversionAsync(conversionDto);
                return CreatedAtAction(nameof(GetConversionById), new { id = createdConversion.ConversionId }, createdConversion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ConversionDto>> UpdateConversion([FromBody] ConversionUpdateDto conversionDto)
        {
            try
            {
                var updatedConversion = await _conversionService.UpdateConversionAsync(conversionDto);
                return Ok(updatedConversion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConversion(int id)
        {
            try
            {
                await _conversionService.DeleteConversionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> ConversionExists(int id)
        {
            var exists = await _conversionService.ConversionExistsAsync(id);
            return Ok(exists);
        }
    }
}