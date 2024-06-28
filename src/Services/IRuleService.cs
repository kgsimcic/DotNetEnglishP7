using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IRuleService
    {
        RuleName[] GetAllRules();
        Task<RuleName> GetRule(int id);
        Task<RuleName> CreateRule(RuleName ruleName);
        Task<int> DeleteRule(int id);
        Task<int> UpdateRule(RuleName ruleName);
    }
}
