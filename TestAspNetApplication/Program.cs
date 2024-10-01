using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Text;
using TestAspNetApplication.Data;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Extensions;
using TestAspNetApplication.FileLogger;
using TestAspNetApplication.Services;

namespace TestAspNetApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("config.json");
            //builder.Logging.AddProvider(new FileLoggerProvider(builder.Configuration.GetSection("Logging:LogDirectory").Value!));
            //builder.WebHost.UseUrls("http://192.168.1.2:5214");
            var services = builder.Services;
            //services.AddCors(option =>
            //{
            //    option.AddDefaultPolicy(option =>
            //    {
            //        option.WithOrigins("http://localhost:8081");
            //        option.AllowAnyMethod();
            //        option.AllowAnyHeader();
            //    });
            //});
            services.AddCors();
            services.AddControllers();
            services.AddSession();
            services.AddJwtAuthentication(builder.Configuration);
            services.AddAuthorization();
            services.AddDistributedMemoryCache();
            services.AddDbContext<PosgresDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ArticleRepository>();
            services.AddScoped<ArticleService>();
            services.AddScoped<FileService>();
            services.AddScoped<UserService>();
            var app = builder.Build();
            app.UseCors(builder =>
            {
                builder
                .WithOrigins("http://localhost:8081", "http://46.8.224.185")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseSession();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                //MinimumSameSitePolicy = SameSiteMode.Strict,
                //HttpOnly = HttpOnlyPolicy.Always,
                //Secure = CookieSecurePolicy.Always
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //app.MapGet("/api/persons", GetAllUsersHandler);
            //app.MapGet("/api/persons/{id:int}", GetUserHandler);
            //app.MapDelete("/api/persons/{id:int}", DeleteUserHandler);
            //app.MapPost("/api/persons", CreateUserHandler);
            //app.MapPut("/api/persons", EditUserHandler);
            //app.Map("/test/{id?}", TestHandler);
            //app.Map("/db", DbTest);
            //app.Map("/logout", (HttpContext context) => { 
            //    context.Response.Cookies.Delete("nasty-boy");
            //    return Results.Redirect("/");
            //});
            app.ApplyMigration();
            app.Run();
        }
        static async Task IndexHandler(HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync("wwwroot/html/index.html");
        }
        static async Task GetAllUsersHandler(HttpContext context, IPersonRepository repo, ILogger<Program> logger)
        {
            var users = await repo.GetAllAsync();
            await Results.Json(users).ExecuteAsync(context);
        }
        static async Task GetUserHandler(HttpContext context, int id, IPersonRepository repo, ILogger<Program> logger)
        {
            Person? user = await repo.GetByIdAsync(id);
            if (user != null)
            {
                await Results.Json(user).ExecuteAsync(context);
            }
            else
            {
                await Results.NotFound($"User with ID {id} not found").ExecuteAsync(context);
            }
        }
        static async Task DeleteUserHandler(HttpContext context, int id, IPersonRepository repo, ILogger<Program> logger)
        {
            
            Person? user = await repo.DeleteAsync(id);
            if (user != null)
            {
                await Results.Json(user).ExecuteAsync(context);
            }
            else
            {
                await Results.NotFound($"User with ID {id} not found").ExecuteAsync(context);
            }
        }
        static async Task CreateUserHandler(HttpContext context, Person? user, IPersonRepository repo, ILogger<Program> logger)
        {
            if (user != null)
            {
                await repo.CreateAsync(user);
                await Results.Json(user).ExecuteAsync(context);
            }
            else
            {
                await Results.NotFound("Input user is empty").ExecuteAsync(context);
            }
        }
        static async Task EditUserHandler(HttpContext context, Person? user, IPersonRepository repo, ILogger<Program> logger)
        {
            if (user != null)
            {
                Person? dbUser = await repo.UpdateAsync(user);
                if (dbUser != null)
                {
                    await Results.Json(dbUser).ExecuteAsync(context);
                }
                else
                {
                    await Results.NotFound($"User with ID {user.Id} not found").ExecuteAsync(context);
                }
            }
            else
            {
                await Results.NotFound("Input user is empty").ExecuteAsync(context);
                logger.LogWarning($"Input user is empty");
            }
        }
        static async Task TestHandler(HttpContext context, string? id, IConfiguration configuration, ILogger<Program> logger)
        {
            logger.LogInformation(new EventId(666) ,"Test method start");
            string resultHtml = string.Empty;
            resultHtml += "<p3>Test page</p3>";
            if (id != null)
            {
                resultHtml += $"<p>ID: {id}</p>";
            }
            if (context.Session.Keys.Contains("key"))
            {
                resultHtml += $"<p>Cookie key: {context.Session.GetString("key")}</p>";
            }
            resultHtml += "<p>Configuration:</p>";
            if (configuration != null)
            {
                resultHtml += GetConfigSectionText(configuration);
            }
            await Results.Content(resultHtml, "text/html", System.Text.Encoding.UTF8).ExecuteAsync(context);
        }
        [Authorize]
        static IResult DbTest(HttpContext httpContext, PosgresDbContext db, ILogger<Program> logger)
        {
            logger.LogInformation("Db method complete");
            return Results.Ok("Ok");
        } 
        static string GetConfigSectionText(IConfiguration configSection)
        {
            string resultHtml = string.Empty;
            foreach (var item in configSection.GetChildren())
            {
                if (item.Value == null) 
                {
                    resultHtml += $"<p>{item.Key}</p>";
                    resultHtml += GetConfigSectionText(item);
                }
                else 
                {
                    resultHtml += $"<p>{item.Key}: {item.Value}</p>";
                }
            }
            return resultHtml;
        }
    }
}
