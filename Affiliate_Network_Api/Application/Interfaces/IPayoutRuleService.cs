using Application.Contracts.PayoutRule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPayoutRuleService
    {
        Task<IEnumerable<PayoutRuleDto>> GetAllPayoutRulesAsync(PayoutRuleFilterDto? filter = null);
        Task<PayoutRuleDto> GetPayoutRuleByIdAsync(int ruleId);
        Task<PayoutRuleDto> CreatePayoutRuleAsync(CreatePayoutRuleDto payoutRuleDto);
        Task<PayoutRuleDto> UpdatePayoutRuleAsync(int ruleId, UpdatePayoutRuleDto payoutRuleDto);
        Task<bool> DeletePayoutRuleAsync(int ruleId);
        Task<bool> PayoutRuleExistsAsync(int ruleId);
        Task<IEnumerable<PayoutRuleDto>> GetPayoutRulesByCurrencyAsync(string currency);
        Task<int> CountPayoutRulesAsync(PayoutRuleFilterDto? filter = null);
    }
}