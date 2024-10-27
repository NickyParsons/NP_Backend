using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class DeleteCommentRequest
    {
        [Required]
        [FromRoute]
        public required Guid CommentId { get; set; }
        [FromForm]
        [Required]
        public required Guid AuthorId { get; set; }
    }
}
