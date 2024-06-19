using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

namespace WebApi.Services
{
    public class BidService : IBidService
    {

        private readonly BidRepository _bidRepository;
        public BidService(BidRepository bidRepository) {
            _bidRepository = bidRepository;
        }

        public BidList[] GetAllBids()
        {
            return _bidRepository.FindAll();
        }

        public BidList GetBid(int id)
        {
            return _bidRepository.FindById(id);
        }

        public void CreateBid(BidList bidList)
        {
            _bidRepository.Add(bidList);
        }

        public void DeleteBid(int id)
        {
            _bidRepository.Delete(id);
        }

        public void UpdateBid(BidList bidList)
        {
            _bidRepository.Update(bidList);
        }
    }
}
