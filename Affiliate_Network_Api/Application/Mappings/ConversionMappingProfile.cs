// Conversion Mapping Profile
using Application.Contracts.Conversion;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class ConversionMappingProfile : Profile
    {
        public ConversionMappingProfile()
        {
            // Entity to DTO
            CreateMap<Conversion, ConversionDto>();

            // DTO to Entity
            CreateMap<ConversionCreateDto, Conversion>();
            CreateMap<ConversionUpdateDto, Conversion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}