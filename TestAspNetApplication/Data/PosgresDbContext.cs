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
            base.OnModelCreating(modelBuilder);
        }
    }
}
