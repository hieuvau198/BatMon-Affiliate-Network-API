using Application.Contracts.DepositRequest;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositRequestController : ControllerBase
    {
        private readonly IDepositRequestService _depositRequestService;

        public DepositRequestController(IDepositRequestService depositRequestService)
        {
            _depositRequestService = depositRequestService ?? throw new ArgumentNullException(nameof(depositRequestService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositRequestDto>>> GetAllDepositRequests()
        {
            var depositRequests = await _depositRequestService.GetAllDepositRequestsAsync();
            return Ok(depositRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepositRequestDto>> GetDepositRequestById(int id)
        {
            var depositRequest = await _depositRequestService.GetDepositRequestByIdAsync(id);
            return Ok(depositRequest);
        }

        [HttpPost]
        public async Task<ActionResult<DepositRequestDto>> CreateDepositRequest([FromBody] DepositRequestCreateDto depositRequestDto)
        {
            var createdDepositRequest = await _depositRequestService.CreateDepositRequestAsync(depositRequestDto);
            return CreatedAtAction(nameof(GetDepositRequestById), new { id = createdDepositRequest.RequestId }, createdDepositRequest);
        }

        [HttpPut]
        public async Task<ActionResult<DepositRequestDto>> UpdateDepositRequest([FromBody] DepositRequestUpdateDto depositRequestDto)
        {
            var updatedDepositRequest = await _depositRequestService.UpdateDepositRequestAsync(depositRequestDto);
            return Ok(updatedDepositRequest);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepositRequest(int id)
        {
            await _depositRequestService.DeleteDepositRequestAsync(id);
            return NoContent();
        }
    }
}