using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class LikeArticleRequest
    {
        [FromRoute]
        [Required]
        public required Guid ArticleId { get; set; }
        [FromForm]
        [Required]
        public required Guid UserId { get; set; }
    }
}
