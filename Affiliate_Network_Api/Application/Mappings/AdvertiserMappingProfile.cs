using Application.Contracts.Advertiser;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class AdvertiserMappingProfile : Profile
    {
        public AdvertiserMappingProfile()
        {
            // Entity to DTO
            CreateMap<Advertiser, AdvertiserDto>();

            // DTO to Entity
            CreateMap<AdvertiserCreateDto, Advertiser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserId, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserBalances, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserUrls, opt => opt.Ignore())
                .ForMember(dest => dest.Campaigns, opt => opt.Ignore())
                .ForMember(dest => dest.DepositRequests, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.WithdrawalRequests, opt => opt.Ignore());

            CreateMap<AdvertiserUpdateDto, Advertiser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserBalances, opt => opt.Ignore())
                .ForMember(dest => dest.AdvertiserUrls, opt => opt.Ignore())
                .ForMember(dest => dest.Campaigns, opt => opt.Ignore())
                .ForMember(dest => dest.DepositRequests, opt => opt.Ignore())
                .ForMember(dest => dest.FraudReports, opt => opt.Ignore())
                .ForMember(dest => dest.WithdrawalRequests, opt => opt.Ignore());
        }
    }
}
