using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CurveId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public decimal Term { get; set; } = decimal.Zero;
        public decimal Value { get; set; } = decimal.Zero;
        public DateTime? CreationDate { get; set; }
    }
}