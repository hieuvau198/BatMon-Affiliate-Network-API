using Application.Contracts.Currency;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(IGenericRepository<Currency> currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CurrencyDto>>(currencies);
        }

        public async Task<CurrencyDto> GetCurrencyByCodeAsync(string currencyCode)
        {
            var currency = await _currencyRepository.FindAsync(c => c.CurrencyCode == currencyCode);

            if (currency == null)
                throw new KeyNotFoundException($"Currency with code {currencyCode} not found.");

            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<CurrencyDto> CreateCurrencyAsync(CreateCurrencyDto currencyDto)
        {
            // Check if currency code already exists
            if (await _currencyRepository.ExistsAsync(c => c.CurrencyCode == currencyDto.CurrencyCode))
                throw new InvalidOperationException($"Currency with code {currencyDto.CurrencyCode} already exists.");

            var currency = _mapper.Map<Currency>(currencyDto);
            var createdCurrency = await _currencyRepository.CreateAsync(currency);

            return _mapper.Map<CurrencyDto>(createdCurrency);
        }

        public async Task<CurrencyDto> UpdateCurrencyAsync(string currencyCode, UpdateCurrencyDto currencyDto)
        {
            var currency = await _currencyRepository.FindAsync(c => c.CurrencyCode == currencyCode);

            if (currency == null)
                throw new KeyNotFoundException($"Currency with code {currencyCode} not found.");

            // Only update the provided properties
            if (currencyDto.CurrencyName != null)
                currency.CurrencyName = currencyDto.CurrencyName;

            if (currencyDto.ExchangeRate.HasValue)
                currency.ExchangeRate = currencyDto.ExchangeRate;

            await _currencyRepository.UpdateAsync(currency);

            return _mapper.Map<CurrencyDto>(currency);
        }

        public async Task<bool> DeleteCurrencyAsync(string currencyCode)
        {
            var currency = await _currencyRepository.FindAsync(c => c.CurrencyCode == currencyCode);

            if (currency == null)
                return false;

            await _currencyRepository.DeleteAsync(currency);
            return true;
        }

        public async Task<bool> CurrencyExistsAsync(string currencyCode)
        {
            return await _currencyRepository.ExistsAsync(c => c.CurrencyCode == currencyCode);
        }
    }
}