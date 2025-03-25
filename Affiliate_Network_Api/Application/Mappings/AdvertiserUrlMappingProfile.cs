using Application.Contracts.AdvertiserUrl;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings
{
    public class AdvertiserUrlMappingProfile : Profile
    {
        public AdvertiserUrlMappingProfile()
        {
            // Map from Entity to DTO
            CreateMap<AdvertiserUrl, AdvertiserUrlDto>()
                .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(src =>
                    src.Advertiser != null ? src.Advertiser.CompanyName : null))
                .ForMember(dest => dest.CampaignCount, opt => opt.MapFrom(src =>
                    src.CampaignAdvertiserUrls != null ? src.CampaignAdvertiserUrls.Count : 0));

            // Map from Create DTO to Entity
            CreateMap<AdvertiserUrlCreateDto, AdvertiserUrl>();

            // Map from Update DTO to Entity
            CreateMap<AdvertiserUrlUpdateDto, AdvertiserUrl>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        }

    }
}