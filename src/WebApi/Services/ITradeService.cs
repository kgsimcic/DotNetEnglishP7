using Dot.Net.WebApi.Domain;
using System.Collections;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public interface ITradeService
    {
        Trade[] GetAllTrades();
# nullable enable
        Trade? GetTrade(int id);
# nullable disable
        Task<int> CreateTrade(Trade trade);
        Task<int> DeleteTrade(int id);
        Task<int> UpdateTrade(int id, Trade trade);
    }
}
