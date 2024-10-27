using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class CreateCommentRequest
    {
        [Required]
        [FromRoute]
        public required Guid ArticleId { get; set; }
        [FromForm]
        [Required]
        public required Guid AuthorId { get; set; }
        [FromForm]
        [Required]
        public required string Text { get; set; }
    }
}
