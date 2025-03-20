using Application.Contracts.DepositRequest;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class DepositRequestMappingProfile : Profile
    {
        public DepositRequestMappingProfile()
        {
            CreateMap<DepositRequest, DepositRequestDto>()
                .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(src => src.Advertiser != null ? src.Advertiser.CompanyName : null)) // Updated to use CompanyName
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.CurrencyCodeNavigation != null ? src.CurrencyCodeNavigation.CurrencyName : null));

            CreateMap<DepositRequestCreateDto, DepositRequest>();

            CreateMap<DepositRequestUpdateDto, DepositRequest>();
        }
    }
}