using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class RuleService : IRuleService
    {

        private readonly RuleRepository _ruleRepository;
        public RuleService(RuleRepository ruleRepository) {
            _ruleRepository = ruleRepository;
        }

        public RuleName[] GetAllRules()
        {
            return _ruleRepository.FindAll();
        }

        public async Task<RuleName> GetRule(int id)
        {
            return await _ruleRepository.FindById(id);
        }

        public async Task<RuleName> CreateRule(RuleName rule)
        {
            await _ruleRepository.Add(rule);
            return rule;
        }

        public async Task<int> DeleteRule(int id)
        {
            return await _ruleRepository.Delete(id);
        }

        public async Task<int> UpdateRule(RuleName rule)
        {
            return await _ruleRepository.Update(rule);
        }
    }
}
