namespace EmailingService.Model
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string StmpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
