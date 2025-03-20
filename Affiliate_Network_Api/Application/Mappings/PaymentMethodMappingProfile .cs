using Application.Contracts.PaymentMethod;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class PaymentMethodMappingProfile : Profile
    {
        public PaymentMethodMappingProfile()
        {
            // Entity to DTO
            CreateMap<PaymentMethod, PaymentMethodDto>();

            // DTO to Entity
            CreateMap<CreatePaymentMethodDto, PaymentMethod>();
            CreateMap<UpdatePaymentMethodDto, PaymentMethod>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}