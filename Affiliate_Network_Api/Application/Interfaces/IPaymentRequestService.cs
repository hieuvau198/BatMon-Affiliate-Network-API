using Application.Contracts.PaymentRequest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentRequestService
    {
        Task<IEnumerable<PaymentRequestDto>> GetAllPaymentRequestsAsync(PaymentRequestFilterDto filter = null);
        Task<PaymentRequestDto> GetPaymentRequestByIdAsync(int id);
        Task<IEnumerable<PaymentRequestDto>> GetPaymentRequestsByPublisherIdAsync(int publisherId);
        Task<IEnumerable<PaymentRequestDto>> GetPaymentRequestsByStatusAsync(string status);
        Task<int> CountPaymentRequestsAsync(PaymentRequestFilterDto filter = null);
        Task<PaymentRequestDto> CreatePaymentRequestAsync(PaymentRequestCreateDto paymentRequestDto);
        Task<PaymentRequestDto> UpdatePaymentRequestAsync(PaymentRequestUpdateDto paymentRequestDto);
        Task<bool> DeletePaymentRequestAsync(int id);
        Task<bool> ApprovePaymentRequestAsync(int id, int reviewerId);
        Task<bool> RejectPaymentRequestAsync(int id, int reviewerId, string reason);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalPaymentAmountByPublisherAsync(int publisherId);
        Task<decimal> GetTotalPaymentAmountByStatusAsync(string status);
    }
}