using Application.Contracts.PaymentRequest;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly IGenericRepository<PayoutRequest> _paymentRequestRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public PaymentRequestService(
            IGenericRepository<PayoutRequest> paymentRequestRepository,
            IGenericRepository<Publisher> publisherRepository,
            IGenericRepository<Currency> currencyRepository,
            IMapper mapper)
        {
            _paymentRequestRepository = paymentRequestRepository ?? throw new ArgumentNullException(nameof(paymentRequestRepository));
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PaymentRequestDto>> GetAllPaymentRequestsAsync(PaymentRequestFilterDto filter = null)
        {
            Expression<Func<PayoutRequest, bool>> predicate = null;

            if (filter != null)
            {
                predicate = BuildFilterPredicate(filter);
            }

            var includes = new Expression<Func<PayoutRequest, object>>[]
            {
                p => p.Publisher,
                p => p.CurrencyCodeNavigation
            };

            var paymentRequests = await _paymentRequestRepository.GetAllAsync(predicate, includes);
            return _mapper.Map<IEnumerable<PaymentRequestDto>>(paymentRequests);
        }

        public async Task<PaymentRequestDto> GetPaymentRequestByIdAsync(int id)
        {
            var includes = new Expression<Func<PayoutRequest, object>>[]
            {
                p => p.Publisher,
                p => p.CurrencyCodeNavigation
            };

            var paymentRequest = await _paymentRequestRepository.GetByIdAsync(id, includes);

            if (paymentRequest == null)
                throw new KeyNotFoundException($"PaymentRequest with ID {id} not found");

            return _mapper.Map<PaymentRequestDto>(paymentRequest);
        }

        public async Task<IEnumerable<PaymentRequestDto>> GetPaymentRequestsByPublisherIdAsync(int publisherId)
        {
            var filter = new PaymentRequestFilterDto { PublisherId = publisherId };
            return await GetAllPaymentRequestsAsync(filter);
        }

        public async Task<IEnumerable<PaymentRequestDto>> GetPaymentRequestsByStatusAsync(string status)
        {
            var filter = new PaymentRequestFilterDto { Status = status };
            return await GetAllPaymentRequestsAsync(filter);
        }

        public async Task<int> CountPaymentRequestsAsync(PaymentRequestFilterDto filter = null)
        {
            Expression<Func<PayoutRequest, bool>> predicate = null;

            if (filter != null)
            {
                predicate = BuildFilterPredicate(filter);
            }

            return await _paymentRequestRepository.CountAsync(predicate);
        }

        public async Task<PaymentRequestDto> CreatePaymentRequestAsync(PaymentRequestCreateDto paymentRequestDto)
        {
            // Validate publisher
            if (paymentRequestDto.PublisherId.HasValue)
            {
                var publisherExists = await _publisherRepository.ExistsAsync(p => p.PublisherId == paymentRequestDto.PublisherId);
                if (!publisherExists)
                    throw new KeyNotFoundException($"Publisher with ID {paymentRequestDto.PublisherId} not found");
            }

            // Validate currency
            var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == paymentRequestDto.CurrencyCode);
            if (!currencyExists)
                throw new KeyNotFoundException($"Currency with Code {paymentRequestDto.CurrencyCode} not found");

            // Map DTO to entity
            var paymentRequest = _mapper.Map<PayoutRequest>(paymentRequestDto);

            // Set default values
            paymentRequest.RequestDate = DateOnly.FromDateTime(DateTime.Now);
            paymentRequest.Status = "Pending";

            // Create the entity
            var createdPaymentRequest = await _paymentRequestRepository.CreateAsync(paymentRequest);

            // Return the created entity
            return _mapper.Map<PaymentRequestDto>(createdPaymentRequest);
        }

        public async Task<PaymentRequestDto> UpdatePaymentRequestAsync(PaymentRequestUpdateDto paymentRequestDto)
        {
            var includes = new Expression<Func<PayoutRequest, object>>[]
            {
                p => p.Publisher,
                p => p.CurrencyCodeNavigation
            };

            var existingPaymentRequest = await _paymentRequestRepository.GetByIdAsync(paymentRequestDto.RequestId, includes);

            if (existingPaymentRequest == null)
                throw new KeyNotFoundException($"PaymentRequest with ID {paymentRequestDto.RequestId} not found");

            // Update only the fields that can be updated
            _mapper.Map(paymentRequestDto, existingPaymentRequest);

            // Save the changes
            await _paymentRequestRepository.UpdateAsync(existingPaymentRequest);

            // Return the updated entity
            return _mapper.Map<PaymentRequestDto>(existingPaymentRequest);
        }

        public async Task<bool> DeletePaymentRequestAsync(int id)
        {
            var paymentRequest = await _paymentRequestRepository.GetByIdAsync(id);

            if (paymentRequest == null)
                return false;

            await _paymentRequestRepository.DeleteAsync(paymentRequest);
            return true;
        }

        public async Task<bool> ApprovePaymentRequestAsync(int id, int reviewerId)
        {
            var paymentRequest = await _paymentRequestRepository.GetByIdAsync(id);

            if (paymentRequest == null)
                return false;

            paymentRequest.Status = "Approved";
            paymentRequest.ReviewedBy = reviewerId;
            paymentRequest.RejectionReason = null;

            await _paymentRequestRepository.UpdateAsync(paymentRequest);
            return true;
        }

        public async Task<bool> RejectPaymentRequestAsync(int id, int reviewerId, string reason)
        {
            var paymentRequest = await _paymentRequestRepository.GetByIdAsync(id);

            if (paymentRequest == null)
                return false;

            paymentRequest.Status = "Rejected";
            paymentRequest.ReviewedBy = reviewerId;
            paymentRequest.RejectionReason = reason;

            await _paymentRequestRepository.UpdateAsync(paymentRequest);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _paymentRequestRepository.ExistsAsync(p => p.RequestId == id);
        }

        public async Task<decimal> GetTotalPaymentAmountByPublisherAsync(int publisherId)
        {
            var paymentRequests = await _paymentRequestRepository.GetAllAsync(
                p => p.PublisherId == publisherId && p.Status == "Approved");

            return paymentRequests.Sum(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetTotalPaymentAmountByStatusAsync(string status)
        {
            var paymentRequests = await _paymentRequestRepository.GetAllAsync(
                p => p.Status == status);

            return paymentRequests.Sum(p => p.Amount ?? 0);
        }

        private Expression<Func<PayoutRequest, bool>> BuildFilterPredicate(PaymentRequestFilterDto filter)
        {
            Expression<Func<PayoutRequest, bool>> predicate = p => true;

            if (filter.PublisherId.HasValue)
            {
                var publisherId = filter.PublisherId.Value;
                predicate = p => p.PublisherId == publisherId;
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                var status = filter.Status;
                predicate = p => p.Status == status;
            }

            if (filter.StartDate.HasValue)
            {
                var startDate = filter.StartDate.Value;
                predicate = p => p.RequestDate >= startDate;
            }

            if (filter.EndDate.HasValue)
            {
                var endDate = filter.EndDate.Value;
                predicate = p => p.RequestDate <= endDate;
            }

            if (filter.ReviewedBy.HasValue)
            {
                var reviewedBy = filter.ReviewedBy.Value;
                predicate = p => p.ReviewedBy == reviewedBy;
            }

            if (!string.IsNullOrEmpty(filter.CurrencyCode))
            {
                var currencyCode = filter.CurrencyCode;
                predicate = p => p.CurrencyCode == currencyCode;
            }

            if (filter.MinAmount.HasValue)
            {
                var minAmount = filter.MinAmount.Value;
                predicate = p => p.Amount >= minAmount;
            }

            if (filter.MaxAmount.HasValue)
            {
                var maxAmount = filter.MaxAmount.Value;
                predicate = p => p.Amount <= maxAmount;
            }

            return predicate;
        }
    }
}