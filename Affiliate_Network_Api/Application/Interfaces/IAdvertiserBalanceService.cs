using Application.Contracts.AdvertiserBalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdvertiserBalanceService
    {
        Task<IEnumerable<AdvertiserBalanceDto>> GetAllAdvertiserBalancesAsync();
        Task<AdvertiserBalanceDto> GetAdvertiserBalanceByIdAsync(int id);
        Task<IEnumerable<AdvertiserBalanceDto>> GetAdvertiserBalancesByAdvertiserIdAsync(int advertiserId);
        Task<AdvertiserBalanceDto> CreateAdvertiserBalanceAsync(AdvertiserBalanceCreateDto balanceDto);
        Task<AdvertiserBalanceDto> UpdateAdvertiserBalanceAsync(AdvertiserBalanceUpdateDto balanceDto);
        Task DeleteAdvertiserBalanceAsync(int id);
        Task<bool> AdvertiserBalanceExistsAsync(int id);
    }
}
