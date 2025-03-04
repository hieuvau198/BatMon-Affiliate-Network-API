using Application.Contracts.Campaign;
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
    public class CampaignService : ICampaignService
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IMapper _mapper;

        public CampaignService(IGenericRepository<Campaign> campaignRepository, IMapper mapper)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignDto>> GetAllCampaignsAsync()
        {
            var campaigns = await _campaignRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<CampaignDto> GetCampaignByIdAsync(int id, bool includeRelated = false)
        {
            Campaign campaign;

            if (includeRelated)
            {
                campaign = await _campaignRepository.GetByIdAsync(id,
                    c => c.Advertiser,
                    c => c.CampaignAdvertiserUrls,
                    c => c.CampaignConversionTypes,
                    c => c.CampaignPublisherCommissions,
                    c => c.CurrencyCodeNavigation);
            }
            else
            {
                campaign = await _campaignRepository.GetByIdAsync(id);
            }

            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            return _mapper.Map<CampaignDto>(campaign);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByAdvertiserIdAsync(int advertiserId)
        {
            var campaigns = await _campaignRepository.GetAllAsync(c => c.AdvertiserId == advertiserId);
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto campaignDto)
        {
            var campaign = _mapper.Map<Campaign>(campaignDto);

            // Set default values
            campaign.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            campaign.Status = "Draft"; // Default status

            var createdCampaign = await _campaignRepository.CreateAsync(campaign);
            return _mapper.Map<CampaignDto>(createdCampaign);
        }

        public async Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto campaignDto)
        {
            var existingCampaign = await _campaignRepository.GetByIdAsync(campaignDto.CampaignId);
            if (existingCampaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {campaignDto.CampaignId} not found");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(campaignDto, existingCampaign);

            // Always update the LastUpdated timestamp
            existingCampaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _campaignRepository.UpdateAsync(existingCampaign);
            return _mapper.Map<CampaignDto>(existingCampaign);
        }

        public async Task DeleteCampaignAsync(int id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            await _campaignRepository.DeleteAsync(campaign);
        }

        public async Task<bool> ActivateCampaignAsync(int id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            campaign.Status = "Active";
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            await _campaignRepository.UpdateAsync(campaign);
            return true;
        }

        public async Task<bool> DeactivateCampaignAsync(int id)
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            campaign.Status = "Inactive";
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            await _campaignRepository.UpdateAsync(campaign);
            return true;
        }

        public async Task<bool> CampaignExistsAsync(int id)
        {
            return await _campaignRepository.ExistsAsync(c => c.CampaignId == id);
        }

        public async Task<int> GetCampaignCountAsync()
        {
            return await _campaignRepository.CountAsync(c => true);
        }

        public async Task<int> GetCampaignCountByAdvertiserAsync(int advertiserId)
        {
            return await _campaignRepository.CountAsync(c => c.AdvertiserId == advertiserId);
        }

        public async Task<decimal> GetTotalBudgetByAdvertiserAsync(int advertiserId)
        {
            var campaigns = await _campaignRepository.GetAllAsync(c => c.AdvertiserId == advertiserId);
            return campaigns.Sum(c => c.Budget ?? 0);
        }

        public async Task<IEnumerable<CampaignDto>> GetActiveCampaignsAsync()
        {
            var campaigns = await _campaignRepository.GetAllAsync(c => c.Status == "Active");
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByStatusAsync(string status)
        {
            var campaigns = await _campaignRepository.GetAllAsync(c => c.Status == status);
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }
    }
}
