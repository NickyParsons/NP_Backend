namespace TestAspNetApplication.DTO
{
    public class RegisterUserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
