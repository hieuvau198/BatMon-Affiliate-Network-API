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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignDto>> GetAllCampaignsAsync()
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync();
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<CampaignDto> GetCampaignByIdAsync(int id, bool includeRelated = false)
        {
            Campaign campaign;

            if (includeRelated)
            {
                campaign = await _unitOfWork.Campaigns.GetByIdAsync(id,
                    c => c.Advertiser,
                    c => c.CampaignAdvertiserUrls,
                    c => c.CampaignConversionTypes,
                    c => c.CampaignPublisherCommissions);
            }
            else
            {
                campaign = await _unitOfWork.Campaigns.GetByIdAsync(id);
            }

            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            return _mapper.Map<CampaignDto>(campaign);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByAdvertiserIdAsync(int advertiserId)
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync(c => c.AdvertiserId == advertiserId);
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto campaignDto)
        {
            var campaign = _mapper.Map<Campaign>(campaignDto);

            // Set default values
            campaign.CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            campaign.Status = "Pending"; // Default status

            var createdCampaign = await _unitOfWork.Campaigns.CreateAsync(campaign);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved

            return _mapper.Map<CampaignDto>(createdCampaign);
        }

        public async Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto campaignDto)
        {
            var existingCampaign = await _unitOfWork.Campaigns.GetByIdAsync(campaignDto.CampaignId);
            if (existingCampaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {campaignDto.CampaignId} not found");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(campaignDto, existingCampaign);

            // Always update the LastUpdated timestamp
            existingCampaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.Campaigns.UpdateAsync(existingCampaign);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved

            return _mapper.Map<CampaignDto>(existingCampaign);
        }

        public async Task DeleteCampaignAsync(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            await _unitOfWork.Campaigns.DeleteAsync(campaign);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved
        }

        public async Task<bool> ActivateCampaignAsync(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            campaign.Status = "Active";
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            await _unitOfWork.Campaigns.UpdateAsync(campaign);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved

            return true;
        }

        public async Task<bool> DeactivateCampaignAsync(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetByIdAsync(id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {id} not found");
            }

            campaign.Status = "Inactive";
            campaign.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            await _unitOfWork.Campaigns.UpdateAsync(campaign);
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are saved

            return true;
        }

        public async Task<bool> CampaignExistsAsync(int id)
        {
            return await _unitOfWork.Campaigns.ExistsAsync(c => c.CampaignId == id);
        }

        public async Task<int> GetCampaignCountAsync()
        {
            return await _unitOfWork.Campaigns.CountAsync(c => true);
        }

        public async Task<int> GetCampaignCountByAdvertiserAsync(int advertiserId)
        {
            return await _unitOfWork.Campaigns.CountAsync(c => c.AdvertiserId == advertiserId);
        }

        public async Task<decimal> GetTotalBudgetByAdvertiserAsync(int advertiserId)
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync(c => c.AdvertiserId == advertiserId);
            return campaigns.Sum(c => c.Budget ?? 0);
        }

        public async Task<IEnumerable<CampaignDto>> GetActiveCampaignsAsync()
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync(c => c.Status == "Active");
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<IEnumerable<CampaignDto>> GetCampaignsByStatusAsync(string status)
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync(c => c.Status == status);
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<IEnumerable<CampaignDto>> GetPendingCampaignsAsync()
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllAsync(c => c.Status == "Pending");
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }
    }
}
