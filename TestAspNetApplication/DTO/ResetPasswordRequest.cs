using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class ResetPasswordRequest
    {
        [Required]
        public required string Token { get; set; }
        [Required, MinLength(6)]
        public required string Password { get; set; }
    }
}
