using Application.Contracts.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICampaignService
    {
        Task<IEnumerable<CampaignDto>> GetAllCampaignsAsync();
        Task<CampaignDto> GetCampaignByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<CampaignDto>> GetCampaignsByAdvertiserIdAsync(int advertiserId);
        Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto campaignDto);
        Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto campaignDto);
        Task DeleteCampaignAsync(int id);
        Task<bool> ActivateCampaignAsync(int id);
        Task<bool> DeactivateCampaignAsync(int id);
        Task<bool> CampaignExistsAsync(int id);
        Task<int> GetCampaignCountAsync();
        Task<int> GetCampaignCountByAdvertiserAsync(int advertiserId);
        Task<decimal> GetTotalBudgetByAdvertiserAsync(int advertiserId);
        Task<IEnumerable<CampaignDto>> GetActiveCampaignsAsync();
        Task<IEnumerable<CampaignDto>> GetCampaignsByStatusAsync(string status);
        Task<IEnumerable<CampaignDto>> GetPendingCampaignsAsync();


    }
}
