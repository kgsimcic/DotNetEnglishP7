using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface IBidService
    {
        Bid[] GetAllBids();
        Bid GetBid(int id);
        void CreateBid(Bid bidList);
        void DeleteBid(int id);
        void UpdateBid(Bid bidList);
    }
}
