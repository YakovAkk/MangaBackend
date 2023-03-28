using EmailingService.FileHelper;
using EmailingService.Type;
using MimeKit;

namespace EmailingService.Model
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string data, EmailType emailType)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("email", x)));
            Subject = subject;
            Content = GetEmailContent(data, emailType);
        }

        private string GetEmailContent(string data, EmailType EmailType)
        {
            FileWorker fileWorker = new FileWorker();

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
