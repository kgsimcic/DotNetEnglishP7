using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
#nullable enable
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username cannot be null.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be null.")]
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
#nullable disable
}