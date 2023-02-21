namespace Data.Helping.Model
{
    public class ResetPasswordToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
