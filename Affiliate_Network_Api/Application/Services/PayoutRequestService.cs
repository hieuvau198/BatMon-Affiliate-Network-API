using Application.Contracts.PayoutRequest;
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
    public class PayoutRequestService : IPayoutRequestService
    {
        private readonly IGenericRepository<PayoutRequest> _payoutRequestRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public PayoutRequestService(
            IGenericRepository<PayoutRequest> payoutRequestRepository,
            IGenericRepository<Publisher> publisherRepository,
            IGenericRepository<Currency> currencyRepository,
            IMapper mapper)
        {
            _payoutRequestRepository = payoutRequestRepository ?? throw new ArgumentNullException(nameof(payoutRequestRepository));
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PayoutRequestDto>> GetAllPayoutRequestsAsync(PayoutRequestFilterDto filter = null)
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

            var payoutRequests = await _payoutRequestRepository.GetAllAsync(predicate, includes);
            return _mapper.Map<IEnumerable<PayoutRequestDto>>(payoutRequests);
        }

        public async Task<PayoutRequestDto> GetPayoutRequestByIdAsync(int id)
        {
            var includes = new Expression<Func<PayoutRequest, object>>[]
            {
                p => p.Publisher,
                p => p.CurrencyCodeNavigation
            };

            var payoutRequest = await _payoutRequestRepository.GetByIdAsync(id, includes);

            if (payoutRequest == null)
                throw new KeyNotFoundException($"PayoutRequest with ID {id} not found");

            return _mapper.Map<PayoutRequestDto>(payoutRequest);
        }

        public async Task<IEnumerable<PayoutRequestDto>> GetPayoutRequestsByPublisherIdAsync(int publisherId)
        {
            var filter = new PayoutRequestFilterDto { PublisherId = publisherId };
            return await GetAllPayoutRequestsAsync(filter);
        }

        public async Task<IEnumerable<PayoutRequestDto>> GetPayoutRequestsByStatusAsync(string status)
        {
            var filter = new PayoutRequestFilterDto { Status = status };
            return await GetAllPayoutRequestsAsync(filter);
        }

        public async Task<int> CountPayoutRequestsAsync(PayoutRequestFilterDto filter = null)
        {
            Expression<Func<PayoutRequest, bool>> predicate = null;

            if (filter != null)
            {
                predicate = BuildFilterPredicate(filter);
            }

            return await _payoutRequestRepository.CountAsync(predicate);
        }

        public async Task<PayoutRequestDto> CreatePayoutRequestAsync(PayoutRequestCreateDto payoutRequestDto)
        {
            // Validate publisher
            if (payoutRequestDto.PublisherId.HasValue)
            {
                var publisherExists = await _publisherRepository.ExistsAsync(p => p.PublisherId == payoutRequestDto.PublisherId);
                if (!publisherExists)
                    throw new KeyNotFoundException($"Publisher with ID {payoutRequestDto.PublisherId} not found");
            }

            // Validate currency
            var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == payoutRequestDto.CurrencyCode);
            if (!currencyExists)
                throw new KeyNotFoundException($"Currency with Code {payoutRequestDto.CurrencyCode} not found");

            // Map DTO to entity
            var payoutRequest = _mapper.Map<PayoutRequest>(payoutRequestDto);

            // Set default values
            payoutRequest.RequestDate = DateOnly.FromDateTime(DateTime.Now);
            payoutRequest.Status = "Pending";

            // Create the entity
            var createdPayoutRequest = await _payoutRequestRepository.CreateAsync(payoutRequest);

            // Return the created entity
            return _mapper.Map<PayoutRequestDto>(createdPayoutRequest);
        }

        public async Task<PayoutRequestDto> UpdatePayoutRequestAsync(PayoutRequestUpdateDto payoutRequestDto)
        {
            var includes = new Expression<Func<PayoutRequest, object>>[]
            {
                p => p.Publisher,
                p => p.CurrencyCodeNavigation
            };

            var existingPayoutRequest = await _payoutRequestRepository.GetByIdAsync(payoutRequestDto.RequestId, includes);

            if (existingPayoutRequest == null)
                throw new KeyNotFoundException($"PayoutRequest with ID {payoutRequestDto.RequestId} not found");

            // Update only the fields that can be updated
            _mapper.Map(payoutRequestDto, existingPayoutRequest);

            // Save the changes
            await _payoutRequestRepository.UpdateAsync(existingPayoutRequest);

            // Return the updated entity
            return _mapper.Map<PayoutRequestDto>(existingPayoutRequest);
        }

        public async Task<bool> DeletePayoutRequestAsync(int id)
        {
            var payoutRequest = await _payoutRequestRepository.GetByIdAsync(id);

            if (payoutRequest == null)
                return false;

            await _payoutRequestRepository.DeleteAsync(payoutRequest);
            return true;
        }

        public async Task<bool> ApprovePayoutRequestAsync(int id, int reviewerId)
        {
            var payoutRequest = await _payoutRequestRepository.GetByIdAsync(id);

            if (payoutRequest == null)
                return false;

            payoutRequest.Status = "Approved";
            payoutRequest.ReviewedBy = reviewerId;
            payoutRequest.RejectionReason = null;

            await _payoutRequestRepository.UpdateAsync(payoutRequest);
            return true;
        }

        public async Task<bool> RejectPayoutRequestAsync(int id, int reviewerId, string reason)
        {
            var payoutRequest = await _payoutRequestRepository.GetByIdAsync(id);

            if (payoutRequest == null)
                return false;

            payoutRequest.Status = "Rejected";
            payoutRequest.ReviewedBy = reviewerId;
            payoutRequest.RejectionReason = reason;

            await _payoutRequestRepository.UpdateAsync(payoutRequest);
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _payoutRequestRepository.ExistsAsync(p => p.RequestId == id);
        }

        public async Task<decimal> GetTotalPayoutAmountByPublisherAsync(int publisherId)
        {
            var payoutRequests = await _payoutRequestRepository.GetAllAsync(
                p => p.PublisherId == publisherId && p.Status == "Approved");

            return payoutRequests.Sum(p => p.Amount ?? 0);
        }

        public async Task<decimal> GetTotalPayoutAmountByStatusAsync(string status)
        {
            var payoutRequests = await _payoutRequestRepository.GetAllAsync(
                p => p.Status == status);

            return payoutRequests.Sum(p => p.Amount ?? 0);
        }

        private Expression<Func<PayoutRequest, bool>> BuildFilterPredicate(PayoutRequestFilterDto filter)
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