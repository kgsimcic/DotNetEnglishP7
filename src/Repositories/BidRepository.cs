using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository
    {
        public LocalDbContext DbContext { get; }

        public BidRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BidList FindById(int id)
        {
            return DbContext.Bids.Where(bidList => bidList.BidListId == id)
                                  .FirstOrDefault();
        }

        public BidList[] FindAll()
        {
            return DbContext.Bids.ToArray();
        }

        public void Add(BidList bidList)
        {
            DbContext.Bids.Add(bidList);
        }

        public void Update(BidList bidList)
        {
            DbContext.Bids.Update(bidList);
        }

        public void Delete(int id) {
            var bidListToDelete = DbContext.Bids.Where(bidList => bidList.BidListId == id).FirstOrDefault();
            if (bidListToDelete != null)
            {
                DbContext.Bids.Remove(bidListToDelete);
            }
        }
    }
}