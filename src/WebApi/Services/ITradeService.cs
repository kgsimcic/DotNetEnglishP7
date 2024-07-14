using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface ITradeService
    {
        Task<Trade[]> GetAllTrades();
# nullable enable
        Task<Trade?> GetTrade(int id);
# nullable disable
        Task<Result> CreateTrade(Trade trade);
        Task<int> DeleteTrade(int id);
        Task<Result> UpdateTrade(int id, Trade trade);
    }
}
