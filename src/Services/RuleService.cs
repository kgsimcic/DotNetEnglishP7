﻿using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class RuleService : IRuleService
    {

        private readonly RuleRepository _ruleRepository;
        public RuleService(RuleRepository ruleRepository) {
            _ruleRepository = ruleRepository;
        }

        public Rule[] GetAllRules()
        {
            return _ruleRepository.FindAll();
        }

        public async Task<Rule> GetRule(int id)
        {
            return await _ruleRepository.FindById(id);
        }

        public async Task<int> CreateRule(Rule rule)
        {
            return await _ruleRepository.Create(rule);
        }

        public async Task<int> DeleteRule(int id)
        {
            var existingRule = _ruleRepository.FindById(id);
            if (existingRule == null)
            {
                throw new KeyNotFoundException("Rule not found.");
            }

            return await _ruleRepository.Delete(id);
        }

        public async Task<int> UpdateRule(int id, Rule rule)
        {
            var existingRule = _ruleRepository.FindById(id);
            if (existingRule == null)
            {
                throw new KeyNotFoundException("Rule not found.");
            }

            return await _ruleRepository.Update(rule);
        }
    }
}
