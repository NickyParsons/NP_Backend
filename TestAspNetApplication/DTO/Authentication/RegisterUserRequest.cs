using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class RegisterUserRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required, MinLength(6)]
        public required string Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
