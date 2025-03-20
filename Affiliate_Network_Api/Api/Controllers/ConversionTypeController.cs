using Application.Contracts.ConversionType;
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
    public class ConversionTypeController : ControllerBase
    {
        private readonly IConversionTypeService _conversionTypeService;

        public ConversionTypeController(IConversionTypeService conversionTypeService)
        {
            _conversionTypeService = conversionTypeService ?? throw new ArgumentNullException(nameof(conversionTypeService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversionTypeDto>>> GetAllConversionTypes()
        {
            var conversionTypes = await _conversionTypeService.GetAllConversionTypesAsync();
            return Ok(conversionTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversionTypeDto>> GetConversionTypeById(int id, [FromQuery] bool includeRelated = false)
        {
            try
            {
                var conversionType = await _conversionTypeService.GetConversionTypeByIdAsync(id, includeRelated);
                return Ok(conversionType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("requiresApproval/{requiresApproval}")]
        public async Task<ActionResult<IEnumerable<ConversionTypeDto>>> GetConversionTypesByRequiresApproval(bool requiresApproval)
        {
            var conversionTypes = await _conversionTypeService.GetConversionTypesByRequiresApprovalAsync(requiresApproval);
            return Ok(conversionTypes);
        }

        [HttpGet("actionType/{actionType}")]
        public async Task<ActionResult<IEnumerable<ConversionTypeDto>>> GetConversionTypesByActionType(string actionType)
        {
            var conversionTypes = await _conversionTypeService.GetConversionTypesByActionTypeAsync(actionType);
            return Ok(conversionTypes);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetConversionTypeCount()
        {
            var count = await _conversionTypeService.GetConversionTypeCountAsync();
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<ConversionTypeDto>> CreateConversionType([FromBody] ConversionTypeCreateDto conversionTypeDto)
        {
            try
            {
                var createdConversionType = await _conversionTypeService.CreateConversionTypeAsync(conversionTypeDto);
                return CreatedAtAction(nameof(GetConversionTypeById), new { id = createdConversionType.TypeId }, createdConversionType);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ConversionTypeDto>> UpdateConversionType([FromBody] ConversionTypeUpdateDto conversionTypeDto)
        {
            try
            {
                var updatedConversionType = await _conversionTypeService.UpdateConversionTypeAsync(conversionTypeDto);
                return Ok(updatedConversionType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConversionType(int id)
        {
            try
            {
                await _conversionTypeService.DeleteConversionTypeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> ConversionTypeExists(int id)
        {
            var exists = await _conversionTypeService.ConversionTypeExistsAsync(id);
            return Ok(exists);
        }
    }
}