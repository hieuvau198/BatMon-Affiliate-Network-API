using Application.Contracts.FraudCase;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudCaseController : ControllerBase
    {
        private readonly IFraudCaseService _fraudCaseService;

        public FraudCaseController(IFraudCaseService fraudCaseService)
        {
            _fraudCaseService = fraudCaseService ?? throw new ArgumentNullException(nameof(fraudCaseService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FraudCaseDto>>> GetAllFraudCases()
        {
            var fraudCases = await _fraudCaseService.GetAllFraudCasesAsync();
            return Ok(fraudCases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FraudCaseDto>> GetFraudCaseById(int id)
        {
            var fraudCase = await _fraudCaseService.GetFraudCaseByIdAsync(id);
            return Ok(fraudCase);
        }

        [HttpPost]
        public async Task<ActionResult<FraudCaseDto>> CreateFraudCase([FromBody] FraudCaseCreateDto fraudCaseDto)
        {
            var createdFraudCase = await _fraudCaseService.CreateFraudCaseAsync(fraudCaseDto);
            return CreatedAtAction(nameof(GetFraudCaseById), new { id = createdFraudCase.CaseId }, createdFraudCase);
        }

        [HttpPut]
        public async Task<ActionResult<FraudCaseDto>> UpdateFraudCase([FromBody] FraudCaseUpdateDto fraudCaseDto)
        {
            var updatedFraudCase = await _fraudCaseService.UpdateFraudCaseAsync(fraudCaseDto);
            return Ok(updatedFraudCase);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFraudCase(int id)
        {
            await _fraudCaseService.DeleteFraudCaseAsync(id);
            return NoContent();
        }
    }
}