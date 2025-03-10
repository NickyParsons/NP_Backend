using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class LikeCommentRequest
    {
        [FromRoute]
        [Required]
        public required Guid CommentId { get; set; }
        [FromForm]
        [Required]
        public required Guid UserId { get; set; }
    }
}
