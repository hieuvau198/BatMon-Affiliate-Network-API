using Application.Contracts.TrafficSource;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class TrafficSourceMappingProfile : Profile
    {
        public TrafficSourceMappingProfile()
        {
            // Map from entity to DTOs
            CreateMap<TrafficSource, TrafficSourceDto>();
            CreateMap<TrafficSource, ShortTrafficSourceDto>();
            CreateMap<Publisher, ShortPublisherDto>();

            // Map from DTOs to entities
            CreateMap<TrafficSourceCreateDto, TrafficSource>();
            CreateMap<TrafficSourceUpdateDto, TrafficSource>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}