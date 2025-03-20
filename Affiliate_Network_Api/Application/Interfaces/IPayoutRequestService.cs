using Application.Contracts.PayoutRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPayoutRequestService
    {
        Task<IEnumerable<PayoutRequestDto>> GetAllPayoutRequestsAsync(PayoutRequestFilterDto filter = null);
        Task<PayoutRequestDto> GetPayoutRequestByIdAsync(int id);
        Task<IEnumerable<PayoutRequestDto>> GetPayoutRequestsByPublisherIdAsync(int publisherId);
        Task<IEnumerable<PayoutRequestDto>> GetPayoutRequestsByStatusAsync(string status);
        Task<int> CountPayoutRequestsAsync(PayoutRequestFilterDto filter = null);
        Task<PayoutRequestDto> CreatePayoutRequestAsync(PayoutRequestCreateDto payoutRequestDto);
        Task<PayoutRequestDto> UpdatePayoutRequestAsync(PayoutRequestUpdateDto payoutRequestDto);
        Task<bool> DeletePayoutRequestAsync(int id);
        Task<bool> ApprovePayoutRequestAsync(int id, int reviewerId);
        Task<bool> RejectPayoutRequestAsync(int id, int reviewerId, string reason);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalPayoutAmountByPublisherAsync(int publisherId);
        Task<decimal> GetTotalPayoutAmountByStatusAsync(string status);
    }
}