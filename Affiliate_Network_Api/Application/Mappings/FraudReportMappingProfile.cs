using Application.Contracts.FraudReport;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class FraudReportMappingProfile : Profile
    {
        public FraudReportMappingProfile()
        {
            // Map FraudReport entity to FraudReportDto
            CreateMap<FraudReport, FraudReportDto>()
                .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src =>
                    src.Campaign != null ? src.Campaign.Name : null))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src =>
                    src.Publisher != null ? src.Publisher.CompanyName : null))
                .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(src =>
                    src.Advertiser != null ? src.Advertiser.CompanyName : null));

            // Map FraudReportCreateDto to FraudReport entity
            CreateMap<FraudReportCreateDto, FraudReport>();

            // Map FraudReportUpdateDto to FraudReport entity
            CreateMap<FraudReportUpdateDto, FraudReport>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Map FraudReportReadDto to FraudReport entity
            CreateMap<FraudReportReadDto, FraudReport>()
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead));
        }
    }
}