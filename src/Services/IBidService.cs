using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IBidService
    {
        Bid[] GetAllBids();
        Task<Bid> GetBid(int id);
        Task<Bid> CreateBid(Bid bid);
        Task<int> DeleteBid(int id);
        Task<int> UpdateBid(Bid bid);
    }
}
