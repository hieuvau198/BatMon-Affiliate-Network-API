using Application.Contracts.Campaign;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class CampaignMappingProfile : Profile
    {
        public CampaignMappingProfile()
        {
            // Entity to DTO
            CreateMap<Campaign, CampaignDto>()
                .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(src => src.Advertiser.CompanyName != null ? src.Advertiser.ContactName : null))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.CurrencyCode != null ? src.CurrencyCodeNavigation.CurrencyName : null));

            // DTO to Entity
            CreateMap<CampaignCreateDto, Campaign>()
                .ForMember(dest => dest.CampaignId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdated, opt => opt.Ignore())
                .ForMember(dest => dest.Advertiser, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignAdvertiserUrls, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignConversionTypes, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignPublisherCommissions, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCodeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore());

            CreateMap<CampaignUpdateDto, Campaign>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Advertiser, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserId, opt => opt.Ignore()) // Protect foreign key
                .ForMember(dest => dest.CampaignAdvertiserUrls, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignConversionTypes, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignPublisherCommissions, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCodeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore());
        }
    }
}
