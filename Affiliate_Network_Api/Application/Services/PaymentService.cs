using Application.Contracts.Payment;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync(PaymentFilterDto filter)
        {
            Expression<Func<Payment, bool>> predicate = BuildFilterPredicate(filter);

            var payments = await _unitOfWork.Payments.GetAllAsync(
                predicate,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetPaymentByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(
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
            var createdPayment = await _unitOfWork.Payments.CreateAsync(payment);

            if (payment.Status == "Completed")
            {
                var transaction = new Transaction
                {
                    SenderId = payment.SenderId,
                    ReceiverId = payment.ReceiverId,
                    Amount = payment.Amount ?? 0,
                    Currency = payment.CurrencyCode,
                    PaymentDate = payment.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                    Notes = payment.Notes,
                    TransactionRef = payment.TransactionId,
                    SenderName = "Sender Name",
                    ReceiverName = "Receiver Name"
                };

                await _unitOfWork.Transactions.CreateAsync(transaction);
            }

            await _unitOfWork.SaveChangesAsync();

            var paymentWithRelations = await _unitOfWork.Payments.GetByIdAsync(
                createdPayment.PaymentId,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<PaymentDto>(paymentWithRelations);
        }

        public async Task<PaymentDto?> UpdatePaymentAsync(int id, UpdatePaymentDto paymentDto)
        {
            var existingPayment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (existingPayment == null)
                return null;

            _mapper.Map(paymentDto, existingPayment);
            await _unitOfWork.Payments.UpdateAsync(existingPayment);

            if (existingPayment.Status == "Completed")
            {
                var transaction = new Transaction
                {
                    SenderId = existingPayment.SenderId,
                    ReceiverId = existingPayment.ReceiverId,
                    Amount = existingPayment.Amount ?? 0,
                    Currency = existingPayment.CurrencyCode,
                    PaymentDate = existingPayment.PaymentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                    Notes = existingPayment.Notes,
                    TransactionRef = existingPayment.TransactionId,
                    SenderName = "Sender Name",
                    ReceiverName = "Receiver Name"
                };

                await _unitOfWork.Transactions.CreateAsync(transaction);
            }

            await _unitOfWork.SaveChangesAsync();

            var updatedPayment = await _unitOfWork.Payments.GetByIdAsync(
                id,
                p => p.PaymentMethod!,
                p => p.CurrencyCodeNavigation);

            return _mapper.Map<PaymentDto>(updatedPayment);
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                return false;

            await _unitOfWork.Payments.DeleteAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApprovePaymentAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                return false;

            payment.Status = "Approved";
            await _unitOfWork.Payments.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectPaymentAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            if (payment == null)
                return false;

            payment.Status = "Rejected";
            await _unitOfWork.Payments.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Payments.ExistsAsync(p => p.PaymentId == id);
        }

        public async Task<int> CountPaymentsAsync(PaymentFilterDto filter)
        {
            Expression<Func<Payment, bool>> predicate = BuildFilterPredicate(filter);
            return await _unitOfWork.Payments.CountAsync(predicate);
        }

        public async Task<int> CountPaymentsByMethodAsync(int paymentMethodId)
        {
            return await _unitOfWork.Payments.CountAsync(p => p.PaymentMethodId == paymentMethodId);
        }

        public async Task<int> CountPaymentsByStatusAsync(string status)
        {
            return await _unitOfWork.Payments.CountAsync(p => p.Status == status);
        }

        private Expression<Func<Payment, bool>> BuildFilterPredicate(PaymentFilterDto filter)
        {
            Expression<Func<Payment, bool>> predicate = p => true;

            if (filter.RequestId.HasValue)
                predicate = predicate.And(p => p.RequestId == filter.RequestId);

            if (filter.PaymentMethodId.HasValue)
                predicate = predicate.And(p => p.PaymentMethodId == filter.PaymentMethodId);

            if (!string.IsNullOrEmpty(filter.Status))
                predicate = predicate.And(p => p.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.RequestType))
                predicate = predicate.And(p => p.RequestType == filter.RequestType);

            if (!string.IsNullOrEmpty(filter.CurrencyCode))
                predicate = predicate.And(p => p.CurrencyCode == filter.CurrencyCode);

            if (filter.StartDate.HasValue)
                predicate = predicate.And(p => p.PaymentDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                predicate = predicate.And(p => p.PaymentDate <= filter.EndDate);

            if (filter.MinAmount.HasValue)
                predicate = predicate.And(p => p.Amount >= filter.MinAmount);

            if (filter.MaxAmount.HasValue)
                predicate = predicate.And(p => p.Amount <= filter.MaxAmount);

            return predicate;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var combined = Expression.AndAlso(
                Expression.Invoke(expr1, parameter),
                Expression.Invoke(expr2, parameter));

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}
