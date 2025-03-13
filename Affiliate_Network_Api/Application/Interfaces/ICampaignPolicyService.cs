using Application.Contracts.CampaignPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICampaignPolicyService
    {
        Task<IEnumerable<CampaignPolicyDto>> GetAllCampaignPoliciesAsync();
        Task<CampaignPolicyDto> GetCampaignPolicyByIdAsync(int id);
        Task<CampaignPolicyDto> CreateCampaignPolicyAsync(CampaignPolicyCreateDto policyDto);
        Task<CampaignPolicyDto> UpdateCampaignPolicyAsync(CampaignPolicyUpdateDto policyDto);
        Task DeleteCampaignPolicyAsync(int id);
        Task<bool> CampaignPolicyExistsAsync(int id);
        Task<int> GetCampaignPolicyCountAsync();
        Task<IEnumerable<CampaignPolicyDto>> GetCampaignPoliciesByAppliedToAsync(string appliedTo);
    }
}
