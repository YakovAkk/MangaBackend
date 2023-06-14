using EmailingService.FileHelper;
using EmailingService.Type;
using MimeKit;
using Services.Shared.Configuration;

namespace EmailingService.Model
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public readonly OthersConfiguration Configuration;
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string data, EmailType emailType, OthersConfiguration configuration)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Configuration = configuration;
            Content = GetEmailContent(data, emailType);
        }

        private string GetEmailContent(string data, EmailType EmailType)
        {
            FileWorker fileWorker = new FileWorker(Configuration);

            var content = string.Empty;

            switch (EmailType)
            {
                case EmailType.ConfirmationEmail:
                    content = fileWorker.GetConfirmationEmailHTMLFile(data);
                    break;
                case EmailType.ResetPasswordTokenEmail:
                    content = fileWorker.GetResetPasswordTokenEmailHTMLFile(data);
                    break;
                default:
                    break;
            }

            return content;
        }
    }
}
