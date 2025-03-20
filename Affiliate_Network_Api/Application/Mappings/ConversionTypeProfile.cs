using Application.Contracts.ConversionType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class ConversionTypeProfile : Profile
    {
        public ConversionTypeProfile()
        {
            // Entity to DTO
            CreateMap<ConversionType, ConversionTypeDto>()
                .ForMember(dest => dest.RequiresApproval, opt => opt.MapFrom(src => src.RequiresApproval ?? false))
                .ForMember(dest => dest.CampaignCount, opt => opt.Ignore()); // Set separately in service

            // CreateDto to Entity
            CreateMap<ConversionTypeCreateDto, ConversionType>();

            // UpdateDto to Entity
            CreateMap<ConversionTypeUpdateDto, ConversionType>();
        }
    }
}