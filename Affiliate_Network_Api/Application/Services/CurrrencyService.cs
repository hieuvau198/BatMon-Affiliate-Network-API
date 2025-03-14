using Application.Contracts.Currency;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
        {
            var currencies = await _unitOfWork.Currencies.GetAllAsync();
            return _mapper.Map<IEnumerable<CurrencyDto>>(currencies);
        }

        public async Task<CurrencyDto> GetCurrencyByCodeAsync(string currencyCode)
        {
            var currency = await _unitOfWork.Currencies.FindAsync(c => c.CurrencyCode == currencyCode);
            if (currency == null)
                throw new KeyNotFoundException($"Currency with code {currencyCode} not found.");

            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<CurrencyDto> CreateCurrencyAsync(CreateCurrencyDto currencyDto)
        {
            // Check if currency code already exists
            if (await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == currencyDto.CurrencyCode))
                throw new InvalidOperationException($"Currency with code {currencyDto.CurrencyCode} already exists.");

            var currency = _mapper.Map<Currency>(currencyDto);
            var createdCurrency = await _unitOfWork.Currencies.CreateAsync(currency);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CurrencyDto>(createdCurrency);
        }

        public async Task<CurrencyDto> UpdateCurrencyAsync(string currencyCode, UpdateCurrencyDto currencyDto)
        {
            var currency = await _unitOfWork.Currencies.FindAsync(c => c.CurrencyCode == currencyCode);
            if (currency == null)
                throw new KeyNotFoundException($"Currency with code {currencyCode} not found.");

            // Only update the provided properties
            if (currencyDto.CurrencyName != null)
                currency.CurrencyName = currencyDto.CurrencyName;

            if (currencyDto.ExchangeRate.HasValue)
                currency.ExchangeRate = currencyDto.ExchangeRate;

            await _unitOfWork.Currencies.UpdateAsync(currency);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<bool> DeleteCurrencyAsync(string currencyCode)
        {
            var currency = await _unitOfWork.Currencies.FindAsync(c => c.CurrencyCode == currencyCode);
            if (currency == null)
                return false;

            await _unitOfWork.Currencies.DeleteAsync(currency);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CurrencyExistsAsync(string currencyCode)
        {
            return await _unitOfWork.Currencies.ExistsAsync(c => c.CurrencyCode == currencyCode);
        }
    }
}
