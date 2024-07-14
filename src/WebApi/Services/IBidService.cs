using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IBidService
    {
        Task<Bid[]> GetAllBids();
# nullable enable
        Task<Bid?> GetBid(int id);
# nullable disable
        Task<Result> CreateBid(Bid bid);
        Task<int> DeleteBid(int id);
        Task<Result> UpdateBid(int id, Bid bid);
    }
}
