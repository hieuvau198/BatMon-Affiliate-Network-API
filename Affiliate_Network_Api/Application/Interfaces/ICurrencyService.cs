using Application.Contracts.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync();
        Task<CurrencyDto> GetCurrencyByCodeAsync(string currencyCode);
        Task<CurrencyDto> CreateCurrencyAsync(CreateCurrencyDto currencyDto);
        Task<CurrencyDto> UpdateCurrencyAsync(string currencyCode, UpdateCurrencyDto currencyDto);
        Task<bool> DeleteCurrencyAsync(string currencyCode);
        Task<bool> CurrencyExistsAsync(string currencyCode);
    }
}
