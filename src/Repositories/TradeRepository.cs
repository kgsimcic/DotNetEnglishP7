using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository
    {
        public LocalDbContext DbContext { get; }

        public TradeRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Trade FindById(int id)
        {
            return DbContext.Trades.Where(trade => trade.TradeId == id)
                                  .FirstOrDefault();
        }

        public Trade[] FindAll()
        {
            return DbContext.Trades.ToArray();
        }

        public void Add(Trade trade)
        {
            DbContext.Trades.Add(trade);
        }

        public void Update(Trade trade)
        {
            DbContext.Trades.Update(trade);
        }

        public void Delete(int id) {
            var tradeToDelete = DbContext.Trades.Where(trade => trade.TradeId == id).FirstOrDefault();
            if (tradeToDelete != null)
            {
                DbContext.Trades.Remove(tradeToDelete);
            }
        }
    }
}