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
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
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
            var services = builder.Services;
            services.AddCors();
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddSession();
            services.AddJwtAuthentication(builder.Configuration);
            services.AddAuthorization();
            //services.AddDistributedMemoryCache();
            string cacheConnectionString;
            string dbConnectionString;
            if (builder.Environment.IsEnvironment("Docker"))
            {
                cacheConnectionString = builder.Configuration.GetConnectionString("DockerCache")!;
                dbConnectionString = builder.Configuration.GetConnectionString("DockerDatabase")!;
            }
            else
            {
                cacheConnectionString = builder.Configuration.GetConnectionString("LocalCache")!;
                dbConnectionString = builder.Configuration.GetConnectionString("LocalDatabase")!;
            }
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = cacheConnectionString;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1"}));
            services.AddDbContext<PosgresDbContext>(opt => opt.UseNpgsql(dbConnectionString));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ArticleRepository>();
            services.AddScoped<ArticleService>();
            services.AddScoped<FileService>();
            services.AddScoped<ProfileService>();
            services.AddScoped<CommentsService>();
            services.AddTransient<TokenGenerator>();
            services.AddTransient<IEmailSender, SimpleEmailSender>();
            var app = builder.Build();
            app.UseCors(builder =>
            {
                builder
                .WithOrigins("http://localhost:80", "http://localhost:8081", "http://46.8.224.185")
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
            //if (app.Configuration.GetValue<Boolean>("ApplyMigration"))
            //{
            //    app.ApplyMigration();
            //}
            if (app.Environment.IsEnvironment("Docker"))
            {
                app.ApplyMigration();
            }
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            app.Run();
        }
    }
}
