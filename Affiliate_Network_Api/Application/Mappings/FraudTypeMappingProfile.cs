using Application.Contracts.FraudType;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class FraudTypeMappingProfile : Profile
    {
        public FraudTypeMappingProfile()
        {
            CreateMap<FraudType, FraudTypeDto>()
                .ForMember(dest => dest.FraudCaseCount, opt => opt.MapFrom(src => src.FraudCases != null ? src.FraudCases.Count : 0));

            CreateMap<FraudTypeCreateDto, FraudType>();

            CreateMap<FraudTypeUpdateDto, FraudType>();
        }
    }
}