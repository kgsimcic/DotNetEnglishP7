using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        public Result ValidateTrade(Trade trade)
        {
            // Validate trade buy quantity
            if (decimal.IsNegative(trade.BuyQuantity))
            {
                return Result.Failure(
                    new Error("trade.BuyQuantityNegative", "Trade Buy Quantity cannot be negative."));
            }
            // Validate trade buy price
            if (decimal.IsNegative(trade.BuyPrice))
            {
                return Result.Failure(
                    new Error("trade.BuyPriceNegative", "Trade Buy Price Term cannot be negative."));
            }
            // Validate trade sell quantity
            if (decimal.IsNegative(trade.SellQuantity))
            {
                return Result.Failure(
                    new Error("trade.SellQuantityNegative", "Trade Sell Quantity cannot be negative."));
            }
            // Validate trade sell price
            if (decimal.IsNegative(trade.SellPrice))
            {
                return Result.Failure(
                    new Error("trade.SellPriceNegative", "Trade Sell Price cannot be negative."));
            }

            return Result.Success();
        }

        public async Task<Trade[]> GetAllTrades()
        {
            return await _tradeRepository.GetAll();
        }

# nullable enable
        public async Task<Trade?> GetTrade(int id)
        {
            return await _tradeRepository.GetById(id);
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
            var existingTrade = await _tradeRepository.GetById(id);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found.");
            }

            _tradeRepository.Delete(existingTrade);
            return await _tradeRepository.SaveChangesAsync();
        }

        public async Task<Result> UpdateTrade(int id, Trade trade)
        {
            var existingTrade = await _tradeRepository.GetById(id);
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
