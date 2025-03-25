using Application.Contracts.CampaignPublisherCommission;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CampaignPublisherCommissionService : ICampaignPublisherCommissionService
    {
        private readonly IGenericRepository<CampaignPublisherCommission> _commissionRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public CampaignPublisherCommissionService(
            IGenericRepository<CampaignPublisherCommission> commissionRepository,
            IGenericRepository<Publisher> publisherRepository,
            IGenericRepository<Campaign> campaignRepository,
            IGenericRepository<Currency> currencyRepository,
            IMapper mapper)
        {
            _commissionRepository = commissionRepository ?? throw new ArgumentNullException(nameof(commissionRepository));
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignPublisherCommissionDto>> GetAllCommissionsAsync()
        {
            var commissions = await _commissionRepository.GetAllAsync(
                includes: new Expression<Func<CampaignPublisherCommission, object>>[] {
                    c => c.Publisher!,
                    c => c.Campaign!,
                    c => c.CurrencyCodeNavigation
                });
            
            return _mapper.Map<IEnumerable<CampaignPublisherCommissionDto>>(commissions);
        }

        public async Task<CampaignPublisherCommissionDto> GetCommissionByIdAsync(int id)
        {
            var commission = await _commissionRepository.GetByIdAsync(
                id,
                c => c.Publisher!,
                c => c.Campaign!,
                c => c.CurrencyCodeNavigation);
            
            if (commission == null)
                throw new KeyNotFoundException($"Commission with ID {id} not found.");
            
            return _mapper.Map<CampaignPublisherCommissionDto>(commission);
        }

        public async Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByPublisherIdAsync(int publisherId)
        {
            var publisher = await _publisherRepository.FindAsync(p => p.PublisherId == publisherId);
            if (publisher == null)
                throw new KeyNotFoundException($"Publisher with ID {publisherId} not found.");
            
            var commissions = await _commissionRepository.GetAllAsync(
                c => c.PublisherId == publisherId,
                c => c.Campaign!,
                c => c.CurrencyCodeNavigation);
            
            return _mapper.Map<IEnumerable<CampaignPublisherCommissionDto>>(commissions);
        }

        public async Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByCampaignIdAsync(int campaignId)
        {
            var campaign = await _campaignRepository.FindAsync(c => c.CampaignId == campaignId);
            if (campaign == null)
                throw new KeyNotFoundException($"Campaign with ID {campaignId} not found.");
            
            var commissions = await _commissionRepository.GetAllAsync(
                c => c.CampaignId == campaignId,
                c => c.Publisher!,
                c => c.CurrencyCodeNavigation);
            
            return _mapper.Map<IEnumerable<CampaignPublisherCommissionDto>>(commissions);
        }

        public async Task<CampaignPublisherCommissionDto> CreateCommissionAsync(CampaignPublisherCommissionCreateDto commissionDto)
        {
            // Validate publisher
            if (commissionDto.PublisherId.HasValue)
            {
                var publisherExists = await _publisherRepository.ExistsAsync(p => p.PublisherId == commissionDto.PublisherId);
                if (!publisherExists)
                    throw new KeyNotFoundException($"Publisher with ID {commissionDto.PublisherId} not found.");
            }
            
            // Validate campaign
            if (commissionDto.CampaignId.HasValue)
            {
                var campaignExists = await _campaignRepository.ExistsAsync(c => c.CampaignId == commissionDto.CampaignId);
                if (!campaignExists)
                    throw new KeyNotFoundException($"Campaign with ID {commissionDto.CampaignId} not found.");
            }
            
            // Validate currency
            var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == commissionDto.CurrencyCode);
            if (!currencyExists)
                throw new KeyNotFoundException($"Currency with code {commissionDto.CurrencyCode} not found.");
            
            var commission = _mapper.Map<CampaignPublisherCommission>(commissionDto);
            var createdCommission = await _commissionRepository.CreateAsync(commission);
            
            return _mapper.Map<CampaignPublisherCommissionDto>(createdCommission);
        }

        public async Task<CampaignPublisherCommissionDto> UpdateCommissionAsync(CampaignPublisherCommissionUpdateDto commissionDto)
        {
            var commission = await _commissionRepository.GetByIdAsync(commissionDto.CommissionId);
            if (commission == null)
                throw new KeyNotFoundException($"Commission with ID {commissionDto.CommissionId} not found.");
            
            // Validate publisher
            if (commissionDto.PublisherId.HasValue)
            {
                var publisherExists = await _publisherRepository.ExistsAsync(p => p.PublisherId == commissionDto.PublisherId);
                if (!publisherExists)
                    throw new KeyNotFoundException($"Publisher with ID {commissionDto.PublisherId} not found.");
            }
            
            // Validate campaign
            if (commissionDto.CampaignId.HasValue)
            {
                var campaignExists = await _campaignRepository.ExistsAsync(c => c.CampaignId == commissionDto.CampaignId);
                if (!campaignExists)
                    throw new KeyNotFoundException($"Campaign with ID {commissionDto.CampaignId} not found.");
            }
            
            // Validate currency
            var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == commissionDto.CurrencyCode);
            if (!currencyExists)
                throw new KeyNotFoundException($"Currency with code {commissionDto.CurrencyCode} not found.");
            
            _mapper.Map(commissionDto, commission);
            await _commissionRepository.UpdateAsync(commission);
            
            // Get updated entity with related data
            var updatedCommission = await _commissionRepository.GetByIdAsync(
                commissionDto.CommissionId,
                c => c.Publisher!,
                c => c.Campaign!,
                c => c.CurrencyCodeNavigation);
            
            return _mapper.Map<CampaignPublisherCommissionDto>(updatedCommission);
        }

        public async Task DeleteCommissionAsync(int id)
        {
            var commission = await _commissionRepository.GetByIdAsync(id);
            if (commission == null)
                throw new KeyNotFoundException($"Commission with ID {id} not found.");
            
            await _commissionRepository.DeleteAsync(commission);
        }

        public async Task<bool> CommissionExistsAsync(int id)
        {
            return await _commissionRepository.ExistsAsync(c => c.CommissionId == id);
        }

        public async Task<decimal> GetTotalPendingCommissionsByPublisherAsync(int publisherId, string currencyCode)
        {
            var publisher = await _publisherRepository.FindAsync(p => p.PublisherId == publisherId);
            if (publisher == null)
                throw new KeyNotFoundException($"Publisher with ID {publisherId} not found.");
            
            var commissions = await _commissionRepository.GetAllAsync(
                c => c.PublisherId == publisherId && c.CurrencyCode == currencyCode);
            
            return commissions.Sum(c => c.PendingAmount ?? 0);
        }

        public async Task<decimal> GetTotalApprovedCommissionsByPublisherAsync(int publisherId, string currencyCode)
        {
            var publisher = await _publisherRepository.FindAsync(p => p.PublisherId == publisherId);
            if (publisher == null)
                throw new KeyNotFoundException($"Publisher with ID {publisherId} not found.");
            
            var commissions = await _commissionRepository.GetAllAsync(
                c => c.PublisherId == publisherId && c.CurrencyCode == currencyCode);
            
            return commissions.Sum(c => c.ApprovedAmount ?? 0);
        }

        public async Task<IEnumerable<CampaignPublisherCommissionDto>> GetCommissionsByStatusAsync(string status)
        {
            var commissions = await _commissionRepository.GetAllAsync(
                c => c.CommissionStatus == status,
                c => c.Publisher!,
                c => c.Campaign!,
                c => c.CurrencyCodeNavigation);
            
            return _mapper.Map<IEnumerable<CampaignPublisherCommissionDto>>(commissions);
        }

        public async Task<CampaignPublisherCommissionDto> UpdateCommissionStatusAsync(int id, string status)
        {
            var commission = await _commissionRepository.GetByIdAsync(id);
            if (commission == null)
                throw new KeyNotFoundException($"Commission with ID {id} not found.");
            
            commission.CommissionStatus = status;
            commission.UpdatedAt = DateTime.UtcNow;
            
            await _commissionRepository.UpdateAsync(commission);
            
            // Get updated entity with related data
            var updatedCommission = await _commissionRepository.GetByIdAsync(
                id,
                c => c.Publisher!,
                c => c.Campaign!,
                c => c.CurrencyCodeNavigation);
            
            return _mapper.Map<CampaignPublisherCommissionDto>(updatedCommission);
        }
    }
}