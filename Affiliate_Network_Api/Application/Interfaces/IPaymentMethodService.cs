using Application.Contracts.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync(PaymentMethodFilterDto filter = null);
        Task<PaymentMethodDto> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodDto paymentMethodDto);
        Task<PaymentMethodDto> UpdatePaymentMethodAsync(int id, UpdatePaymentMethodDto paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountPaymentMethodsAsync(PaymentMethodFilterDto filter = null);
        Task<IEnumerable<PaymentMethodDto>> GetActivePaymentMethodsAsync();
    }
}
