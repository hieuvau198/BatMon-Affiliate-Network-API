using Application.Contracts.CampaignAdvertiserUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICampaignAdvertiserUrlService
    {
        Task<IEnumerable<CampaignAdvertiserUrlDto>> GetAllCampaignAdvertiserUrlsAsync();
        Task<CampaignAdvertiserUrlDto> GetCampaignAdvertiserUrlByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByCampaignIdAsync(int campaignId);
        Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByAdvertiserUrlIdAsync(int advertiserUrlId);
        Task<IEnumerable<CampaignAdvertiserUrlDto>> GetActiveCampaignAdvertiserUrlsAsync();
        Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByStatusAsync(bool isActive);
        Task<int> GetCampaignAdvertiserUrlCountAsync();
        Task<int> GetCampaignAdvertiserUrlCountByCampaignAsync(int campaignId);
        Task<int> GetCampaignAdvertiserUrlCountByAdvertiserUrlAsync(int advertiserUrlId);
        Task<CampaignAdvertiserUrlDto> CreateCampaignAdvertiserUrlAsync(CampaignAdvertiserUrlCreateDto campaignAdvertiserUrlDto);
        Task<CampaignAdvertiserUrlDto> UpdateCampaignAdvertiserUrlAsync(CampaignAdvertiserUrlUpdateDto campaignAdvertiserUrlDto);
        Task DeleteCampaignAdvertiserUrlAsync(int id);
        Task<bool> ActivateCampaignAdvertiserUrlAsync(int id);
        Task<bool> DeactivateCampaignAdvertiserUrlAsync(int id);
        Task<bool> CampaignAdvertiserUrlExistsAsync(int id);
    }
}
