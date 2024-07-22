namespace TestAspNetApplication.DTO
{
    public class CreateArticleRequest
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Text { get; set; }
        public Guid? AuthorId { get; set; }
    }
}
