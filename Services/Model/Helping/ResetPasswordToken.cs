namespace Services.Model.Helping
{
    public class ResetPasswordToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
