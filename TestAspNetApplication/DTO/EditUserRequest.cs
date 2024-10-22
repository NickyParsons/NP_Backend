using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class EditUserRequest
    {
        [Required]
        public required Guid Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public required bool isPasswordChanging { get; set; }
        public string? newPassword { get; set; }
        public string? oldPassword { get; set; }
    }
}
