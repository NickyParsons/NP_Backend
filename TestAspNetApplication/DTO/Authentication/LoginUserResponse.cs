using System.ComponentModel.DataAnnotations;

namespace TestAspNetApplication.DTO
{
    public class LoginUserResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
