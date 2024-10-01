using Microsoft.EntityFrameworkCore;
using TestAspNetApplication.Data;

namespace TestAspNetApplication.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using PosgresDbContext dbContext = scope.ServiceProvider.GetRequiredService<PosgresDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
