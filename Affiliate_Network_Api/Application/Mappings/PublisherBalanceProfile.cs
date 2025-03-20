// Application/MappingProfiles/PublisherBalanceProfile.cs
using Application.Contracts.PublisherBalance;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class PublisherBalanceProfile : Profile
    {
        public PublisherBalanceProfile()
        {
            CreateMap<PublisherBalance, PublisherBalanceDto>();
            CreateMap<PublisherBalanceCreateDto, PublisherBalance>();
            CreateMap<PublisherBalanceUpdateDto, PublisherBalance>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}