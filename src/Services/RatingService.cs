using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

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

        public Rating GetRating(int id)
        {
            return _ratingRepository.FindById(id);
        }

        public void CreateRating(Rating rating)
        {
            _ratingRepository.Add(rating);
        }

        public void DeleteRating(int id)
        {
            _ratingRepository.Delete(id);
        }

        public void UpdateRating(Rating rating)
        {
            _ratingRepository.Update(rating);
        }
    }
}
