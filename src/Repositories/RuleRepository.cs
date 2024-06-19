using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;

namespace Dot.Net.WebApi.Repositories
{
    public class RuleRepository
    {
        public LocalDbContext DbContext { get; }

        public RuleRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public RuleName FindById(int id)
        {
            return DbContext.Rules.Where(ruleName => ruleName.Id == id)
                                  .FirstOrDefault();
        }

        public RuleName[] FindAll()
        {
            return DbContext.Rules.ToArray();
        }

        public void Add(RuleName ruleName)
        {
            DbContext.Rules.Add(ruleName);
        }

        public void Update(RuleName ruleName)
        {
            DbContext.Rules.Update(ruleName);
        }

        public void Delete(int id) {
            var ruleToDelete = DbContext.Rules.Where(ruleName => ruleName.Id == id).FirstOrDefault();
            if (ruleToDelete != null)
            {
                DbContext.Rules.Remove(ruleToDelete);
            }
        }
    }
}