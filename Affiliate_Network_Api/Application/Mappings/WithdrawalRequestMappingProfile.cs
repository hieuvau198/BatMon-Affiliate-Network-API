using Application.Contracts.WithdrawalRequest;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class WithdrawalRequestMappingProfile : Profile
    {
        public WithdrawalRequestMappingProfile()
        {
            CreateMap<WithdrawalRequest, WithdrawalRequestDto>()
                .ForMember(dest => dest.AdvertiserName, opt => opt.MapFrom(src => src.Advertiser != null ? src.Advertiser.CompanyName : null))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.CurrencyCodeNavigation != null ? src.CurrencyCodeNavigation.CurrencyName : null));
            CreateMap<CreateWithdrawalRequestDto, WithdrawalRequest>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(_ => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pending"));
            CreateMap<UpdateWithdrawalRequestDto, WithdrawalRequest>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}