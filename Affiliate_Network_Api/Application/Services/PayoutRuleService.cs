using Application.Contracts.PayoutRule;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PayoutRuleService : IPayoutRuleService
    {
        private readonly IGenericRepository<PayoutRule> _payoutRuleRepository;
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public PayoutRuleService(
            IGenericRepository<PayoutRule> payoutRuleRepository,
            IGenericRepository<Currency> currencyRepository,
            IMapper mapper)
        {
            _payoutRuleRepository = payoutRuleRepository ?? throw new ArgumentNullException(nameof(payoutRuleRepository));
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PayoutRuleDto>> GetAllPayoutRulesAsync(PayoutRuleFilterDto? filter = null)
        {
            Expression<Func<PayoutRule, bool>>? predicate = null;

            if (filter != null)
            {
                predicate = rule =>
                    (filter.Currency == null || rule.Currency == filter.Currency) &&
                    (filter.AutoPayout == null || rule.AutoPayout == filter.AutoPayout);
            }

            var payoutRules = await _payoutRuleRepository.GetAllAsync(predicate);
            return _mapper.Map<IEnumerable<PayoutRuleDto>>(payoutRules);
        }

        public async Task<PayoutRuleDto> GetPayoutRuleByIdAsync(int ruleId)
        {
            var payoutRule = await _payoutRuleRepository.GetByIdAsync(ruleId);

            if (payoutRule == null)
                throw new KeyNotFoundException($"PayoutRule with ID {ruleId} not found.");

            return _mapper.Map<PayoutRuleDto>(payoutRule);
        }

        public async Task<PayoutRuleDto> CreatePayoutRuleAsync(CreatePayoutRuleDto payoutRuleDto)
        {
            // Validate currency if provided
            if (!string.IsNullOrEmpty(payoutRuleDto.Currency))
            {
                var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == payoutRuleDto.Currency);
                if (!currencyExists)
                    throw new KeyNotFoundException($"Currency with code {payoutRuleDto.Currency} not found.");
            }

            // Validate payout day (1-31)
            if (payoutRuleDto.PayoutDay.HasValue && (payoutRuleDto.PayoutDay < 1 || payoutRuleDto.PayoutDay > 31))
                throw new ArgumentException("PayoutDay must be between 1 and 31.");

            var payoutRule = _mapper.Map<PayoutRule>(payoutRuleDto);
            var createdPayoutRule = await _payoutRuleRepository.CreateAsync(payoutRule);

            return _mapper.Map<PayoutRuleDto>(createdPayoutRule);
        }

        public async Task<PayoutRuleDto> UpdatePayoutRuleAsync(int ruleId, UpdatePayoutRuleDto payoutRuleDto)
        {
            var existingPayoutRule = await _payoutRuleRepository.GetByIdAsync(ruleId);

            if (existingPayoutRule == null)
                throw new KeyNotFoundException($"PayoutRule with ID {ruleId} not found.");

            // Validate currency if provided
            if (!string.IsNullOrEmpty(payoutRuleDto.Currency))
            {
                var currencyExists = await _currencyRepository.ExistsAsync(c => c.CurrencyCode == payoutRuleDto.Currency);
                if (!currencyExists)
                    throw new KeyNotFoundException($"Currency with code {payoutRuleDto.Currency} not found.");
            }

            // Validate payout day (1-31)
            if (payoutRuleDto.PayoutDay.HasValue && (payoutRuleDto.PayoutDay < 1 || payoutRuleDto.PayoutDay > 31))
                throw new ArgumentException("PayoutDay must be between 1 and 31.");

            _mapper.Map(payoutRuleDto, existingPayoutRule);
            await _payoutRuleRepository.UpdateAsync(existingPayoutRule);

            return _mapper.Map<PayoutRuleDto>(existingPayoutRule);
        }

        public async Task<bool> DeletePayoutRuleAsync(int ruleId)
        {
            var payoutRule = await _payoutRuleRepository.GetByIdAsync(ruleId);

            if (payoutRule == null)
                return false;

            await _payoutRuleRepository.DeleteAsync(payoutRule);
            return true;
        }

        public async Task<bool> PayoutRuleExistsAsync(int ruleId)
        {
            return await _payoutRuleRepository.ExistsAsync(r => r.RuleId == ruleId);
        }

        public async Task<IEnumerable<PayoutRuleDto>> GetPayoutRulesByCurrencyAsync(string currency)
        {
            var payoutRules = await _payoutRuleRepository.GetAllAsync(r => r.Currency == currency);
            return _mapper.Map<IEnumerable<PayoutRuleDto>>(payoutRules);
        }

        public async Task<int> CountPayoutRulesAsync(PayoutRuleFilterDto? filter = null)
        {
            Expression<Func<PayoutRule, bool>>? predicate = null;

            if (filter != null)
            {
                predicate = rule =>
                    (filter.Currency == null || rule.Currency == filter.Currency) &&
                    (filter.AutoPayout == null || rule.AutoPayout == filter.AutoPayout);
            }
            else
            {
                predicate = _ => true;
            }

            return await _payoutRuleRepository.CountAsync(predicate);
        }
    }
}