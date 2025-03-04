using Application.Contracts.Publisher;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class PublisherMappingProfile : Profile
    {
        public PublisherMappingProfile()
        {
            // Entity to DTO
            CreateMap<Publisher, PublisherDto>()
                .ForMember(dest => dest.TotalTrafficSources, opt =>
                    opt.MapFrom(src => src.TrafficSources != null ? src.TrafficSources.Count : 0))
                .ForMember(dest => dest.TotalCampaigns, opt =>
                    opt.MapFrom(src => src.Promotes != null ? src.Promotes.Select(p => p.CampaignId).Distinct().Count() : 0));

            // DTO to Entity
            CreateMap<PublisherCreateDto, Publisher>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherId, opt => opt.Ignore())
                .ForMember(dest => dest.ReferralCode, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignPublisherCommissions, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.PayoutRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherBalances, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherProfiles, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherReferralReferreds, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherReferralReferrers, opt => opt.Ignore())
                .ForMember(dest => dest.TrafficSources, opt => opt.Ignore());

            CreateMap<PublisherUpdateDto, Publisher>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.ReferralCode, opt => opt.Ignore())
                .ForMember(dest => dest.ReferredByCode, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignPublisherCommissions, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.PayoutRequests, opt => opt.Ignore())
                .ForMember(dest => dest.Promotes, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherBalances, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherProfiles, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherReferralReferreds, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherReferralReferrers, opt => opt.Ignore())
                .ForMember(dest => dest.TrafficSources, opt => opt.Ignore());
        }
    }
}
