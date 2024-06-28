using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IRuleService
    {
        Rule[] GetAllRules();
        Task<Rule> GetRule(int id);
        Task<Rule> CreateRule(Rule ruleName);
        Task<int> DeleteRule(int id);
        Task<int> UpdateRule(int id, Rule ruleName);
    }
}
