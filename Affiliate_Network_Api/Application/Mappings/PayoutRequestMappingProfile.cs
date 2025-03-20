using Application.Contracts.PayoutRequest;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Mappings
{
    public class PayoutRequestMappingProfile : Profile
    {
        public PayoutRequestMappingProfile()
        {
            CreateMap<PayoutRequest, PayoutRequestDto>()
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src =>
                    src.Publisher != null ? src.Publisher.CompanyName ?? src.Publisher.Username : null))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src =>
                    src.CurrencyCodeNavigation != null ? src.CurrencyCodeNavigation.CurrencyName : null));

            CreateMap<PayoutRequestCreateDto, PayoutRequest>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));

            CreateMap<PayoutRequestUpdateDto, PayoutRequest>()
                .ForMember(dest => dest.RequestDate, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherId, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCode, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCodeNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore());
        }
    }
}