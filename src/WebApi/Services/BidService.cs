using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace  Dot.Net.WebApi.Services
{
    public class BidService : IBidService
    {

        protected IRepository<Bid> _bidRepository { get; }
        public BidService(IRepository<Bid> bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public Result ValidateBid(Bid bid)
        {
            // Validate bid quantity
            if (decimal.IsNegative(bid.BidQuantity))
            {
                return Result.Failure(
                    new Error("bid.BidQuantityNegative", "Bid Quantity cannot be negative."));
            }
            // Validate ask quantity
            if (decimal.IsNegative(bid.AskQuantity))
            {
                return Result.Failure(
                    new Error("bid.AskQuantityNegative", "Bid Ask Quantity Term cannot be negative."));
            }
            // Validate bid amount
            if (decimal.IsNegative(bid.BidAmount))
            {
                return Result.Failure(
                    new Error("bid.BidAmountNegative", "Bid Amount cannot be negative."));
            }
            // Validate ask
            if (decimal.IsNegative(bid.Ask))
            {
                return Result.Failure(
                    new Error("bid.AskNegative", "Bid Ask Amount cannot be negative."));
            }

            return Result.Success();
        }

        public Bid[] GetAllBids()
        {
            return _bidRepository.GetAll();
        }
# nullable enable
        public Bid? GetBid(int id)
        {
            return _bidRepository.GetById(id);
        }
# nullable disable

        public async Task<Result> CreateBid(Bid bid)
        {
            var validationResult = ValidateBid(bid);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _bidRepository.Add(bid);
            await _bidRepository.SaveChangesAsync();
            return validationResult;
        }

        public async Task<int> DeleteBid(int id)
        {
            var existingBid = _bidRepository.GetById(id);
            if (existingBid == null)
            {
                throw new KeyNotFoundException("Bid not found.");
            }

            _bidRepository.Delete(existingBid);
            return await _bidRepository.SaveChangesAsync();
        }

        public async Task<Result> UpdateBid(int id,Bid bid)
        {
            var existingBid = _bidRepository.GetById(id);
            if (existingBid == null)
            {
                throw new KeyNotFoundException("Bid not found.");
            }

            var validationResult = ValidateBid(bid);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _bidRepository.Update(bid);
            await _bidRepository.SaveChangesAsync();
            return validationResult;
        }
    }
}
