namespace Services.DTO;

public class UserDTORegistration
{
    public string Name { get; set; }
    public string DeviceToken { get; set; }

    public override string ToString()
    {
        return $"Name = {Name} DeviceToken = {DeviceToken}";
    }
}
