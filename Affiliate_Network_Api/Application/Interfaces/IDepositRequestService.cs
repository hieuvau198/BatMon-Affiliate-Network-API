using Application.Contracts.DepositRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDepositRequestService
    {
        Task<IEnumerable<DepositRequestDto>> GetAllDepositRequestsAsync();
        Task<DepositRequestDto> GetDepositRequestByIdAsync(int id);
        Task<DepositRequestDto> CreateDepositRequestAsync(DepositRequestCreateDto depositRequestDto);
        Task<DepositRequestDto> UpdateDepositRequestAsync(DepositRequestUpdateDto depositRequestDto);
        Task DeleteDepositRequestAsync(int id);
    }
}