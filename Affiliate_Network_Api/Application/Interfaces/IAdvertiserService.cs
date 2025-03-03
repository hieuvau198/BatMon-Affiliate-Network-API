using Application.Contracts.Advertiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdvertiserService
    {
        Task<IEnumerable<AdvertiserDto>> GetAllAdvertisersAsync();
        Task<AdvertiserDto> GetAdvertiserByIdAsync(int id, bool includeRelated = false);
        Task<AdvertiserDto> GetAdvertiserByEmailAsync(string email);
        Task<AdvertiserDto> CreateAdvertiserAsync(AdvertiserCreateDto advertiserDto);
        Task<AdvertiserDto> UpdateAdvertiserAsync(AdvertiserUpdateDto advertiserDto);
        Task DeleteAdvertiserAsync(int id);
        Task<bool> ActivateAdvertiserAsync(int id);
        Task<bool> DeactivateAdvertiserAsync(int id);
        Task<bool> AdvertiserExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<int> GetAdvertiserCountAsync();
    }
}
