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
using TestAspNetApplication.RazorComponents;
using TestAspNetApplication.Services;

namespace TestAspNetApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("config.json");
            builder.Logging.AddProvider(new FileLoggerProvider(builder.Configuration.GetSection("Logging:LogDirectory").Value!));
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "NickyParsonsSite";
            });
            var services = builder.Services;
            services.AddJwtAuthentication(builder.Configuration);
            services.AddAuthorization();
            services.AddDistributedMemoryCache();
            services.AddDbContext<PosgresDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            services.AddRazorPages();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddRazorComponents().AddInteractiveServerComponents();
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(app.Configuration["DirectoryToShare"]!), RequestPath = new PathString("/share")
            });
            app.UseSession();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
            app.MapRazorPages();
            app.UseAntiforgery();
            app.MapRazorComponents<BlazorApp>().AddInteractiveServerRenderMode();
            app.MapGet("/api/persons", GetAllUsersHandler);
            app.MapGet("/api/persons/{id:int}", GetUserHandler);
            app.MapDelete("/api/persons/{id:int}", DeleteUserHandler);
            app.MapPost("/api/persons", CreateUserHandler);
            app.MapPut("/api/persons", EditUserHandler);
            app.Map("/test/{id?}", TestHandler);
            app.Map("/db", DbTest);
            app.Map("/logout", (HttpContext context) => { 
                context.Response.Cookies.Delete("nasty-boy");
                return Results.Redirect("/login");
            });
            app.Run();
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
