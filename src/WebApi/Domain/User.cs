using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
#nullable enable
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username cannot be null.")]
        public required string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password cannot be null.")]
        public string Password { get; set; } = String.Empty;
        [Required(ErrorMessage = "Password Salt cannot be null.")]
        public byte[] Salt { get; set; } = Array.Empty<byte>();
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
#nullable disable
}