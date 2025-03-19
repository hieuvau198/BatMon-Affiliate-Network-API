using Application.Contracts.PaymentMethod;
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
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllPaymentMethodsAsync(PaymentMethodFilterDto filter = null)
        {
            Expression<Func<PaymentMethod, bool>> predicate = null;

            if (filter != null)
            {
                predicate = p =>
                    (string.IsNullOrEmpty(filter.Type) || p.Type == filter.Type) &&
                    (string.IsNullOrEmpty(filter.Name) || p.Name.Contains(filter.Name)) &&
                    (!filter.IsActive.HasValue || p.IsActive == filter.IsActive) &&
                    (!filter.StartDate.HasValue || p.AddedDate >= filter.StartDate) &&
                    (!filter.EndDate.HasValue || p.AddedDate <= filter.EndDate);
            }

            var paymentMethods = await _unitOfWork.PaymentMethods.GetAllAsync(predicate);
            return _mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethods);
        }

        public async Task<PaymentMethodDto> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethods.GetByIdAsync(id);
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<PaymentMethodDto> CreatePaymentMethodAsync(CreatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodEntity = _mapper.Map<PaymentMethod>(paymentMethodDto);

            if (paymentMethodEntity.AddedDate == null)
                paymentMethodEntity.AddedDate = DateOnly.FromDateTime(DateTime.Today);

            var createdPaymentMethod = await _unitOfWork.PaymentMethods.CreateAsync(paymentMethodEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentMethodDto>(createdPaymentMethod);
        }

        public async Task<PaymentMethodDto> UpdatePaymentMethodAsync(int id, UpdatePaymentMethodDto paymentMethodDto)
        {
            var existingPaymentMethod = await _unitOfWork.PaymentMethods.GetByIdAsync(id);

            if (existingPaymentMethod == null)
                return null;

            // Update properties
            _mapper.Map(paymentMethodDto, existingPaymentMethod);

            await _unitOfWork.PaymentMethods.UpdateAsync(existingPaymentMethod);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PaymentMethodDto>(existingPaymentMethod);
        }

        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethods.GetByIdAsync(id);

            if (paymentMethod == null)
                return false;

            await _unitOfWork.PaymentMethods.DeleteAsync(paymentMethod);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.PaymentMethods.ExistsAsync(pm => pm.MethodId == id);
        }

        public async Task<int> CountPaymentMethodsAsync(PaymentMethodFilterDto filter = null)
        {
            Expression<Func<PaymentMethod, bool>> predicate = p => true;

            if (filter != null)
            {
                predicate = p =>
                    (string.IsNullOrEmpty(filter.Type) || p.Type == filter.Type) &&
                    (string.IsNullOrEmpty(filter.Name) || p.Name.Contains(filter.Name)) &&
                    (!filter.IsActive.HasValue || p.IsActive == filter.IsActive) &&
                    (!filter.StartDate.HasValue || p.AddedDate >= filter.StartDate) &&
                    (!filter.EndDate.HasValue || p.AddedDate <= filter.EndDate);
            }

            return await _unitOfWork.PaymentMethods.CountAsync(predicate);
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetActivePaymentMethodsAsync()
        {
            var activePaymentMethods = await _unitOfWork.PaymentMethods.GetAllAsync(p => p.IsActive == true);
            return _mapper.Map<IEnumerable<PaymentMethodDto>>(activePaymentMethods);
        }
    }
}