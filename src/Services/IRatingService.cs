using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface IRatingService
    {
        Rating[] GetAllRatings();
        Rating GetRating(int id);
        void CreateRating(Rating rating);
        void DeleteRating(int id);
        void UpdateRating(Rating rating);
    }
}
