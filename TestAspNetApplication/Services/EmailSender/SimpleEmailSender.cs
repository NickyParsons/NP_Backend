using System.Net;
using System.Net.Mail;
using System.Web;
using TestAspNetApplication.Enums;

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
        public async Task SendTokenMailAsync(string email, SenderType tokenType, string token)
        {
            MailAddress from = new MailAddress("no-reply@nickyparsons.ru", "NickyParsons RU");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            switch (tokenType)
            {
                case SenderType.VerifyEmail:
                    message.Subject = "Confirm your email";
                    message.Body = CreateVerificationEmailBody(token);
                    break;
                case SenderType.ForgotPassword:
                    message.Subject = "Reset your password";
                    message.Body = CreateForgotPasswordBody(token);
                    break;
                default:
                    _logger.LogDebug($"Unknown sender type");
                    throw new NotImplementedException();
            }
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
        public async Task SendResetPasswordTokenAsync(string token, string email)
        {
            await SendTokenMailAsync(email, SenderType.ForgotPassword, token);
        }

        public async Task SendVerifyEmailTokenAsync(string token, string email)
        {
            await SendTokenMailAsync(email, SenderType.VerifyEmail, token);
        }
        public string CreateVerificationEmailBody(string token)
        {
            string url = $"https://nickyparsons.ru/verify-email?token={HttpUtility.UrlEncode(token)}";
            string body = string.Empty;
            body += $"<p>Hello. To confirm your E-mail at nickyparsons.ru go to <a href=\"{url}\">this</a> link</p>";
            body += $"<p>or use manually this token: {token}</p>";
            return body;
        }
        public string CreateForgotPasswordBody(string token)
        {
            string url = $"https://nickyparsons.ru/reset-password?token={HttpUtility.UrlEncode(token)}";
            string body = string.Empty;
            body += $"<p>Hello. To reset your password at nickyparsons.ru go to <a href=\"{url}\">this</a> link</p>";
            body += $"<p>or use manually this token: {token}</p>";
            return body;
        }
    }
}
