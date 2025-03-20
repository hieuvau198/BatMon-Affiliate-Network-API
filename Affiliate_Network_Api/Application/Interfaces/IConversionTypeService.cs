using Application.Contracts.ConversionType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IConversionTypeService
    {
        Task<IEnumerable<ConversionTypeDto>> GetAllConversionTypesAsync();
        Task<ConversionTypeDto> GetConversionTypeByIdAsync(int id, bool includeRelated = false);
        Task<ConversionTypeDto> CreateConversionTypeAsync(ConversionTypeCreateDto conversionTypeDto);
        Task<ConversionTypeDto> UpdateConversionTypeAsync(ConversionTypeUpdateDto conversionTypeDto);
        Task DeleteConversionTypeAsync(int id);
        Task<bool> ConversionTypeExistsAsync(int id);
        Task<int> GetConversionTypeCountAsync();
        Task<IEnumerable<ConversionTypeDto>> GetConversionTypesByRequiresApprovalAsync(bool requiresApproval);
        Task<IEnumerable<ConversionTypeDto>> GetConversionTypesByActionTypeAsync(string actionType);
    }
}