using Dot.Net.WebApi.Data;
using System.Linq;
using Dot.Net.WebApi.Domain;
using System;
using System.Collections.ObjectModel;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingRepository
    {
        public LocalDbContext DbContext { get; }

        public RatingRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Rating FindById(int id)
        {
            return DbContext.Ratings.Where(rating => rating.Id == id)
                                  .FirstOrDefault();
        }

        public Rating[] FindAll()
        {
            return DbContext.Ratings.ToArray();
        }

        public void Add(Rating rating)
        {
            DbContext.Ratings.Add(rating);
        }

        public void Update(Rating rating)
        {
            DbContext.Ratings.Update(rating);
        }

        public void Delete(int id) {
            var ratingToDelete = DbContext.Ratings.Where(rating => rating.Id == id).FirstOrDefault();
            if (ratingToDelete != null)
            {
                DbContext.Ratings.Remove(ratingToDelete);
            }
        }
    }
}