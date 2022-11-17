namespace Services.DTO;

public class UserDTOLogin
{
    public string Name { get; set; }

    public override string ToString()
    {
        return $"Name = {Name}";
    }
}
