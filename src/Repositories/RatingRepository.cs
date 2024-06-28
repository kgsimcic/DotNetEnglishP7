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

        public async Task<Rating> Create(Rating rating)
        {
            DbContext.Ratings.Add(rating);
            await DbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<int> Update(Rating rating)
        {
            var ratingToUpdate = DbContext.Ratings.Where(r => r.Id == rating.Id).FirstOrDefault();
            if (ratingToUpdate != null)
            {
                DbContext.Ratings.Update(ratingToUpdate);
            }
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id) {
            var ratingToDelete = DbContext.Ratings.Where(rating => rating.Id == id).FirstOrDefault();
            if (ratingToDelete != null)
            {
                DbContext.Ratings.Remove(ratingToDelete);
            }
            return await DbContext.SaveChangesAsync();
        }
    }
}