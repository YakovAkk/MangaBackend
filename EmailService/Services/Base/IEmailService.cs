using EmailingService.Model;

namespace EmailingService.Services.Base
{
    public interface IEmailService
    {
        public void SendEmail(Message message);
    }
}
