using MailKit.Net.Smtp;
using MimeKit;

namespace JwtApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                Console.WriteLine("EMAIL START");

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart("plain") { Text = message };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSettings:Username"],
                    _config["EmailSettings:Password"]
                );

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine("EMAIL SENT SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                Console.WriteLine("EMAIL ERROR:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
