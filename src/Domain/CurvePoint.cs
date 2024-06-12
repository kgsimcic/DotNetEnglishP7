using System;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        public int Id { get; set; }
        public int CurveId { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal Term { get; set; }
        public decimal Value { get; set; }
        public DateTime CreationDate { get; set; }
    }
}