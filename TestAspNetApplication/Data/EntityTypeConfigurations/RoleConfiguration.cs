using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data.EntityTypeConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasMany(r => r.Users).WithOne(u => u.Role);
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Администратор" });
            builder.HasData(new Role { Id = Guid.NewGuid(), Name = "User", Description = "Пользователь" });
        }
    }
}
