using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Conversion;

namespace Application.Interfaces
{
    public interface IConversionService
    {
        Task<IEnumerable<ConversionDto>> GetAllConversionsAsync();
        Task<ConversionDto> GetConversionByIdAsync(int id);
        Task<IEnumerable<ConversionDto>> GetConversionsByPromoteIdAsync(int promoteId);
        Task<ConversionDto> CreateConversionAsync(ConversionCreateDto conversionDto);
        Task<ConversionDto> UpdateConversionAsync(ConversionUpdateDto conversionDto);
        Task DeleteConversionAsync(int id);
        Task<bool> ConversionExistsAsync(int id);
    }
}
