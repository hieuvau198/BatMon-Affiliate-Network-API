using Application.Contracts.FraudCase;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class FraudCaseMappingProfile : Profile
    {
        public FraudCaseMappingProfile()
        {
            CreateMap<FraudCase, FraudCaseDto>()
                .ForMember(dest => dest.ConversionTransactionId, opt => opt.MapFrom(src => src.Conversion != null ? src.Conversion.TransactionId : null))
                .ForMember(dest => dest.FraudTypeName, opt => opt.MapFrom(src => src.FraudType != null ? src.FraudType.Name : null));

            CreateMap<FraudCaseCreateDto, FraudCase>();

            CreateMap<FraudCaseUpdateDto, FraudCase>();
        }
    }
}