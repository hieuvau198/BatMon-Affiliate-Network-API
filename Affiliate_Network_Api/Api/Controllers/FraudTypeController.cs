using Application.Contracts.FraudType;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudTypeController : ControllerBase
    {
        private readonly IFraudTypeService _fraudTypeService;

        public FraudTypeController(IFraudTypeService fraudTypeService)
        {
            _fraudTypeService = fraudTypeService ?? throw new ArgumentNullException(nameof(fraudTypeService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FraudTypeDto>>> GetAllFraudTypes()
        {
            var fraudTypes = await _fraudTypeService.GetAllFraudTypesAsync();
            return Ok(fraudTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FraudTypeDto>> GetFraudTypeById(int id)
        {
            var fraudType = await _fraudTypeService.GetFraudTypeByIdAsync(id);
            return Ok(fraudType);
        }

        [HttpPost]
        public async Task<ActionResult<FraudTypeDto>> CreateFraudType([FromBody] FraudTypeCreateDto fraudTypeDto)
        {
            var createdFraudType = await _fraudTypeService.CreateFraudTypeAsync(fraudTypeDto);
            return CreatedAtAction(nameof(GetFraudTypeById), new { id = createdFraudType.FraudTypeId }, createdFraudType);
        }

        [HttpPut]
        public async Task<ActionResult<FraudTypeDto>> UpdateFraudType([FromBody] FraudTypeUpdateDto fraudTypeDto)
        {
            var updatedFraudType = await _fraudTypeService.UpdateFraudTypeAsync(fraudTypeDto);
            return Ok(updatedFraudType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFraudType(int id)
        {
            await _fraudTypeService.DeleteFraudTypeAsync(id);
            return NoContent();
        }
    }
}