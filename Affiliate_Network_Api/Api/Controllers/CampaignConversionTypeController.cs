using Application.Contracts.CampaignConversionType;
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
    public class CampaignConversionTypeController : ControllerBase
    {
        private readonly ICampaignConversionTypeService _campaignConversionTypeService;

        public CampaignConversionTypeController(ICampaignConversionTypeService campaignConversionTypeService)
        {
            _campaignConversionTypeService = campaignConversionTypeService ?? throw new ArgumentNullException(nameof(campaignConversionTypeService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignConversionTypeDto>>> GetAll()
        {
            var campaignConversionTypes = await _campaignConversionTypeService.GetAllCampaignConversionTypesAsync();
            return Ok(campaignConversionTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignConversionTypeDto>> GetById(int id)
        {
            try
            {
                var campaignConversionType = await _campaignConversionTypeService.GetCampaignConversionTypeByIdAsync(id);
                return Ok(campaignConversionType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampaignConversionTypeDto>> Create([FromBody] CampaignConversionTypeCreateDto createDto)
        {
            try
            {
                var createdCampaignConversionType = await _campaignConversionTypeService.CreateCampaignConversionTypeAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCampaignConversionType.CampaignConversionId }, createdCampaignConversionType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CampaignConversionTypeDto>> Update([FromBody] CampaignConversionTypeUpdateDto updateDto)
        {
            try
            {
                var updatedCampaignConversionType = await _campaignConversionTypeService.UpdateCampaignConversionTypeAsync(updateDto);
                return Ok(updatedCampaignConversionType);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _campaignConversionTypeService.DeleteCampaignConversionTypeAsync(id);
                if (result)
                    return NoContent();
                return NotFound($"CampaignConversionType with ID {id} not found.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _campaignConversionTypeService.CampaignConversionTypeExistsAsync(id);
            return Ok(exists);
        }

    }
}