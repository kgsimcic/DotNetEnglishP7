using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface IBidService
    {
        BidList[] GetAllBids();
        BidList GetBid(int id);
        void CreateBid(BidList bidList);
        void DeleteBid(int id);
        void UpdateBid(BidList bidList);
    }
}
