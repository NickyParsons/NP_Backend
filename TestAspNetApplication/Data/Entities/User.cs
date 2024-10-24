using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAspNetApplication.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
        public string? VerificationToken { get; set; }
        public DateTimeOffset? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTimeOffset? ResetTokenExpires { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public override string ToString()
        {
            return $"[{Id}] {FirstName} {LastName} {Email}";
        }
    }
}
