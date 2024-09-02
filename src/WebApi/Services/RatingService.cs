using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        public Result ValidateRating(Rating rating)
        {
            // Validate rating order number
            if (int.IsNegative(rating.OrderNumber))
            {
                return Result.Failure(
                    new Error("Rating.OrderNumberNegative", "Order Number cannot be negative."));
            }

            return Result.Success();
        }

        public async Task<Rating[]> GetAllRatings()
        {
            return await _ratingRepository.GetAll();
        }
# nullable enable
        public async Task<Rating?> GetRating(int id)
        {
            return await _ratingRepository.GetById(id);
        }
# nullable disable

        public async Task<Result> CreateRating(Rating rating)
        {
            var validationResult = ValidateRating(rating);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _ratingRepository.Add(rating);
            await _ratingRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<int> DeleteRating(int id)
        {
            var existingRating = await _ratingRepository.GetById(id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }

            _ratingRepository.Delete(existingRating);
            return await _ratingRepository.SaveChangesAsync();
        }

        public async Task<Result> UpdateRating(int id, Rating rating)
        {
            var existingRating = await _ratingRepository.GetById(id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found.");
            }

            var validationResult = ValidateRating(rating);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _ratingRepository.Update(rating);
            await _ratingRepository.SaveChangesAsync();
            return validationResult;
        }
    }
}
