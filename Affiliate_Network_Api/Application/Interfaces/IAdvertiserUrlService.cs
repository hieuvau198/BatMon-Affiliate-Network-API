using Application.Contracts.AdvertiserUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdvertiserUrlService
    {
        Task<IEnumerable<AdvertiserUrlDto>> GetAllAdvertiserUrlsAsync();
        Task<AdvertiserUrlDto> GetAdvertiserUrlByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<AdvertiserUrlDto>> GetAdvertiserUrlsByAdvertiserIdAsync(int advertiserId);
        Task<AdvertiserUrlDto> CreateAdvertiserUrlAsync(AdvertiserUrlCreateDto advertiserUrlDto);
        Task<AdvertiserUrlDto> UpdateAdvertiserUrlAsync(AdvertiserUrlUpdateDto advertiserUrlDto);
        Task DeleteAdvertiserUrlAsync(int id);
        Task<bool> ActivateAdvertiserUrlAsync(int id);
        Task<bool> DeactivateAdvertiserUrlAsync(int id);
        Task<bool> AdvertiserUrlExistsAsync(int id);
        Task<int> GetAdvertiserUrlCountAsync();
        Task<int> GetAdvertiserUrlCountByAdvertiserAsync(int advertiserId);
        Task<IEnumerable<AdvertiserUrlDto>> GetActiveAdvertiserUrlsAsync();
        Task<IEnumerable<AdvertiserUrlDto>> GetAdvertiserUrlsByStatusAsync(bool isActive);
    }
}
