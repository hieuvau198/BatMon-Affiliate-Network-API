using Application.Contracts.Promote;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class PromoteMappingProfile : Profile
    {
        public PromoteMappingProfile()
        {
            CreateMap<Promote, PromoteDto>()
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Username : null))
                .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src => src.Campaign != null ? src.Campaign.Name : null));

            CreateMap<PromoteCreateDto, Promote>();

            CreateMap<PromoteUpdateDto, Promote>();
        }
    }
}