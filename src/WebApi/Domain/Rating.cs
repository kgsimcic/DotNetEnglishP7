using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
# nullable enable
        public string? MoodysRating  { get; set; }
        public string? SandPRating { get; set; }
        public string? FitchRating { get; set; }
# nullable disable
        [Required]
        public int OrderNumber { get; set; }
    }
}