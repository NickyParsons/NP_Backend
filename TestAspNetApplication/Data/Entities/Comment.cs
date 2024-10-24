namespace TestAspNetApplication.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        required public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public Guid ArticleId { get; set; }
        public Article? Article { get; set; }
    }
}
