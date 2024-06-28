using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IRatingService
    {
        Rating[] GetAllRatings();
        Task<Rating> GetRating(int id);
        Task<Rating> CreateRating(Rating rating);
        Task<int> DeleteRating(int id);
        Task<int> UpdateRating(Rating rating);
    }
}
