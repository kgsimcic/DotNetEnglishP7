using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class BidService : IBidService
    {

        private readonly BidRepository _bidRepository;
        public BidService(BidRepository bidRepository) {
            _bidRepository = bidRepository;
        }

        public Bid[] GetAllBids()
        {
            return _bidRepository.FindAll();
        }

        public async Task<Bid> GetBid(int id)
        {
            return await _bidRepository.FindById(id);
        }

        public async Task<int> CreateBid(Bid bid)
        {
            return await _bidRepository.Create(bid);
        }

        public async Task<int> DeleteBid(int id)
        {
            return await _bidRepository.Delete(id);
        }

        public async Task<int> UpdateBid(int id,Bid bid)
        {
            var existingBid = _bidRepository.FindById(id);
            if (existingBid == null)
            {
                throw new KeyNotFoundException("Bid not found.");
            }
            return await _bidRepository.Update(bid);
        }
    }
}
