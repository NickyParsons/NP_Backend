using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class ChangeEmailRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; }
        [Required, EmailAddress]
        public required string NewEmail { get; set; }
    }
}
