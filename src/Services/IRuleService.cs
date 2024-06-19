using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface IRuleService
    {
        RuleName[] GetAllRules();
        RuleName GetRule(int id);
        void CreateRule(RuleName ruleName);
        void DeleteRule(int id);
        void UpdateRule(RuleName ruleName);
    }
}
