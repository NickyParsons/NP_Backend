using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class LoginUserRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
