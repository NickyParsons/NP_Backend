namespace TestAspNetApplication.Data.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Text { get; set; }
        public string? ImageUrl {  get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
