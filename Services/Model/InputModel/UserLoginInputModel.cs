namespace Services.Model.InputModel;

public class UserLoginInputModel
{
    public string NameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
