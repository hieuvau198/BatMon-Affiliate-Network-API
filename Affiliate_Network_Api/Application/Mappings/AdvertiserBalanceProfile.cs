// AdvertiserBalanceProfile.cs
using Application.Contracts.AdvertiserBalance;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AdvertiserBalanceProfile : Profile
    {
        public AdvertiserBalanceProfile()
        {
            CreateMap<AdvertiserBalance, AdvertiserBalanceDto>();
            CreateMap<AdvertiserBalanceCreateDto, AdvertiserBalance>();
            CreateMap<AdvertiserBalanceUpdateDto, AdvertiserBalance>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}