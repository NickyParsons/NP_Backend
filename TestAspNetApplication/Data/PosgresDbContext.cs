using Microsoft.EntityFrameworkCore;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data.EntityTypeConfigurations;

namespace TestAspNetApplication.Data
{
    public class PosgresDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public PosgresDbContext(DbContextOptions<PosgresDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin", Description = "Администратор" };
            var userRole = new Role { Id = Guid.NewGuid(), Name = "User", Description = "Пользователь" };
            modelBuilder.Entity<Role>().HasData(adminRole);
            modelBuilder.Entity<Role>().HasData(userRole);
            modelBuilder.Entity<User>().HasData(new User { Id = Guid.NewGuid(), Email = "admin@admin", HashedPassword = "ClS/8E1JPP39gGPmRyBl+w==;9eoVht2Ofj0+SGFPWBL/WivJKHjT1ffYFPD4dj90WJE=", RoleId = adminRole.Id});
            base.OnModelCreating(modelBuilder);
        }
    }
}
