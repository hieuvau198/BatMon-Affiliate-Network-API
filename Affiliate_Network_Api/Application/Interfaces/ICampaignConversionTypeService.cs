using Application.Contracts.CampaignConversionType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICampaignConversionTypeService
    {
        Task<IEnumerable<CampaignConversionTypeDto>> GetAllCampaignConversionTypesAsync();
        Task<CampaignConversionTypeDto> GetCampaignConversionTypeByIdAsync(int id);
        Task<IEnumerable<CampaignConversionTypeDto>> GetCampaignConversionTypesByCampaignIdAsync(int campaignId);
        Task<IEnumerable<CampaignConversionTypeDto>> GetCampaignConversionTypesByConversionTypeIdAsync(int conversionTypeId);
        Task<CampaignConversionTypeDto> CreateCampaignConversionTypeAsync(CampaignConversionTypeCreateDto campaignConversionTypeDto);
        Task<CampaignConversionTypeDto> UpdateCampaignConversionTypeAsync(CampaignConversionTypeUpdateDto campaignConversionTypeDto);
        Task<bool> DeleteCampaignConversionTypeAsync(int id);
        Task<bool> CampaignConversionTypeExistsAsync(int id);
        Task<int> GetCountByCampaignIdAsync(int campaignId);
        Task<int> GetCountByConversionTypeIdAsync(int conversionTypeId);
    }
}