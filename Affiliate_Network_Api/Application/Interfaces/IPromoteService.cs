using System;
using Application.Contracts.Promote;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPromoteService
    {
        Task<IEnumerable<PromoteDto>> GetAllPromotesAsync();
        Task<PromoteDto> GetPromoteByIdAsync(int id, bool includeRelated = false);
        Task<IEnumerable<PromoteDto>> GetPromotesByPublisherIdAsync(int publisherId);
        Task<IEnumerable<PromoteDto>> GetPromotesByCampaignIdAsync(int campaignId);
        Task<PromoteDto> CreatePromoteAsync(PromoteCreateDto promoteDto);
        Task<PromoteDto> UpdatePromoteAsync(PromoteUpdateDto promoteDto);
        Task DeletePromoteAsync(int id);
        Task<bool> ApprovePromoteAsync(int id);
        Task<bool> RejectPromoteAsync(int id);
        Task<bool> PromoteExistsAsync(int id);
        Task<int> GetPromoteCountAsync();
        Task<int> GetPromoteCountByPublisherAsync(int publisherId);
        Task<int> GetPromoteCountByCampaignAsync(int campaignId);
    }
}
