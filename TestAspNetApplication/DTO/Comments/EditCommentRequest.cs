using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class EditCommentRequest
    {
        [Required]
        [FromRoute]
        public required Guid CommentId { get; set; }
        [FromForm]
        [Required]
        public required Guid AuthorId { get; set; }
        [FromForm]
        [Required]
        public required string Text { get; set; }
    }
}
