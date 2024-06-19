using System;
using System.Numerics;

namespace Dot.Net.WebApi.Domain
{
    public class BidList
    {
        public int BidListId { get; set; }
        public string Account {  get; set; }
        public string Type { get; set; }
        public decimal? BidQuantity { get; set; }
        public decimal? AskQuantity { get; set; }
        public decimal? Bid { get; set; }
        public decimal? Ask { get; set; }
        public string? Benchmark { get; set; }
        public DateTime BidListDate { get; set; }
        public string? Commentary { get; set; }
        public string? Security { get; set; }
        public string? Status { get; set; }
        public string? Trader { get; set; }
        public string? Book { get; set; }
        public string? CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string? DealName { get; set; }
        public string? DealType { get; set; }
        public string? SourceListId { get; set; }
        public string? Side { get; set; }

    }
}