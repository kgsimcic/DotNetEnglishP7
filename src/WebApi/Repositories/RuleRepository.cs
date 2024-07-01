using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Repositories
{
    public class RuleRepository
    {
        public LocalDbContext DbContext { get; }

        public RuleRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Rule> FindById(int id)
        {
            return await DbContext.Rules.ToAsyncEnumerable()
                .Where(ruleName => ruleName.Id == id)
                                  .FirstOrDefaultAsync();
        }

        public Rule[] FindAll()
        {
            return DbContext.Rules.ToArray();
        }

        public async Task<int> Create(Rule ruleName)
        {
            DbContext.Rules.Add(ruleName);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Rule rule)
        {
            var ruleToUpdate = DbContext.Rules.Where(r => r.Id == rule.Id).FirstOrDefault();
            if (ruleToUpdate != null)
            {
                DbContext.Rules.Update(rule);
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var ruleToDelete = DbContext.Rules.Where(ruleName => ruleName.Id == id).FirstOrDefault();
            DbContext.Rules.Remove(ruleToDelete);
            return await DbContext.SaveChangesAsync();
        }
    }
}