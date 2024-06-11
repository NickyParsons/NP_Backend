using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAspNetApplication.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new();
        public override string ToString()
        {
            return $"Role ID: [{Id}] {Name}";
        }
    }
}
