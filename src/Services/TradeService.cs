using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<Trade> GetTrade(int id)
        {
            return await _tradeRepository.FindById(id);
        }

        public async Task<int> CreateTrade(Trade trade)
        {
            return await _tradeRepository.Create(trade);
        }

        public async Task<int> DeleteTrade(int id)
        {
            return await _tradeRepository.Delete(id);
        }

        public async Task<int> UpdateTrade(int id, Trade trade)
        {
            var existingTrade = _tradeRepository.FindById(id);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found.");
            }

            return await _tradeRepository.Update(trade);
        }
    }
}
