using Application.Contracts.AdvertiserUrl;
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
    public class AdvertiserUrlService : IAdvertiserUrlService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdvertiserUrlService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AdvertiserUrlDto>> GetAllAdvertiserUrlsAsync()
        {
            var advertiserUrls = await _unitOfWork.AdvertiserUrls.GetAllAsync();
            return _mapper.Map<IEnumerable<AdvertiserUrlDto>>(advertiserUrls);
        }

        public async Task<AdvertiserUrlDto> GetAdvertiserUrlByIdAsync(int id, bool includeRelated = false)
        {
            AdvertiserUrl advertiserUrl;

            if (includeRelated)
            {
                advertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(id,
                    u => u.Advertiser,
                    u => u.CampaignAdvertiserUrls);
            }
            else
            {
                advertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(id);
            }

            if (advertiserUrl == null)
            {
                throw new KeyNotFoundException($"AdvertiserUrl with ID {id} not found");
            }

            return _mapper.Map<AdvertiserUrlDto>(advertiserUrl);
        }

        public async Task<IEnumerable<AdvertiserUrlDto>> GetAdvertiserUrlsByAdvertiserIdAsync(int advertiserId)
        {
            var advertiserUrls = await _unitOfWork.AdvertiserUrls.GetAllAsync(u => u.AdvertiserId == advertiserId);
            return _mapper.Map<IEnumerable<AdvertiserUrlDto>>(advertiserUrls);
        }

        public async Task<AdvertiserUrlDto> CreateAdvertiserUrlAsync(AdvertiserUrlCreateDto advertiserUrlDto)
        {
            var advertiserUrl = _mapper.Map<AdvertiserUrl>(advertiserUrlDto);

            // Set default values
            advertiserUrl.AddedDate = DateOnly.FromDateTime(DateTime.UtcNow);
            advertiserUrl.IsActive = advertiserUrlDto.IsActive ?? true;

            var createdAdvertiserUrl = await _unitOfWork.AdvertiserUrls.CreateAsync(advertiserUrl);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserUrlDto>(createdAdvertiserUrl);
        }

        public async Task<AdvertiserUrlDto> UpdateAdvertiserUrlAsync(AdvertiserUrlUpdateDto advertiserUrlDto)
        {
            var existingAdvertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(advertiserUrlDto.UrlId);
            if (existingAdvertiserUrl == null)
            {
                throw new KeyNotFoundException($"AdvertiserUrl with ID {advertiserUrlDto.UrlId} not found");
            }

            // Map updated properties while preserving ones we don't want to change
            _mapper.Map(advertiserUrlDto, existingAdvertiserUrl);

            await _unitOfWork.AdvertiserUrls.UpdateAsync(existingAdvertiserUrl);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AdvertiserUrlDto>(existingAdvertiserUrl);
        }

        public async Task DeleteAdvertiserUrlAsync(int id)
        {
            var advertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(id);
            if (advertiserUrl == null)
            {
                throw new KeyNotFoundException($"AdvertiserUrl with ID {id} not found");
            }

            await _unitOfWork.AdvertiserUrls.DeleteAsync(advertiserUrl);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ActivateAdvertiserUrlAsync(int id)
        {
            var advertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(id);
            if (advertiserUrl == null)
            {
                throw new KeyNotFoundException($"AdvertiserUrl with ID {id} not found");
            }

            advertiserUrl.IsActive = true;
            await _unitOfWork.AdvertiserUrls.UpdateAsync(advertiserUrl);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAdvertiserUrlAsync(int id)
        {
            var advertiserUrl = await _unitOfWork.AdvertiserUrls.GetByIdAsync(id);
            if (advertiserUrl == null)
            {
                throw new KeyNotFoundException($"AdvertiserUrl with ID {id} not found");
            }

            advertiserUrl.IsActive = false;
            await _unitOfWork.AdvertiserUrls.UpdateAsync(advertiserUrl);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AdvertiserUrlExistsAsync(int id)
        {
            return await _unitOfWork.AdvertiserUrls.ExistsAsync(u => u.UrlId == id);
        }

        public async Task<int> GetAdvertiserUrlCountAsync()
        {
            return await _unitOfWork.AdvertiserUrls.CountAsync(u => true);
        }

        public async Task<int> GetAdvertiserUrlCountByAdvertiserAsync(int advertiserId)
        {
            return await _unitOfWork.AdvertiserUrls.CountAsync(u => u.AdvertiserId == advertiserId);
        }

        public async Task<IEnumerable<AdvertiserUrlDto>> GetActiveAdvertiserUrlsAsync()
        {
            var advertiserUrls = await _unitOfWork.AdvertiserUrls.GetAllAsync(u => u.IsActive == true);
            return _mapper.Map<IEnumerable<AdvertiserUrlDto>>(advertiserUrls);
        }

        public async Task<IEnumerable<AdvertiserUrlDto>> GetAdvertiserUrlsByStatusAsync(bool isActive)
        {
            var advertiserUrls = await _unitOfWork.AdvertiserUrls.GetAllAsync(u => u.IsActive == isActive);
            return _mapper.Map<IEnumerable<AdvertiserUrlDto>>(advertiserUrls);
        }
    }
}