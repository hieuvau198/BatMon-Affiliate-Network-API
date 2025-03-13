using Application.Contracts.CampaignPolicy;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class CampaignPolicyMappingProfile : Profile
    {
        public CampaignPolicyMappingProfile()
        {
            // Map from entity to DTO
            CreateMap<CampaignPolicy, CampaignPolicyDto>();

            // Map from create DTO to entity
            CreateMap<CampaignPolicyCreateDto, CampaignPolicy>();

            // Map from update DTO to entity
            CreateMap<CampaignPolicyUpdateDto, CampaignPolicy>();
        }
    }

}
