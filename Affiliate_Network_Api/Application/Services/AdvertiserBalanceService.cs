// AdvertiserBalanceService.cs
using Application.Contracts.AdvertiserBalance;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdvertiserBalanceService : IAdvertiserBalanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdvertiserBalanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AdvertiserBalanceDto>> GetAllAdvertiserBalancesAsync()
        {
            var balances = await _unitOfWork.AdvertiserBalances.GetAllAsync();
            return _mapper.Map<IEnumerable<AdvertiserBalanceDto>>(balances);
        }

        public async Task<AdvertiserBalanceDto> GetAdvertiserBalanceByIdAsync(int id)
        {
            var balance = await _unitOfWork.AdvertiserBalances.GetByIdAsync(id);

            if (balance == null)
            {
                throw new KeyNotFoundException($"Advertiser Balance with ID {id} not found");
            }

            return _mapper.Map<AdvertiserBalanceDto>(balance);
        }

        public async Task<IEnumerable<AdvertiserBalanceDto>> GetAdvertiserBalancesByAdvertiserIdAsync(int advertiserId)
        {
            var balances = await _unitOfWork.AdvertiserBalances.GetAllAsync(b => b.AdvertiserId == advertiserId);
            return _mapper.Map<IEnumerable<AdvertiserBalanceDto>>(balances);
        }

        public async Task<AdvertiserBalanceDto> CreateAdvertiserBalanceAsync(AdvertiserBalanceCreateDto balanceDto)
        {
            // Check if advertiser exists
            if (!await _unitOfWork.Advertisers.ExistsAsync(a => a.AdvertiserId == balanceDto.AdvertiserId))
            {
                throw new KeyNotFoundException($"Advertiser with ID {balanceDto.AdvertiserId} not found");
            }

            // Check if currency exists
            if (!await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == balanceDto.CurrencyCode))
            {
                throw new KeyNotFoundException($"Currency with code {balanceDto.CurrencyCode} not found");
            }

            // Check if balance already exists for this advertiser and currency
            if (await _unitOfWork.AdvertiserBalances.ExistsAsync(
                b => b.AdvertiserId == balanceDto.AdvertiserId && b.CurrencyCode == balanceDto.CurrencyCode))
            {
                throw new InvalidOperationException(
                    $"Balance for Advertiser ID {balanceDto.AdvertiserId} with currency {balanceDto.CurrencyCode} already exists");
            }

            var balance = _mapper.Map<AdvertiserBalance>(balanceDto);

            // Set default values
            balance.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            balance.LifetimeDeposits = 0;
            balance.LifetimeWithdrawals = 0;
            balance.LifetimeSpend = 0;

            var createdBalance = await _unitOfWork.AdvertiserBalances.CreateAsync(balance);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserBalanceDto>(createdBalance);
        }

        public async Task<AdvertiserBalanceDto> UpdateAdvertiserBalanceAsync(AdvertiserBalanceUpdateDto balanceDto)
        {
            var existingBalance = await _unitOfWork.AdvertiserBalances.GetByIdAsync(balanceDto.BalanceId);
            if (existingBalance == null)
            {
                throw new KeyNotFoundException($"Advertiser Balance with ID {balanceDto.BalanceId} not found");
            }

            // Check if currency exists if it's being changed
            if (balanceDto.CurrencyCode != null &&
                balanceDto.CurrencyCode != existingBalance.CurrencyCode &&
                !await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == balanceDto.CurrencyCode))
            {
                throw new KeyNotFoundException($"Currency with code {balanceDto.CurrencyCode} not found");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(balanceDto, existingBalance);

            // Always update the LastUpdated timestamp
            existingBalance.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.AdvertiserBalances.UpdateAsync(existingBalance);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserBalanceDto>(existingBalance);
        }

        public async Task DeleteAdvertiserBalanceAsync(int id)
        {
            var balance = await _unitOfWork.AdvertiserBalances.GetByIdAsync(id);
            if (balance == null)
            {
                throw new KeyNotFoundException($"Advertiser Balance with ID {id} not found");
            }

            await _unitOfWork.AdvertiserBalances.DeleteAsync(balance);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> AdvertiserBalanceExistsAsync(int id)
        {
            return await _unitOfWork.AdvertiserBalances.ExistsAsync(b => b.BalanceId == id);
        }
    }
}