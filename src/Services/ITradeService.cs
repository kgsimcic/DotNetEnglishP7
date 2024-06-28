using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface ITradeService
    {
        Trade[] GetAllTrades();
        Task<Trade> GetTrade(int id);
        Task<Trade> AddTrade(Trade trade);
        Task<int> DeleteTrade(int id);
        Task<int> UpdateTrade(Trade trade);
    }
}
