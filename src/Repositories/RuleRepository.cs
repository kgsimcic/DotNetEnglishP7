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

        public async Task<RuleName> FindById(int id)
        {
            return await DbContext.Rules.ToAsyncEnumerable()
                .Where(ruleName => ruleName.Id == id)
                                  .FirstOrDefault();
        }

        public RuleName[] FindAll()
        {
            return DbContext.Rules.ToArray();
        }

        public async Task<RuleName> Add(RuleName ruleName)
        {
            DbContext.Rules.Add(ruleName);
            await DbContext.SaveChangesAsync();
            return ruleName;
        }

        public async Task<int> Update(int id)
        {
            var ruleToUpdate = DbContext.Trades.Where(trade => trade.TradeId == id).FirstOrDefault();
            if (ruleToUpdate != null)
            {
                DbContext.Trades.Update(ruleToUpdate);
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var ruleToDelete = DbContext.Rules.Where(ruleName => ruleName.Id == id).FirstOrDefault();
            if (ruleToDelete != null)
            {
                DbContext.Rules.Remove(ruleToDelete);
            }
            return await DbContext.SaveChangesAsync();
        }
    }
}