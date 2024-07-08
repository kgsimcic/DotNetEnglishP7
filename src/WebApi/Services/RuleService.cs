using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
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

        public Rule[] GetAllRules()
        {
            return _ruleRepository.GetAll();
        }
# nullable enable
        public Rule? GetRule(int id)
        {
            return _ruleRepository.GetById(id);
        }
# nullable disable

        public async Task<int> CreateRule(Rule rule)
        {
            _ruleRepository.Add(rule);
            return await _ruleRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteRule(int id)
        {
            var existingRule = _ruleRepository.GetById(id);
            if (existingRule == null)
            {
                throw new KeyNotFoundException("Rule not found.");
            }

            _ruleRepository.Delete(existingRule);
            return await _ruleRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateRule(int id, Rule rule)
        {
            var existingRule = _ruleRepository.GetById(id);
            if (existingRule == null)
            {
                throw new KeyNotFoundException("Rule not found.");
            }

            _ruleRepository.Update(rule);
            return await _ruleRepository.SaveChangesAsync();
        }
    }
}
