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
