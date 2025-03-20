using Application.Contracts.FraudCase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFraudCaseService
    {
        Task<IEnumerable<FraudCaseDto>> GetAllFraudCasesAsync();
        Task<FraudCaseDto> GetFraudCaseByIdAsync(int id);
        Task<FraudCaseDto> CreateFraudCaseAsync(FraudCaseCreateDto fraudCaseDto);
        Task<FraudCaseDto> UpdateFraudCaseAsync(FraudCaseUpdateDto fraudCaseDto);
        Task DeleteFraudCaseAsync(int id);
    }
}