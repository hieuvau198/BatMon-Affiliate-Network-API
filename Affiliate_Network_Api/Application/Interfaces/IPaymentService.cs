using Application.Contracts.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(PaymentFilterDto filter);
        Task<PaymentDto?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByMethodIdAsync(int paymentMethodId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByStatusAsync(string status);
        Task<IEnumerable<PaymentDto>> GetPaymentsByRequestTypeAsync(string requestType);
        Task<IEnumerable<PaymentDto>> GetPaymentsByCurrencyAsync(string currencyCode);
        Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<PaymentDto>> GetPaymentsByRequestIdAsync(int requestId);
        Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto paymentDto);
        Task<PaymentDto?> UpdatePaymentAsync(int id, UpdatePaymentDto paymentDto);
        Task<bool> DeletePaymentAsync(int id);
        Task<bool> ApprovePaymentAsync(int id);
        Task<bool> RejectPaymentAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountPaymentsAsync(PaymentFilterDto filter);
        Task<int> CountPaymentsByMethodAsync(int paymentMethodId);
        Task<int> CountPaymentsByStatusAsync(string status);
    }
}
