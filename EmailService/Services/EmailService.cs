using EmailingService.Services.Base;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailingService.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("rosemary.powlowski@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("rosemary.powlowski@ethereal.email"));
            email.Subject = "Test email";
            email.Body = new TextPart(TextFormat.Html) {Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 4, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("rosemary.powlowski@ethereal.email", "FkAJr11Uu7MWqEEgfF");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}
