namespace TestAspNetApplication.DTO
{
    public class EditUserRequest
    {
        public required Guid Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public required bool isPasswordChanging { get; set; }
        public string? newPassword { get; set; }
        public string? oldPassword { get; set; }
    }
}
