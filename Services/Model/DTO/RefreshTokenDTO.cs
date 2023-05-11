namespace Services.Model.DTO
{
    public class RefreshTokenDTO
    {
        public string User_Id { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
