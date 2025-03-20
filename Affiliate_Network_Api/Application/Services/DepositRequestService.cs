using Application.Contracts.DepositRequest;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepositRequestService : IDepositRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepositRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<DepositRequestDto>> GetAllDepositRequestsAsync()
        {
            var depositRequests = await _unitOfWork.DepositRequests.GetAllAsync(
                includes: new System.Linq.Expressions.Expression<Func<DepositRequest, object>>[]
                {
                    dr => dr.Advertiser,
                    dr => dr.CurrencyCodeNavigation
                });

            return _mapper.Map<IEnumerable<DepositRequestDto>>(depositRequests);
        }

        public async Task<DepositRequestDto> GetDepositRequestByIdAsync(int id)
        {
            var depositRequest = await _unitOfWork.DepositRequests.GetByIdAsync(id,
                dr => dr.Advertiser,
                dr => dr.CurrencyCodeNavigation);

            if (depositRequest == null)
            {
                throw new KeyNotFoundException($"DepositRequest with ID {id} not found");
            }

            return _mapper.Map<DepositRequestDto>(depositRequest);
        }

        public async Task<DepositRequestDto> CreateDepositRequestAsync(DepositRequestCreateDto depositRequestDto)
        {
            // Validation checks
            if (depositRequestDto.AdvertiserId.HasValue)
            {
                var advertiserExists = await _unitOfWork.Advertisers.ExistsAsync(a => a.AdvertiserId == depositRequestDto.AdvertiserId.Value);
                if (!advertiserExists)
                {
                    throw new ArgumentException($"Advertiser with ID {depositRequestDto.AdvertiserId} does not exist");
                }
            }
            else
            {
                throw new ArgumentException("AdvertiserId is required");
            }

            if (depositRequestDto.Amount.HasValue)
            {
                if (depositRequestDto.Amount <= 0)
                {
                    throw new ArgumentException("Amount must be greater than 0");
                }
            }
            else
            {
                throw new ArgumentException("Amount is required");
            }

            if (string.IsNullOrWhiteSpace(depositRequestDto.CurrencyCode))
            {
                throw new ArgumentException("CurrencyCode is required");
            }

            var currencyExists = await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == depositRequestDto.CurrencyCode);
            if (!currencyExists)
            {
                throw new ArgumentException($"Currency with code {depositRequestDto.CurrencyCode} does not exist");
            }

            if (!string.IsNullOrWhiteSpace(depositRequestDto.PaymentMethod))
            {
                var paymentMethodExists = await _unitOfWork.PaymentMethods.ExistsAsync(pm => pm.Name == depositRequestDto.PaymentMethod);
                if (!paymentMethodExists)
                {
                    throw new ArgumentException($"PaymentMethod '{depositRequestDto.PaymentMethod}' does not exist");
                }
            }

            if (!string.IsNullOrWhiteSpace(depositRequestDto.TransactionId))
            {
                var transactionExists = await _unitOfWork.DepositRequests.ExistsAsync(dr => dr.TransactionId == depositRequestDto.TransactionId);
                if (transactionExists)
                {
                    throw new ArgumentException($"TransactionId '{depositRequestDto.TransactionId}' is already in use");
                }
            }

            // Map DTO to entity
            var depositRequest = _mapper.Map<DepositRequest>(depositRequestDto);

            // Set default values
            depositRequest.RequestDate = depositRequest.RequestDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            depositRequest.Status = depositRequest.Status ?? "Pending";

            // Create the deposit request
            var createdDepositRequest = await _unitOfWork.DepositRequests.CreateAsync(depositRequest);
            await _unitOfWork.SaveChangesAsync();

            // Fetch the created deposit request with related data for response
            var result = await _unitOfWork.DepositRequests.GetByIdAsync(createdDepositRequest.RequestId,
                dr => dr.Advertiser,
                dr => dr.CurrencyCodeNavigation);

            return _mapper.Map<DepositRequestDto>(result);
        }

        public async Task<DepositRequestDto> UpdateDepositRequestAsync(DepositRequestUpdateDto depositRequestDto)
        {
            var existingDepositRequest = await _unitOfWork.DepositRequests.GetByIdAsync(depositRequestDto.RequestId,
                dr => dr.Advertiser,
                dr => dr.CurrencyCodeNavigation);

            if (existingDepositRequest == null)
            {
                throw new KeyNotFoundException($"DepositRequest with ID {depositRequestDto.RequestId} not found");
            }

            // Validation for status
            if (!string.IsNullOrEmpty(depositRequestDto.Status))
            {
                var validStatuses = new[] { "Pending", "Approved", "Rejected" };
                if (!validStatuses.Contains(depositRequestDto.Status))
                {
                    throw new ArgumentException($"Invalid status value. Valid statuses are: {string.Join(", ", validStatuses)}");
                }

                if (depositRequestDto.Status == "Approved" && string.IsNullOrWhiteSpace(depositRequestDto.TransactionId))
                {
                    throw new ArgumentException("TransactionId is required when status is set to Approved");
                }
            }

            if (!string.IsNullOrWhiteSpace(depositRequestDto.TransactionId))
            {
                var transactionExists = await _unitOfWork.DepositRequests.ExistsAsync(dr => dr.TransactionId == depositRequestDto.TransactionId && dr.RequestId != depositRequestDto.RequestId);
                if (transactionExists)
                {
                    throw new ArgumentException($"TransactionId '{depositRequestDto.TransactionId}' is already in use");
                }
            }

            // Map updated properties
            _mapper.Map(depositRequestDto, existingDepositRequest);

            await _unitOfWork.DepositRequests.UpdateAsync(existingDepositRequest);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DepositRequestDto>(existingDepositRequest);
        }

        public async Task DeleteDepositRequestAsync(int id)
        {
            var depositRequest = await _unitOfWork.DepositRequests.GetByIdAsync(id);
            if (depositRequest == null)
            {
                throw new KeyNotFoundException($"DepositRequest with ID {id} not found");
            }

            // Prevent deletion if the request is not in a "Pending" state
            if (depositRequest.Status != "Pending")
            {
                throw new InvalidOperationException($"Cannot delete DepositRequest with ID {id} because its status is '{depositRequest.Status}'. Only 'Pending' requests can be deleted.");
            }

            await _unitOfWork.DepositRequests.DeleteAsync(depositRequest);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}