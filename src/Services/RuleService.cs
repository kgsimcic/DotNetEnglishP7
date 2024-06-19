using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

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

        public RuleName GetRule(int id)
        {
            return _ruleRepository.FindById(id);
        }

        public void CreateRule(RuleName rule)
        {
            _ruleRepository.Add(rule);
        }

        public void DeleteRule(int id)
        {
            _ruleRepository.Delete(id);
        }

        public void UpdateRule(RuleName rule)
        {
            _ruleRepository.Update(rule);
        }
    }
}
