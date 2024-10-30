using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class EditUserRequest
    {
        [Required]
        [FromRoute]
        public required Guid UserId { get; set; }
        [FromForm]
        public string? Firstname { get; set; }
        [FromForm]
        public string? Lastname { get; set; }
        [FromForm]
        [Required]
        public required bool isPasswordChanging { get; set; }
        [FromForm]
        public string? newPassword { get; set; }
        [FromForm]
        public string? oldPassword { get; set; }
    }
}
