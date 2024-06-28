using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository
    {
        public LocalDbContext DbContext { get; }

        public BidRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Bid> FindById(int id)
        {
            return await DbContext.Bids.ToAsyncEnumerable().Where(bid => bid.BidListId == id)
                                  .FirstOrDefault();
        }

        public Bid[] FindAll()
        {
            return DbContext.Bids.ToArray();
        }

        public async Task<Bid> Create(Bid bid)
        {
            DbContext.Bids.Add(bid);
            await DbContext.SaveChangesAsync();
            return bid;
        }

        public async Task<int> Update(Bid bid)
        {
            DbContext.Bids.Update(bid);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var bidToDelete = DbContext.Bids.FirstOrDefault();
            DbContext.Bids.Remove(bidToDelete);
            return await DbContext.SaveChangesAsync();
        }
    }
}