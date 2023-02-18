namespace EmailingService.Services.Base
{
    public interface IEmailService
    {
        public Task SendEmail(string body);
    }
}
