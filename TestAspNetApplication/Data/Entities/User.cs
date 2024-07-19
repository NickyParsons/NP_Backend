using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAspNetApplication.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
        public override string ToString()
        {
            return $"[{Id}] {FirstName} {LastName} {Email}";
        }
    }
}
