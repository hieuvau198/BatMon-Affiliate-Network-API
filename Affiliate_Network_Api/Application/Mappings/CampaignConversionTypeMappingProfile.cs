using AutoMapper;
using Application.Contracts.CampaignConversionType;
using Domain.Entities;

namespace Application.Mappings
{
    public class CampaignConversionTypeMappingProfile : Profile
    {
        public CampaignConversionTypeMappingProfile()
        {
            CreateMap<CampaignConversionType, CampaignConversionTypeDto>()
                .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src => src.Campaign != null ? src.Campaign.Name : null))
                .ForMember(dest => dest.ConversionTypeName, opt => opt.MapFrom(src => src.ConversionType != null ? src.ConversionType.Name : null))
                .ForMember(dest => dest.TrackingMethod, opt => opt.MapFrom(src => src.ConversionType != null ? src.ConversionType.TrackingMethod : null));

            CreateMap<CampaignConversionTypeCreateDto, CampaignConversionType>();

            CreateMap<CampaignConversionTypeUpdateDto, CampaignConversionType>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}