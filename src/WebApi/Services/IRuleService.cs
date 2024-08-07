﻿using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IRuleService
    {
        Task<Rule[]> GetAllRules();
# nullable enable
        Task<Rule?> GetRule(int id);
# nullable disable
        Task<Result> CreateRule(Rule ruleName);
        Task<int> DeleteRule(int id);
        Task<Result> UpdateRule(int id, Rule ruleName);
    }
}
