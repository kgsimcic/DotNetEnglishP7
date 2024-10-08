﻿using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public class RuleService : IRuleService
    {
        protected IRepository<Rule> _ruleRepository { get; }
        public RuleService(IRepository<Rule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public Result ValidateRule(Rule rule)
        {
            // Validate rule name
            if (string.IsNullOrWhiteSpace(rule.Name))
            {
                return Result.Failure(
                    new Error("Rule.NameRequired", "Rule Name is required."));
            }
            return Result.Success();
        }

        public async Task<Rule[]> GetAllRules()
        {
            return await _ruleRepository.GetAll();
        }
# nullable enable
        public async Task<Rule?> GetRule(int id)
        {
            return await _ruleRepository.GetById(id);
        }
# nullable disable

        public async Task<Result> CreateRule(Rule rule)
        {
            var validationResult = ValidateRule(rule);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _ruleRepository.Add(rule);
            await _ruleRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<int> DeleteRule(int id)
        {
            var existingRule = await _ruleRepository.GetById(id);
            if (existingRule != null)
            {
                _ruleRepository.Delete(existingRule);
                return await _ruleRepository.SaveChangesAsync();
            }

            throw new KeyNotFoundException("Rule not found.");
        }

        public async Task<Result> UpdateRule(int id, Rule rule)
        {
            var existingRule = await _ruleRepository.GetById(id);
            if (existingRule != null)
            {
                var validationResult = ValidateRule(rule);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;
                }

                _ruleRepository.Update(rule);
                await _ruleRepository.SaveChangesAsync();
                return validationResult;
            }

            throw new KeyNotFoundException("Rule not found.");
        }
    }
}
