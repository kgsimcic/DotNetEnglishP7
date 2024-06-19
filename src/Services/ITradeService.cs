using Dot.Net.WebApi.Domain;
using System.Collections;

namespace WebApi.Services
{
    public interface ITradeService
    {
        Trade[] GetAllTrades();
        Trade GetTrade(int id);
        void AddTrade(Trade trade);
        void DeleteTrade(int id);
        void UpdateTrade(Trade trade);
    }
}
