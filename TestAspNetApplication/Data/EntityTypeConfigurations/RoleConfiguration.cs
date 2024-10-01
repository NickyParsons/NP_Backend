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
            Role defaultRole = new Role();
            defaultRole.Name = "User";
            defaultRole.Description = "Пользователь";
            defaultRole.Id = Guid.NewGuid();
            builder.HasData(defaultRole);
        }
    }
}
