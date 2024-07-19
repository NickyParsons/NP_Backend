using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data.EntityTypeConfigurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(u => u.Author).WithMany(r => r.Articles).HasForeignKey(u => u.AuthorId);
        }
    }
}
