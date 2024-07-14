using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface IRatingService
    {
        Task<Rating[]> GetAllRatings();
# nullable enable
        Task<Rating?> GetRating(int id);
# nullable disable
        Task<Result> CreateRating(Rating rating);
        Task<int> DeleteRating(int id);
        Task<Result> UpdateRating(int id, Rating rating);
    }
}
