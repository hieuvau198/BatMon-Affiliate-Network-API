using Application.Contracts.FraudType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFraudTypeService
    {
        Task<IEnumerable<FraudTypeDto>> GetAllFraudTypesAsync();
        Task<FraudTypeDto> GetFraudTypeByIdAsync(int id);
        Task<FraudTypeDto> CreateFraudTypeAsync(FraudTypeCreateDto fraudTypeDto);
        Task<FraudTypeDto> UpdateFraudTypeAsync(FraudTypeUpdateDto fraudTypeDto);
        Task DeleteFraudTypeAsync(int id);
    }
}