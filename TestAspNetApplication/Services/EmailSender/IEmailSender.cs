namespace TestAspNetApplication.Services
{
    public interface IEmailSender
    {
        public Task SendVerifyEmailTokenAsync(string token, string email);
        public Task SendResetPasswordTokenAsync(string token, string email);
    }
}
