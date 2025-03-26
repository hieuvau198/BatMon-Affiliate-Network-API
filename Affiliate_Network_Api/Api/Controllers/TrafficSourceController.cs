using Application.Contracts.TrafficSource;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficSourceController : ControllerBase
    {
        private readonly ITrafficSourceService _trafficSourceService;

        public TrafficSourceController(ITrafficSourceService trafficSourceService)
        {
            _trafficSourceService = trafficSourceService ?? throw new ArgumentNullException(nameof(trafficSourceService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrafficSourceDto>>> GetAllTrafficSources()
        {
            var trafficSources = await _trafficSourceService.GetAllTrafficSourcesAsync();
            return Ok(trafficSources);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrafficSourceDto>> GetTrafficSourceById(int id, [FromQuery] bool includeRelated = false)
        {
            try
            {
                var trafficSource = await _trafficSourceService.GetTrafficSourceByIdAsync(id, includeRelated);
                return Ok(trafficSource);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("publisher/{publisherId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TrafficSourceDto>>> GetTrafficSourcesByPublisherId(int publisherId)
        {
            try
            {
                var trafficSources = await _trafficSourceService.GetTrafficSourcesByPublisherIdAsync(publisherId);
                return Ok(trafficSources);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrafficSourceDto>> CreateTrafficSource([FromBody] TrafficSourceCreateDto trafficSourceDto)
        {
            try
            {
                var createdTrafficSource = await _trafficSourceService.CreateTrafficSourceAsync(trafficSourceDto);
                return CreatedAtAction(nameof(GetTrafficSourceById), new { id = createdTrafficSource.SourceId }, createdTrafficSource);
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrafficSourceDto>> UpdateTrafficSource(int id, [FromBody] TrafficSourceUpdateDto trafficSourceDto)
        {
            try
            {
                // Validate that the ID in the URL matches the ID in the DTO
                if (id != trafficSourceDto.SourceId)
                {
                    return BadRequest(new { message = "The ID in the URL does not match the ID in the request body" });
                }

                var updatedTrafficSource = await _trafficSourceService.UpdateTrafficSourceAsync(trafficSourceDto);
                return Ok(updatedTrafficSource);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTrafficSource(int id)
        {
            try
            {
                await _trafficSourceService.DeleteTrafficSourceAsync(id);
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

        [HttpPatch("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ActivateTrafficSource(int id)
        {
            try
            {
                var result = await _trafficSourceService.ActivateTrafficSourceAsync(id);
                return Ok(new { success = result, message = "Traffic source activated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeactivateTrafficSource(int id)
        {
            try
            {
                var result = await _trafficSourceService.DeactivateTrafficSourceAsync(id);
                return Ok(new { success = result, message = "Traffic source deactivated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrafficSourceDto>>> GetActiveTrafficSources()
        {
            var trafficSources = await _trafficSourceService.GetActiveTrafficSourcesAsync();
            return Ok(trafficSources);
        }

        [HttpGet("type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TrafficSourceDto>>> GetTrafficSourcesByType(string type)
        {
            try
            {
                var trafficSources = await _trafficSourceService.GetTrafficSourcesByTypeAsync(type);
                return Ok(trafficSources);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTrafficSourceCount()
        {
            var count = await _trafficSourceService.GetTrafficSourceCountAsync();
            return Ok(count);
        }

        [HttpGet("count/publisher/{publisherId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTrafficSourceCountByPublisher(int publisherId)
        {
            var count = await _trafficSourceService.GetTrafficSourceCountByPublisherAsync(publisherId);
            return Ok(count);
        }
    }
}
