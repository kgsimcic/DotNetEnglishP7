using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;

namespace WebApi.Services
{
    public class TradeService : ITradeService
    {

        private readonly TradeRepository _tradeRepository;
        public TradeService(TradeRepository tradeRepository) {
            _tradeRepository = tradeRepository;
        }

        public Trade[] GetAllTrades()
        {
            return _tradeRepository.FindAll();
        }

        public Trade GetTrade(int id)
        {
            return _tradeRepository.FindById(id);
        }

        public void CreateTrade(Trade trade)
        {
            _tradeRepository.Add(trade);
        }

        public void DeleteTrade(int id)
        {
            _tradeRepository.Delete(id);
        }

        public void UpdateTrade(Trade trade)
        {
            _tradeRepository.Update(trade);
        }
    }
}
