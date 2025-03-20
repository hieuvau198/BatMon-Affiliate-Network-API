using Application.Contracts.WithdrawalRequest;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WithdrawalRequestService : IWithdrawalRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WithdrawalRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<WithdrawalRequestDto>> GetAllWithdrawalRequestsAsync()
        {
            var withdrawalRequests = await _unitOfWork.WithdrawalRequests.GetAllAsync(
                null,
                wr => wr.Advertiser,
                wr => wr.CurrencyCodeNavigation
            );
            return _mapper.Map<IEnumerable<WithdrawalRequestDto>>(withdrawalRequests);
        }

        public async Task<WithdrawalRequestDto> GetWithdrawalRequestByIdAsync(int id)
        {
            var withdrawalRequest = await _unitOfWork.WithdrawalRequests.GetByIdAsync(
                id,
                wr => wr.Advertiser,
                wr => wr.CurrencyCodeNavigation
            );

            if (withdrawalRequest == null)
                throw new KeyNotFoundException($"Withdrawal request with ID {id} not found.");

            return _mapper.Map<WithdrawalRequestDto>(withdrawalRequest);
        }

        public async Task<IEnumerable<WithdrawalRequestDto>> GetWithdrawalRequestsByAdvertiserIdAsync(int advertiserId)
        {
            // First check if advertiser exists
            if (!await _unitOfWork.Advertisers.ExistsAsync(a => a.AdvertiserId == advertiserId))
                throw new KeyNotFoundException($"Advertiser with ID {advertiserId} not found.");

            var withdrawalRequests = await _unitOfWork.WithdrawalRequests.GetAllAsync(
                wr => wr.AdvertiserId == advertiserId,
                wr => wr.Advertiser,
                wr => wr.CurrencyCodeNavigation
            );

            return _mapper.Map<IEnumerable<WithdrawalRequestDto>>(withdrawalRequests);
        }

        public async Task<WithdrawalRequestDto> CreateWithdrawalRequestAsync(CreateWithdrawalRequestDto withdrawalRequestDto)
        {
            // Validate advertiser exists
            var advertiser = await _unitOfWork.Advertisers.GetByIdAsync(withdrawalRequestDto.AdvertiserId);
            if (advertiser == null)
                throw new KeyNotFoundException($"Advertiser with ID {withdrawalRequestDto.AdvertiserId} not found.");

            // Validate currency exists
            if (!await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == withdrawalRequestDto.CurrencyCode))
                throw new KeyNotFoundException($"Currency with code {withdrawalRequestDto.CurrencyCode} not found.");

            // Check if advertiser has sufficient balance
            var advertiserBalance = await _unitOfWork.AdvertiserBalances.FindAsync(
                ab => ab.AdvertiserId == withdrawalRequestDto.AdvertiserId &&
                      ab.CurrencyCode == withdrawalRequestDto.CurrencyCode
            );

            if (advertiserBalance == null || advertiserBalance.AvailableBalance < withdrawalRequestDto.Amount)
                throw new InvalidOperationException("Insufficient balance for withdrawal.");

            var withdrawalRequest = _mapper.Map<WithdrawalRequest>(withdrawalRequestDto);

            var createdRequest = await _unitOfWork.WithdrawalRequests.CreateAsync(withdrawalRequest);
            await _unitOfWork.SaveChangesAsync();
            return await GetWithdrawalRequestByIdAsync(createdRequest.RequestId);
        }

        public async Task<WithdrawalRequestDto> UpdateWithdrawalRequestStatusAsync(int id, UpdateWithdrawalRequestDto updateDto)
        {
            var withdrawalRequest = await _unitOfWork.WithdrawalRequests.GetByIdAsync(id);
            if (withdrawalRequest == null)
                throw new KeyNotFoundException($"Withdrawal request with ID {id} not found.");

            if (withdrawalRequest.Status != "Pending")
                throw new InvalidOperationException($"Cannot update withdrawal request with status '{withdrawalRequest.Status}'.");

            _mapper.Map(updateDto, withdrawalRequest);
            
            await _unitOfWork.WithdrawalRequests.UpdateAsync(withdrawalRequest);
            await _unitOfWork.SaveChangesAsync();

            return await GetWithdrawalRequestByIdAsync(id);
        }

        public async Task<bool> DeleteWithdrawalRequestAsync(int id)
        {
            var withdrawalRequest = await _unitOfWork.WithdrawalRequests.GetByIdAsync(id);
            if (withdrawalRequest == null)
                return false;

            // Only allow deletion of pending requests
            if (withdrawalRequest.Status != "Pending")
                throw new InvalidOperationException($"Cannot delete withdrawal request with status '{withdrawalRequest.Status}'.");

            await _unitOfWork.WithdrawalRequests.DeleteAsync(withdrawalRequest);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WithdrawalRequestDto>> GetFilteredWithdrawalRequestsAsync(WithdrawalRequestFilterDto filterDto)
        {
            var withdrawalRequests = await _unitOfWork.WithdrawalRequests.GetAllAsync(
                wr => (filterDto.AdvertiserId == null || wr.AdvertiserId == filterDto.AdvertiserId) &&
                      (string.IsNullOrEmpty(filterDto.Status) || wr.Status == filterDto.Status) &&
                      (filterDto.FromDate == null || wr.RequestDate >= filterDto.FromDate) &&
                      (filterDto.ToDate == null || wr.RequestDate <= filterDto.ToDate) &&
                      (string.IsNullOrEmpty(filterDto.CurrencyCode) || wr.CurrencyCode == filterDto.CurrencyCode),
                wr => wr.Advertiser,
                wr => wr.CurrencyCodeNavigation
            );

            return _mapper.Map<IEnumerable<WithdrawalRequestDto>>(withdrawalRequests);
        }

        public async Task<bool> ApproveWithdrawalRequestAsync(int id, int reviewerId)
        {
            var withdrawalRequest = await _unitOfWork.WithdrawalRequests.GetByIdAsync(id);
            if (withdrawalRequest == null)
                throw new KeyNotFoundException($"Withdrawal request with ID {id} not found.");

            if (withdrawalRequest.Status != "Pending")
                throw new InvalidOperationException($"Cannot approve withdrawal request with status '{withdrawalRequest.Status}'.");
            withdrawalRequest.Status = "Approved";
            withdrawalRequest.ReviewedBy = reviewerId;
            var advertiserBalance = await _unitOfWork.AdvertiserBalances.FindAsync(
                ab => ab.AdvertiserId == withdrawalRequest.AdvertiserId &&
                      ab.CurrencyCode == withdrawalRequest.CurrencyCode
            );

            if (advertiserBalance == null || advertiserBalance.AvailableBalance < withdrawalRequest.Amount)
                throw new InvalidOperationException("Insufficient balance for withdrawal.");

            advertiserBalance.AvailableBalance -= withdrawalRequest.Amount;
            advertiserBalance.LifetimeWithdrawals = (advertiserBalance.LifetimeWithdrawals ?? 0) + withdrawalRequest.Amount;
            advertiserBalance.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.AdvertiserBalances.UpdateAsync(advertiserBalance);
            await _unitOfWork.WithdrawalRequests.UpdateAsync(withdrawalRequest);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RejectWithdrawalRequestAsync(int id, int reviewerId, string rejectionReason)
        {
            if (string.IsNullOrWhiteSpace(rejectionReason))
                throw new ArgumentException("Rejection reason cannot be empty.");

            var withdrawalRequest = await _unitOfWork.WithdrawalRequests.GetByIdAsync(id);
            if (withdrawalRequest == null)
                throw new KeyNotFoundException($"Withdrawal request with ID {id} not found.");

            if (withdrawalRequest.Status != "Pending")
                throw new InvalidOperationException($"Cannot reject withdrawal request with status '{withdrawalRequest.Status}'.");

            // Check if reviewer exists (assuming reviewer is an Admin)
            if (!await _unitOfWork.Admins.ExistsAsync(a => a.AdminId == reviewerId))
                throw new KeyNotFoundException($"Admin with ID {reviewerId} not found.");

            // Update withdrawal request status
            withdrawalRequest.Status = "Rejected";
            withdrawalRequest.ReviewedBy = reviewerId;
            withdrawalRequest.RejectionReason = rejectionReason;

            await _unitOfWork.WithdrawalRequests.UpdateAsync(withdrawalRequest);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> WithdrawalRequestExistsAsync(int id)
        {
            return await _unitOfWork.WithdrawalRequests.ExistsAsync(wr => wr.RequestId == id);
        }

        public async Task<decimal> GetTotalPendingWithdrawalAmountAsync(int advertiserId, string currencyCode)
        {
            var pendingWithdrawals = await _unitOfWork.WithdrawalRequests.GetAllAsync(
                wr => wr.AdvertiserId == advertiserId && 
                      wr.Status == "Pending" && 
                      wr.CurrencyCode == currencyCode
            );

            return pendingWithdrawals.Sum(wr => wr.Amount ?? 0);
        }

        public async Task<decimal> GetTotalWithdrawnAmountAsync(int advertiserId, string currencyCode)
        {
            var approvedWithdrawals = await _unitOfWork.WithdrawalRequests.GetAllAsync(
                wr => wr.AdvertiserId == advertiserId &&
                      wr.Status == "Approved" &&
                      wr.CurrencyCode == currencyCode
            );

            return approvedWithdrawals.Sum(wr => wr.Amount ?? 0);
        }
    }
}