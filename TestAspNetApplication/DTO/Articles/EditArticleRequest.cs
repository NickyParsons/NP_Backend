using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class EditArticleRequest
    {
        [FromRoute]
        [Required]
        public required Guid ArticleId { get; set; }
        [FromForm]
        public string? Name { get; set; }
        [FromForm]
        public string? Description { get; set; }
        [FromForm]
        public string? Text { get; set; }
        [FromForm]
        [Required]
        public required Guid AuthorId { get; set; }
    }
}
