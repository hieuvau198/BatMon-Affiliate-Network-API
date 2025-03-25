using Application.Contracts.CampaignPublisherCommission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICampaignPublisherCommissionService
    {
        Task<IEnumerable<CampaignPublisherCommissionDto>> GetAllCommissionsAsync();
        Task<CampaignPublisherCommissionDto> GetCommissionByIdAsync(int id);
        Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByPublisherIdAsync(int publisherId);
        Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByCampaignIdAsync(int campaignId);
        Task<CampaignPublisherCommissionDto> CreateCommissionAsync(CampaignPublisherCommissionCreateDto commissionDto);
        Task<CampaignPublisherCommissionDto> UpdateCommissionAsync(CampaignPublisherCommissionUpdateDto commissionDto);
        Task DeleteCommissionAsync(int id);
        Task<bool> CommissionExistsAsync(int id);
        Task<decimal> GetTotalPendingCommissionsByPublisherAsync(int publisherId, string currencyCode);
        Task<decimal> GetTotalApprovedCommissionsByPublisherAsync(int publisherId, string currencyCode);
        Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByStatusAsync(string status);
        Task<CampaignPublisherCommissionDto> UpdateCommissionStatusAsync(int id, string status);
    }
}