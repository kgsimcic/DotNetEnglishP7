using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers.Domain
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public string? MoodysRating  { get; set; }
        public string? SandPRating { get; set; }
        public string? FitchRating { get; set; }
        [Required]
        public int OrderNumber { get; set; }
    }
}