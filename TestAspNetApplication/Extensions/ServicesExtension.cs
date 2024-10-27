using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestAspNetApplication.Extensions
{
    public static class ServicesExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                var validateParametrs = new TokenValidationParameters();
                validateParametrs.ValidateIssuer = false;
                validateParametrs.ValidateAudience = false;
                validateParametrs.ValidateLifetime = true;
                validateParametrs.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:SecretKey").Value!));
                option.TokenValidationParameters = validateParametrs;
                option.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["nasty-boy"];
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
