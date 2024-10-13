using System.Net;
using System.Net.Mail;

namespace TestAspNetApplication.Services
{
    public class SimpleEmailSender : IEmailSender
    {
        private readonly string _login;
        private readonly string _password;
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<SimpleEmailSender> _logger;
        public SimpleEmailSender(ILogger<SimpleEmailSender> logger) 
        {
            _logger = logger;
            _login = "7d8dfe001@smtp-brevo.com";
            _password = "BArSwECUsJRqMQjd";
            _smtpClient = new SmtpClient("smtp-relay.brevo.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_login, _password),
                Timeout = 10000
            };
        }
        public Task SendResetPasswordTokenAsync(string token, string email)
        {
            throw new NotImplementedException();
        }

        public async Task SendVerifyEmailTokenAsync(string token, string email)
        {
            MailAddress from = new MailAddress("no-reply@nickyparsons.ru", "NickyParsons RU");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Confirm your email";
            message.Body = $"https://nickyparsons.ru/verify-email?token={token}";
            try
            {
                await _smtpClient.SendMailAsync(message);
                _logger.LogDebug($"Message sended");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            } 
        }
    }
}
