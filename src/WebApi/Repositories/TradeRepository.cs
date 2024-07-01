using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository
    {
        public LocalDbContext DbContext { get; }

        public TradeRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Trade> FindById(int id)
        {
            return await DbContext.Trades.ToAsyncEnumerable().
                Where(trade => trade.TradeId == id)
                                  .FirstOrDefaultAsync();
        }

        public Trade[] FindAll()
        {
            return DbContext.Trades.ToArray();
        }

        public async Task<int> Create(Trade trade)
        {
            DbContext.Trades.Add(trade);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Trade trade)
        {
            DbContext.Trades.Update(trade);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var tradeToDelete = DbContext.Trades.Where(trade => trade.TradeId == id).FirstOrDefault();
            DbContext.Trades.Remove(tradeToDelete);
            return await DbContext.SaveChangesAsync();

        }
    }
}