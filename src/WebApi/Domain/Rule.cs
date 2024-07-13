using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Controllers
{
    public class Rule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
# nullable enable
        public string? Description { get; set; }
        public string? Json { get; set; }
        public string? Template { get; set; }
        public string? SqlStr { get; set; }
        public string? SqlPart { get; set; }
# nullable disable
    }
}