using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class CreateArticleRequest
    {
        [FromForm]
        [Required]
        public required string Name { get; set; }
        [FromForm]
        public string? Description { get; set; }
        [FromForm]
        public string? Text { get; set; }
        [Required]
        [FromForm]
        required public Guid AuthorId { get; set; }
    }
}
