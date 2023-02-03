namespace Services.Model.DTO;

public class UserRegistrationDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string DeviceToken { get; set; }

    public override string ToString()
    {
        return $"Name = {UserName} DeviceToken = {DeviceToken} " +
            $"Email = {Email} Password = {Password} ConfirmPassword = {ConfirmPassword}";
    }
}
