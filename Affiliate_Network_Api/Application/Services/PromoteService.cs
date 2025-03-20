using Application.Interfaces;
using Application.Contracts.Promote;
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
    public class PromoteService : IPromoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PromoteDto>> GetAllPromotesAsync()
        {
            var promotes = await _unitOfWork.Promotes.GetAllAsync(
                null,
                p => p.Publisher,
                p => p.Campaign
            );
            return _mapper.Map<IEnumerable<PromoteDto>>(promotes);
        }

        public async Task<PromoteDto> GetPromoteByIdAsync(int id, bool includeRelated = false)
        {
            Promote? promote;

            if (includeRelated)
            {
                promote = await _unitOfWork.Promotes.GetByIdAsync(
                    id,
                    p => p.Publisher,
                    p => p.Campaign,
                    p => p.CampaignAdvertiserUrl
                );
            }
            else
            {
                promote = await _unitOfWork.Promotes.GetByIdAsync(id);
            }

            if (promote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {id} not found");
            }

            return _mapper.Map<PromoteDto>(promote);
        }

        public async Task<IEnumerable<PromoteDto>> GetPromotesByPublisherIdAsync(int publisherId)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(publisherId);
            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {publisherId} not found");
            }

            var promotes = await _unitOfWork.Promotes.GetAllAsync(
                p => p.PublisherId == publisherId,
                p => p.Campaign
            );

            return _mapper.Map<IEnumerable<PromoteDto>>(promotes);
        }

        public async Task<IEnumerable<PromoteDto>> GetPromotesByCampaignIdAsync(int campaignId)
        {
            var campaign = await _unitOfWork.Campaigns.GetByIdAsync(campaignId);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {campaignId} not found");
            }

            var promotes = await _unitOfWork.Promotes.GetAllAsync(
                p => p.CampaignId == campaignId,
                p => p.Publisher
            );

            return _mapper.Map<IEnumerable<PromoteDto>>(promotes);
        }

        public async Task<PromoteDto> CreatePromoteAsync(PromoteCreateDto promoteDto)
        {
            // Validate publisher exists if ID is provided
            if (promoteDto.PublisherId.HasValue && !await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == promoteDto.PublisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID {promoteDto.PublisherId} not found");
            }

            // Validate campaign exists if ID is provided
            if (promoteDto.CampaignId.HasValue && !await _unitOfWork.Campaigns.ExistsAsync(c => c.CampaignId == promoteDto.CampaignId))
            {
                throw new KeyNotFoundException($"Campaign with ID {promoteDto.CampaignId} not found");
            }

            // Validate campaign advertiser URL exists if ID is provided
            if (promoteDto.CampaignAdvertiserUrlId.HasValue && !await _unitOfWork.CampaignAdvertiserUrls.ExistsAsync(cau => cau.CampaignUrlId == promoteDto.CampaignAdvertiserUrlId))
            {
                throw new KeyNotFoundException($"Campaign Advertiser URL with ID {promoteDto.CampaignAdvertiserUrlId} not found");
            }

            var promote = _mapper.Map<Promote>(promoteDto);

            // Set default values
            promote.JoinDate = DateOnly.FromDateTime(DateTime.UtcNow);
            promote.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
            promote.IsApproved = promoteDto.IsApproved ?? false;
            promote.Status = promoteDto.Status ?? "Pending";

            var createdPromote = await _unitOfWork.Promotes.CreateAsync(promote);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PromoteDto>(createdPromote);
        }

        public async Task<PromoteDto> UpdatePromoteAsync(PromoteUpdateDto promoteDto)
        {
            var existingPromote = await _unitOfWork.Promotes.GetByIdAsync(promoteDto.PromoteId);
            if (existingPromote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {promoteDto.PromoteId} not found");
            }

            // Validate publisher exists if ID is provided
            if (promoteDto.PublisherId.HasValue && !await _unitOfWork.Publishers.ExistsAsync(p => p.PublisherId == promoteDto.PublisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID {promoteDto.PublisherId} not found");
            }

            // Validate campaign exists if ID is provided
            if (promoteDto.CampaignId.HasValue && !await _unitOfWork.Campaigns.ExistsAsync(c => c.CampaignId == promoteDto.CampaignId))
            {
                throw new KeyNotFoundException($"Campaign with ID {promoteDto.CampaignId} not found");
            }

            // Validate campaign advertiser URL exists if ID is provided
            if (promoteDto.CampaignAdvertiserUrlId.HasValue && !await _unitOfWork.CampaignAdvertiserUrls.ExistsAsync(cau => cau.CampaignUrlId == promoteDto.CampaignAdvertiserUrlId))
            {
                throw new KeyNotFoundException($"Campaign Advertiser URL with ID {promoteDto.CampaignAdvertiserUrlId} not found");
            }

            _mapper.Map(promoteDto, existingPromote);
            existingPromote.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.Promotes.UpdateAsync(existingPromote);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PromoteDto>(existingPromote);
        }

        public async Task DeletePromoteAsync(int id)
        {
            var promote = await _unitOfWork.Promotes.GetByIdAsync(id);
            if (promote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {id} not found");
            }

            // Additional business rules can be added here
            // For example, checking if there are active conversions

            await _unitOfWork.Promotes.DeleteAsync(promote);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ApprovePromoteAsync(int id)
        {
            var promote = await _unitOfWork.Promotes.GetByIdAsync(id);
            if (promote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {id} not found");
            }

            promote.IsApproved = true;
            promote.Status = "Approved";
            promote.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.Promotes.UpdateAsync(promote);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectPromoteAsync(int id)
        {
            var promote = await _unitOfWork.Promotes.GetByIdAsync(id);
            if (promote == null)
            {
                throw new KeyNotFoundException($"Promote with ID {id} not found");
            }

            promote.IsApproved = false;
            promote.Status = "Rejected";
            promote.LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);

            await _unitOfWork.Promotes.UpdateAsync(promote);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> PromoteExistsAsync(int id)
        {
            return await _unitOfWork.Promotes.ExistsAsync(p => p.PromoteId == id);
        }
        public async Task<int> GetPromoteCountAsync()
        {
            return await _unitOfWork.Promotes.CountAsync(p => true);
        }

        public async Task<int> GetPromoteCountByPublisherAsync(int publisherId)
        {
            return await _unitOfWork.Promotes.CountAsync(p => p.PublisherId == publisherId);
        }

        public async Task<int> GetPromoteCountByCampaignAsync(int campaignId)
        {
            return await _unitOfWork.Promotes.CountAsync(p => p.CampaignId == campaignId);
        }
    }
}