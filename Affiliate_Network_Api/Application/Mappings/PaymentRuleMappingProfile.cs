using Application.Contracts.PayoutRule;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class PayoutRuleMappingProfile : Profile
    {
        public PayoutRuleMappingProfile()
        {
            // Entity to DTO
            CreateMap<PayoutRule, PayoutRuleDto>();

            // DTOs to Entity
            CreateMap<CreatePayoutRuleDto, PayoutRule>();

            CreateMap<UpdatePayoutRuleDto, PayoutRule>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}