using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class CreateCommentRequest
    {
        [Required]
        public required Guid AuthorId { get; set; }
        [Required]
        public required string Text { get; set; }
    }
}
