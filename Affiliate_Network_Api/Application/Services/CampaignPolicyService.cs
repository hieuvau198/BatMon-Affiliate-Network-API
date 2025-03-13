using Application.Contracts.CampaignPolicy;
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
    public class CampaignPolicyService : ICampaignPolicyService
    {
        private readonly IGenericRepository<CampaignPolicy> _policyRepository;
        private readonly IMapper _mapper;

        public CampaignPolicyService(IGenericRepository<CampaignPolicy> policyRepository, IMapper mapper)
        {
            _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CampaignPolicyDto>> GetAllCampaignPoliciesAsync()
        {
            var policies = await _policyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CampaignPolicyDto>>(policies);
        }

        public async Task<CampaignPolicyDto> GetCampaignPolicyByIdAsync(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);

            if (policy == null)
            {
                throw new KeyNotFoundException($"Campaign Policy with ID {id} not found");
            }

            return _mapper.Map<CampaignPolicyDto>(policy);
        }

        public async Task<CampaignPolicyDto> CreateCampaignPolicyAsync(CampaignPolicyCreateDto policyDto)
        {
            var policy = _mapper.Map<CampaignPolicy>(policyDto);
            var createdPolicy = await _policyRepository.CreateAsync(policy);
            return _mapper.Map<CampaignPolicyDto>(createdPolicy);
        }

        public async Task<CampaignPolicyDto> UpdateCampaignPolicyAsync(CampaignPolicyUpdateDto policyDto)
        {
            var existingPolicy = await _policyRepository.GetByIdAsync(policyDto.PolicyId);

            if (existingPolicy == null)
            {
                throw new KeyNotFoundException($"Campaign Policy with ID {policyDto.PolicyId} not found");
            }

            _mapper.Map(policyDto, existingPolicy);
            await _policyRepository.UpdateAsync(existingPolicy);
            return _mapper.Map<CampaignPolicyDto>(existingPolicy);
        }

        public async Task DeleteCampaignPolicyAsync(int id)
        {
            var policy = await _policyRepository.GetByIdAsync(id);

            if (policy == null)
            {
                throw new KeyNotFoundException($"Campaign Policy with ID {id} not found");
            }

            await _policyRepository.DeleteAsync(policy);
        }

        public async Task<bool> CampaignPolicyExistsAsync(int id)
        {
            return await _policyRepository.ExistsAsync(p => p.PolicyId == id);
        }

        public async Task<int> GetCampaignPolicyCountAsync()
        {
            return await _policyRepository.CountAsync(p => true);
        }

        public async Task<IEnumerable<CampaignPolicyDto>> GetCampaignPoliciesByAppliedToAsync(string appliedTo)
        {
            var policies = await _policyRepository.GetAllAsync(p => p.AppliedTo == appliedTo);
            return _mapper.Map<IEnumerable<CampaignPolicyDto>>(policies);
        }
    }
}
