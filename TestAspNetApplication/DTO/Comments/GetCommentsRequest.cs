using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class GetCommentsRequest
    {
        [Required]
        [FromRoute]
        public required Guid ArticleId { get; set; }
        [FromQuery]
        public  int? Page { get; set; }
        [FromQuery]
        public int? PageSize { get; set; }
    }
}
