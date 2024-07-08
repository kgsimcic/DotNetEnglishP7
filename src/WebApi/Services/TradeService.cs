using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot.Net.WebApi.Services
{
    public class TradeService : ITradeService
    {
        protected IRepository<Trade> _tradeRepository { get; }
        public TradeService(IRepository<Trade> tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public Trade[] GetAllTrades()
        {
            return _tradeRepository.GetAll();
        }

# nullable enable
        public Trade? GetTrade(int id)
        {
            return _tradeRepository.GetById(id);
        }
# nullable disable

        public async Task<int> CreateTrade(Trade trade)
        {
            _tradeRepository.Add(trade);
            return await _tradeRepository.SaveChangesAsync();
        }

        public async Task<int> DeleteTrade(int id)
        {
            var existingTrade = _tradeRepository.GetById(id);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found.");
            }

            _tradeRepository.Delete(existingTrade);
            return await _tradeRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateTrade(int id, Trade trade)
        {
            var existingTrade = _tradeRepository.GetById(id);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found.");
            }

            _tradeRepository.Update(trade);
            return await _tradeRepository.SaveChangesAsync();
        }
    }
}
