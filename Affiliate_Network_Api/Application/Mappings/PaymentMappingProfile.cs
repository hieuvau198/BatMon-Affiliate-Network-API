using Application.Contracts.Payment;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            // Entity to DTO
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src =>
                    src.PaymentMethod != null ? src.PaymentMethod.Name : null))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src =>
                    src.CurrencyCodeNavigation != null ? src.CurrencyCodeNavigation.CurrencyName : null));

            // Create DTO to Entity
            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore());

            // Update DTO to Entity
            CreateMap<UpdatePaymentDto, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())
                .ForMember(dest => dest.RequestType, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCodeNavigation, opt => opt.Ignore());
        }
    }
}
