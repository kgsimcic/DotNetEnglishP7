using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Trade
    {
        [Key]
        public int TradeId { get; set; }
        [Required]
        public string Account { get; set; }
# nullable enable
        public string? Type { get; set; }
        public decimal BuyQuantity { get; set; } = decimal.Zero;
        public decimal SellQuantity { get; set; } = decimal.Zero;
        public decimal BuyPrice { get; set; } = decimal.Zero;
        public decimal SellPrice { get; set; } = decimal.Zero;
        public string? Benchmark { get; set; }
        public DateTime? TradeDate { get; set; }
        public string? Security {  get; set; }
        public string? Status { get; set; }
        public string? Trader { get; set; }
        public string? Book { get; set; }
        public string? CreationName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string? DealName { get; set; }
        public string? DealType { get; set; }
        public string? SourcelistId { get; set; }
        public string? Side { get; set; }
# nullable disable
    }
}