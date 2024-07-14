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

        public async Task<Result> CreateTrade(Trade trade)
        {
            var validationResult = ValidateTrade(trade);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _tradeRepository.Add(trade);
            await _tradeRepository.SaveChangesAsync();
            return validationResult;
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

        public async Task<Result> UpdateTrade(int id, Trade trade)
        {
            var existingTrade = _tradeRepository.GetById(id);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found.");
            }

            var validationResult = ValidateTrade(trade);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            _tradeRepository.Update(trade);
            await _tradeRepository.SaveChangesAsync();
            return validationResult;
        }
    }
}
