using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = "Undefined";
        public string? LastName { get; set; } = "Undefined";
        public string? PhotoPath { get; set; } = @"default.png";
        public override string ToString()
        {
            return $"[{Id}] {FirstName} {LastName}";
        }
    }
}
