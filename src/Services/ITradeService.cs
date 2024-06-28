using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface ITradeService
    {
        Trade[] GetAllTrades();
        Task<Trade> GetTrade(int id);
        Task<Trade> CreateTrade(Trade trade);
        Task<int> DeleteTrade(int id);
        Task<int> UpdateTrade(int id, Trade trade);
    }
}
