namespace Services.DTO;

public class UserLoginDTO
{
    public string NameOrEmail { get; set; }
    public string Password { get; set; }
    public override string ToString()
    {
        return $"Name = {NameOrEmail} Password = {Password}";
    }
}
