using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public class RatingService : IRatingService
    {
        protected IRepository<Rating> _ratingRepository { get; }
        public RatingService(IRepository<Rating> ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public Rating[] GetAllRatings()
        {
            return _ratingRepository.GetAll();
        }
# nullable enable
        public Rating? GetRating(int id)
        {
            return _ratingRepository.GetById(id);
        }
# nullable disable

        public async Task<int> CreateRating(Rating rating)
        {
            _ratingRepository.Add(rating);
            return await _ratingRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteRating(int id)
        {
            var existingRating = _ratingRepository.GetById(id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }

            _ratingRepository.Delete(existingRating);
            return await _ratingRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateRating(int id, Rating rating)
        {
            var existingRating = _ratingRepository.GetById(id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }

            _ratingRepository.Update(rating);
            return await _ratingRepository.SaveChangesAsync();
        }
    }
}
