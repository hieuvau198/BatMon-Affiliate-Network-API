using Application.Contracts.CampaignAdvertiserUrl;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CampaignAdvertiserUrlService : ICampaignAdvertiserUrlService
    {
        private readonly IGenericRepository<CampaignAdvertiserUrl> _campaignAdvertiserUrlRepository;
        private readonly IMapper _mapper;

        public CampaignAdvertiserUrlService(
            IGenericRepository<CampaignAdvertiserUrl> campaignAdvertiserUrlRepository,
            IMapper mapper)
        {
            _campaignAdvertiserUrlRepository = campaignAdvertiserUrlRepository ?? throw new ArgumentNullException(nameof(campaignAdvertiserUrlRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignAdvertiserUrlDto>> GetAllCampaignAdvertiserUrlsAsync()
        {
            var campaignAdvertiserUrls = await _campaignAdvertiserUrlRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CampaignAdvertiserUrlDto>>(campaignAdvertiserUrls);
        }

        public async Task<CampaignAdvertiserUrlDto> GetCampaignAdvertiserUrlByIdAsync(int id, bool includeRelated = false)
        {
            CampaignAdvertiserUrl? campaignAdvertiserUrl;

            if (includeRelated)
            {
                campaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(id,
                    c => c.Campaign,
                    c => c.AdvertiserUrl);
            }
            else
            {
                campaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(id);
            }

            if (campaignAdvertiserUrl == null)
                throw new KeyNotFoundException($"CampaignAdvertiserUrl with ID {id} not found.");

            return _mapper.Map<CampaignAdvertiserUrlDto>(campaignAdvertiserUrl);
        }

        public async Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByCampaignIdAsync(int campaignId)
        {
            var campaignAdvertiserUrls = await _campaignAdvertiserUrlRepository.GetAllAsync(
                c => c.CampaignId == campaignId);

            return _mapper.Map<IEnumerable<CampaignAdvertiserUrlDto>>(campaignAdvertiserUrls);
        }

        public async Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByAdvertiserUrlIdAsync(int advertiserUrlId)
        {
            var campaignAdvertiserUrls = await _campaignAdvertiserUrlRepository.GetAllAsync(
                c => c.AdvertiserUrlId == advertiserUrlId);

            return _mapper.Map<IEnumerable<CampaignAdvertiserUrlDto>>(campaignAdvertiserUrls);
        }

        public async Task<IEnumerable<CampaignAdvertiserUrlDto>> GetActiveCampaignAdvertiserUrlsAsync()
        {
            var campaignAdvertiserUrls = await _campaignAdvertiserUrlRepository.GetAllAsync(
                c => c.IsActive == true);

            return _mapper.Map<IEnumerable<CampaignAdvertiserUrlDto>>(campaignAdvertiserUrls);
        }

        public async Task<IEnumerable<CampaignAdvertiserUrlDto>> GetCampaignAdvertiserUrlsByStatusAsync(bool isActive)
        {
            var campaignAdvertiserUrls = await _campaignAdvertiserUrlRepository.GetAllAsync(
                c => c.IsActive == isActive);

            return _mapper.Map<IEnumerable<CampaignAdvertiserUrlDto>>(campaignAdvertiserUrls);
        }

        public async Task<int> GetCampaignAdvertiserUrlCountAsync()
        {
            return await _campaignAdvertiserUrlRepository.CountAsync(c => true);
        }

        public async Task<int> GetCampaignAdvertiserUrlCountByCampaignAsync(int campaignId)
        {
            return await _campaignAdvertiserUrlRepository.CountAsync(c => c.CampaignId == campaignId);
        }

        public async Task<int> GetCampaignAdvertiserUrlCountByAdvertiserUrlAsync(int advertiserUrlId)
        {
            return await _campaignAdvertiserUrlRepository.CountAsync(c => c.AdvertiserUrlId == advertiserUrlId);
        }

        public async Task<CampaignAdvertiserUrlDto> CreateCampaignAdvertiserUrlAsync(CampaignAdvertiserUrlCreateDto campaignAdvertiserUrlDto)
        {
            var campaignAdvertiserUrl = _mapper.Map<CampaignAdvertiserUrl>(campaignAdvertiserUrlDto);

            var createdCampaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.CreateAsync(campaignAdvertiserUrl);

            return _mapper.Map<CampaignAdvertiserUrlDto>(createdCampaignAdvertiserUrl);
        }

        public async Task<CampaignAdvertiserUrlDto> UpdateCampaignAdvertiserUrlAsync(CampaignAdvertiserUrlUpdateDto campaignAdvertiserUrlDto)
        {
            var existingCampaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(campaignAdvertiserUrlDto.CampaignUrlId);

            if (existingCampaignAdvertiserUrl == null)
                throw new KeyNotFoundException($"CampaignAdvertiserUrl with ID {campaignAdvertiserUrlDto.CampaignUrlId} not found.");

            _mapper.Map(campaignAdvertiserUrlDto, existingCampaignAdvertiserUrl);

            await _campaignAdvertiserUrlRepository.UpdateAsync(existingCampaignAdvertiserUrl);

            return _mapper.Map<CampaignAdvertiserUrlDto>(existingCampaignAdvertiserUrl);
        }

        public async Task DeleteCampaignAdvertiserUrlAsync(int id)
        {
            var campaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(id);

            if (campaignAdvertiserUrl == null)
                throw new KeyNotFoundException($"CampaignAdvertiserUrl with ID {id} not found.");

            await _campaignAdvertiserUrlRepository.DeleteAsync(campaignAdvertiserUrl);
        }

        public async Task<bool> ActivateCampaignAdvertiserUrlAsync(int id)
        {
            var campaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(id);

            if (campaignAdvertiserUrl == null)
                throw new KeyNotFoundException($"CampaignAdvertiserUrl with ID {id} not found.");

            campaignAdvertiserUrl.IsActive = true;

            await _campaignAdvertiserUrlRepository.UpdateAsync(campaignAdvertiserUrl);

            return true;
        }

        public async Task<bool> DeactivateCampaignAdvertiserUrlAsync(int id)
        {
            var campaignAdvertiserUrl = await _campaignAdvertiserUrlRepository.GetByIdAsync(id);

            if (campaignAdvertiserUrl == null)
                throw new KeyNotFoundException($"CampaignAdvertiserUrl with ID {id} not found.");

            campaignAdvertiserUrl.IsActive = false;

            await _campaignAdvertiserUrlRepository.UpdateAsync(campaignAdvertiserUrl);

            return true;
        }

        public async Task<bool> CampaignAdvertiserUrlExistsAsync(int id)
        {
            return await _campaignAdvertiserUrlRepository.ExistsAsync(c => c.CampaignUrlId == id);
        }
    }
}