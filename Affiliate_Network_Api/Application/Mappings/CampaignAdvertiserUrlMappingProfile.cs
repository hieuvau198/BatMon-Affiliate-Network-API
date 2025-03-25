using Application.Contracts.CampaignAdvertiserUrl;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class CampaignAdvertiserUrlMappingProfile : Profile
    {
        public CampaignAdvertiserUrlMappingProfile()
        {
            CreateMap<CampaignAdvertiserUrl, CampaignAdvertiserUrlDto>()
                .ForMember(dest => dest.Campaign, opt => opt.MapFrom(src => src.Campaign))
                .ForMember(dest => dest.AdvertiserUrl, opt => opt.MapFrom(src => src.AdvertiserUrl));

            CreateMap<CampaignAdvertiserUrlCreateDto, CampaignAdvertiserUrl>()
                .ForMember(dest => dest.Campaign, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore());

            CreateMap<CampaignAdvertiserUrlUpdateDto, CampaignAdvertiserUrl>()
                .ForMember(dest => dest.Campaign, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore());

            CreateMap<Campaign, CampaignMinimalDto>();

            CreateMap<AdvertiserUrl, AdvertiserUrlMinimalDto>()
                .ForMember(dest => dest.UrlId, opt => opt.MapFrom(src => src.UrlId));


        }
    }
}
