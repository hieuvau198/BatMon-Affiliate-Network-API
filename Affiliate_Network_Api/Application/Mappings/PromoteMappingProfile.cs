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

            CreateMap<PromoteCreateDto, Promote>()
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false)); // Explicitly map to false

            CreateMap<PromoteUpdateDto, Promote>();
        }
    }
}