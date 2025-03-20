using Microsoft.AspNetCore.Http;
using Application.Contracts.PromoteUrlvariantService;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.PromoteUrlvariantService;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoteUrlvariantController : ControllerBase
    {
        private readonly IPromoteUrlvariantService _promoteUrlvariantService;

        public PromoteUrlvariantController(IPromoteUrlvariantService promoteUrlvariantService)
        {
            _promoteUrlvariantService = promoteUrlvariantService ?? throw new ArgumentNullException(nameof(promoteUrlvariantService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromoteUrlvariantDto>>> GetAllPromoteUrlvariants()
        {
            var variants = await _promoteUrlvariantService.GetAllPromoteUrlvariantsAsync();
            return Ok(variants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromoteUrlvariantDto>> GetPromoteUrlvariantById(int id, [FromQuery] bool includeRelated = false)
        {
            try
            {
                var variant = await _promoteUrlvariantService.GetPromoteUrlvariantByIdAsync(id, includeRelated);
                return Ok(variant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("promote/{promoteId}")]
        public async Task<ActionResult<IEnumerable<PromoteUrlvariantDto>>> GetPromoteUrlvariantsByPromoteId(int promoteId)
        {
            try
            {
                var variants = await _promoteUrlvariantService.GetPromoteUrlvariantsByPromoteIdAsync(promoteId);
                return Ok(variants);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("trafficsource/{trafficSourceId}")]
        public async Task<ActionResult<IEnumerable<PromoteUrlvariantDto>>> GetPromoteUrlvariantsByTrafficSourceId(int trafficSourceId)
        {
            try
            {
                var variants = await _promoteUrlvariantService.GetPromoteUrlvariantsByTrafficSourceIdAsync(trafficSourceId);
                return Ok(variants);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<PromoteUrlvariantDto>> CreatePromoteUrlvariant([FromBody] PromoteUrlvariantCreateDto variantDto)
        {
            try
            {
                var createdVariant = await _promoteUrlvariantService.CreatePromoteUrlvariantAsync(variantDto);
                return CreatedAtAction(nameof(GetPromoteUrlvariantById), new { id = createdVariant.VariantId }, createdVariant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<PromoteUrlvariantDto>> UpdatePromoteUrlvariant([FromBody] PromoteUrlvariantUpdateDto variantDto)
        {
            try
            {
                var updatedVariant = await _promoteUrlvariantService.UpdatePromoteUrlvariantAsync(variantDto);
                return Ok(updatedVariant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromoteUrlvariant(int id)
        {
            try
            {
                await _promoteUrlvariantService.DeletePromoteUrlvariantAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivatePromoteUrlvariant(int id)
        {
            try
            {
                var result = await _promoteUrlvariantService.ActivatePromoteUrlvariantAsync(id);
                return Ok(new { success = result, message = "Promote URL variant activated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DeactivatePromoteUrlvariant(int id)
        {
            try
            {
                var result = await _promoteUrlvariantService.DeactivatePromoteUrlvariantAsync(id);
                return Ok(new { success = result, message = "Promote URL variant deactivated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<PromoteUrlvariantDto>>> GetActivePromoteUrlvariants()
        {
            var variants = await _promoteUrlvariantService.GetActivePromoteUrlvariantsAsync();
            return Ok(variants);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetPromoteUrlvariantCount()
        {
            var count = await _promoteUrlvariantService.GetPromoteUrlvariantCountAsync();
            return Ok(count);
        }

        [HttpGet("count/promote/{promoteId}")]
        public async Task<ActionResult<int>> GetPromoteUrlvariantCountByPromote(int promoteId)
        {
            var count = await _promoteUrlvariantService.GetPromoteUrlvariantCountByPromoteAsync(promoteId);
            return Ok(count);
        }

        [HttpGet("count/trafficsource/{trafficSourceId}")]
        public async Task<ActionResult<int>> GetPromoteUrlvariantCountByTrafficSource(int trafficSourceId)
        {
            var count = await _promoteUrlvariantService.GetPromoteUrlvariantCountByTrafficSourceAsync(trafficSourceId);
            return Ok(count);
        }
    }
}