using Application.Contracts.CampaignPolicy;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignPolicyController : ControllerBase
    {
        private readonly ICampaignPolicyService _policyService;

        public CampaignPolicyController(ICampaignPolicyService policyService)
        {
            _policyService = policyService ?? throw new ArgumentNullException(nameof(policyService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignPolicyDto>>> GetAllCampaignPolicies()
        {
            var policies = await _policyService.GetAllCampaignPoliciesAsync();
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignPolicyDto>> GetCampaignPolicyById(int id)
        {
            try
            {
                var policy = await _policyService.GetCampaignPolicyByIdAsync(id);
                return Ok(policy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("appliedTo/{appliedTo}")]
        public async Task<ActionResult<IEnumerable<CampaignPolicyDto>>> GetCampaignPoliciesByAppliedTo(string appliedTo)
        {
            var policies = await _policyService.GetCampaignPoliciesByAppliedToAsync(appliedTo);
            return Ok(policies);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCampaignPolicyCount()
        {
            var count = await _policyService.GetCampaignPolicyCountAsync();
            return Ok(count);
        }

        [HttpPost]
        public async Task<ActionResult<CampaignPolicyDto>> CreateCampaignPolicy([FromBody] CampaignPolicyCreateDto policyDto)
        {
            var createdPolicy = await _policyService.CreateCampaignPolicyAsync(policyDto);
            return CreatedAtAction(nameof(GetCampaignPolicyById), new { id = createdPolicy.PolicyId }, createdPolicy);
        }

        [HttpPut]
        public async Task<ActionResult<CampaignPolicyDto>> UpdateCampaignPolicy([FromBody] CampaignPolicyUpdateDto policyDto)
        {
            try
            {
                var updatedPolicy = await _policyService.UpdateCampaignPolicyAsync(policyDto);
                return Ok(updatedPolicy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCampaignPolicy(int id)
        {
            try
            {
                await _policyService.DeleteCampaignPolicyAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> CampaignPolicyExists(int id)
        {
            var exists = await _policyService.CampaignPolicyExistsAsync(id);
            return Ok(exists);
        }
    }
}
