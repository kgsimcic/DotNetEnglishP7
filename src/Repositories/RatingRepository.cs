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
    public class RatingRepository
    {
        public LocalDbContext DbContext { get; }

        public RatingRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Rating> FindById(int id)
        {
            return await DbContext.Ratings.ToAsyncEnumerable()
                .Where(rating => rating.Id == id)
                                  .FirstOrDefault();
        }

        public Rating[] FindAll()
        {
            return DbContext.Ratings.ToArray();
        }

        public async Task<int> Create(Rating rating)
        {
            DbContext.Ratings.Add(rating);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Update(Rating rating)
        {
            DbContext.Ratings.Update(rating);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var ratingToDelete = DbContext.Ratings.Where(rating => rating.Id == id).FirstOrDefault();
            DbContext.Ratings.Remove(ratingToDelete);
            return await DbContext.SaveChangesAsync();
        }
    }
}