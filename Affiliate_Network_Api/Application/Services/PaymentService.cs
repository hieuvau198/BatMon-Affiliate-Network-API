using Application.Contracts.Payment;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IGenericRepository<Payment> paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(PaymentFilterDto filter)
        {
            Expression<Func<Payment, bool>> predicate = BuildFilterPredicate(filter);

            var payments = await _paymentRepository.GetAllAsync(
                predicate,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(
                id,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByMethodIdAsync(int paymentMethodId)
        {
            var filter = new PaymentFilterDto { PaymentMethodId = paymentMethodId };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByStatusAsync(string status)
        {
            var filter = new PaymentFilterDto { Status = status };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByRequestTypeAsync(string requestType)
        {
            var filter = new PaymentFilterDto { RequestType = requestType };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByCurrencyAsync(string currencyCode)
        {
            var filter = new PaymentFilterDto { CurrencyCode = currencyCode };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var filter = new PaymentFilterDto { StartDate = startDate, EndDate = endDate };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByRequestIdAsync(int requestId)
        {
            var filter = new PaymentFilterDto { RequestId = requestId };
            return await GetAllPaymentsAsync(filter);
        }

        public async Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);

            var createdPayment = await _paymentRepository.CreateAsync(payment);

            // Fetch the created payment with its relations
            var paymentWithRelations = await _paymentRepository.GetByIdAsync(
                createdPayment.PaymentId,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<PaymentDto>(paymentWithRelations);
        }

        public async Task<PaymentDto?> UpdatePaymentAsync(int id, UpdatePaymentDto paymentDto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);

            if (existingPayment == null)
                return null;

            // Update only the properties that are provided
            _mapper.Map(paymentDto, existingPayment);

            await _paymentRepository.UpdateAsync(existingPayment);

            // Fetch the updated payment with its relations
            var updatedPayment = await _paymentRepository.GetByIdAsync(
                id,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<PaymentDto>(updatedPayment);
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                return false;

            await _paymentRepository.DeleteAsync(payment);
            return true;
        }

        public async Task<bool> ApprovePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                return false;

            payment.Status = "Approved";
            await _paymentRepository.UpdateAsync(payment);
            return true;
        }

        public async Task<bool> RejectPaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                return false;

            payment.Status = "Rejected";
            await _paymentRepository.UpdateAsync(payment);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _paymentRepository.ExistsAsync(p => p.PaymentId == id);
        }

        public async Task<int> CountPaymentsAsync(PaymentFilterDto filter)
        {
            Expression<Func<Payment, bool>> predicate = BuildFilterPredicate(filter);
            return await _paymentRepository.CountAsync(predicate);
        }

        public async Task<int> CountPaymentsByMethodAsync(int paymentMethodId)
        {
            return await _paymentRepository.CountAsync(p => p.PaymentMethodId == paymentMethodId);
        }

        public async Task<int> CountPaymentsByStatusAsync(string status)
        {
            return await _paymentRepository.CountAsync(p => p.Status == status);
        }

        private Expression<Func<Payment, bool>> BuildFilterPredicate(PaymentFilterDto filter)
        {
            // Start with a predicate that is always true
            Expression<Func<Payment, bool>> predicate = p => true;

            if (filter.RequestId.HasValue)
                predicate = p => p.RequestId == filter.RequestId;

            if (filter.PaymentMethodId.HasValue)
                predicate = p => p.PaymentMethodId == filter.PaymentMethodId;

            if (!string.IsNullOrEmpty(filter.Status))
                predicate = p => p.Status == filter.Status;

            if (!string.IsNullOrEmpty(filter.RequestType))
                predicate = p => p.RequestType == filter.RequestType;

            if (!string.IsNullOrEmpty(filter.CurrencyCode))
                predicate = p => p.CurrencyCode == filter.CurrencyCode;

            if (filter.StartDate.HasValue)
                predicate = p => p.PaymentDate >= filter.StartDate;

            if (filter.EndDate.HasValue)
                predicate = p => p.PaymentDate <= filter.EndDate;

            if (filter.MinAmount.HasValue)
                predicate = p => p.Amount >= filter.MinAmount;

            if (filter.MaxAmount.HasValue)
                predicate = p => p.Amount <= filter.MaxAmount;

            return predicate;
        }
    }
}