using Application.Contracts.Currency;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class CurrencyMappingProfile : Profile
    {
        public CurrencyMappingProfile()
        {
            // Entity to DTO
            CreateMap<Currency, CurrencyDto>();

            // DTOs to Entity
            CreateMap<CreateCurrencyDto, Currency>();
            CreateMap<UpdateCurrencyDto, Currency>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}