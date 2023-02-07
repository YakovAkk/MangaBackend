namespace Services.Model.DTO;

public class UserLoginDTO
{
    public string NameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public override string ToString()
    {
        return $"Name = {NameOrEmail} Password = {Password}";
    }
}
