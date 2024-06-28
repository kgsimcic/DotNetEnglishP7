using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class RatingService : IRatingService
    {

        private readonly RatingRepository _ratingRepository;
        public RatingService(RatingRepository ratingRepository) {
            _ratingRepository = ratingRepository;
        }

        public Rating[] GetAllRatings()
        {
            return _ratingRepository.FindAll();
        }

        public async Task<Rating> GetRating(int id)
        {
            return await _ratingRepository.FindById(id);
        }

        public async Task<Rating> CreateRating(Rating rating)
        {
            return await _ratingRepository.Create(rating);
        }

        public async Task<int> DeleteRating(int id)
        {
            return await _ratingRepository.Delete(id);
        }

        public async Task<int> UpdateRating(int id, Rating rating)
        {
            var existingRating = _ratingRepository.FindById(id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }

            return await _ratingRepository.Update(rating);
        }
    }
}
