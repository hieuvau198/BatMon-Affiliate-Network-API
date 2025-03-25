using Application.Contracts.WithdrawalRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWithdrawalRequestService
    {
        Task<IEnumerable<WithdrawalRequestDto>> GetAllWithdrawalRequestsAsync();
        Task<WithdrawalRequestDto> GetWithdrawalRequestByIdAsync(int id);
        Task<IEnumerable<WithdrawalRequestDto>> GetWithdrawalRequestsByAdvertiserIdAsync(int advertiserId);
        Task<WithdrawalRequestDto> CreateWithdrawalRequestAsync(CreateWithdrawalRequestDto withdrawalRequestDto);
        Task<WithdrawalRequestDto> UpdateWithdrawalRequestStatusAsync(int id, UpdateWithdrawalRequestDto updateDto);
        Task<bool> DeleteWithdrawalRequestAsync(int id);
        Task<IEnumerable<WithdrawalRequestDto>> GetFilteredWithdrawalRequestsAsync(WithdrawalRequestFilterDto filterDto);
        Task<bool> ApproveWithdrawalRequestAsync(int id, int reviewerId);
        Task<bool> RejectWithdrawalRequestAsync(int id, int reviewerId, string rejectionReason);
        Task<bool> WithdrawalRequestExistsAsync(int id);
        Task<decimal> GetTotalPendingWithdrawalAmountAsync(int advertiserId, string currencyCode);
        Task<decimal> GetTotalWithdrawnAmountAsync(int advertiserId, string currencyCode);

    }
}