using Application.Contracts.PromoteUrlvariant;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class PromoteUrlvariantsMappingProfile : Profile
    {
        public PromoteUrlvariantsMappingProfile()
        {
            CreateMap<PromoteUrlvariant, PromoteUrlvariantDto>()
                .ForMember(dest => dest.PromoteName, opt => opt.MapFrom(src => src.Promote != null ? $"Promote {src.Promote.PromoteId}" : null))
                .ForMember(dest => dest.TrafficSourceName, opt => opt.MapFrom(src => src.TrafficSource != null ? src.TrafficSource.Name : null));

            CreateMap<PromoteUrlvariantCreateDto, PromoteUrlvariant>();

            CreateMap<PromoteUrlvariantUpdateDto, PromoteUrlvariant>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
