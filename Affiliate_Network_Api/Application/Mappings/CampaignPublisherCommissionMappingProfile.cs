using Application.Contracts.CampaignPublisherCommission;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CampaignPublisherCommissionMappingProfile : Profile
    {
        public CampaignPublisherCommissionMappingProfile()
        {
            // Entity to DTO
            CreateMap<CampaignPublisherCommission, CampaignPublisherCommissionDto>()
                .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src => src.Campaign != null ? src.Campaign.Name : null))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Username : null))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.CurrencyCodeNavigation != null ? src.CurrencyCodeNavigation.CurrencyName : null));

            // Create DTO to Entity
            CreateMap<CampaignPublisherCommissionCreateDto, CampaignPublisherCommission>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Update DTO to Entity
            CreateMap<CampaignPublisherCommissionUpdateDto, CampaignPublisherCommission>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}