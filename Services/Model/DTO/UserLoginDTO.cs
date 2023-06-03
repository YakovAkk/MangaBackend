using System.ComponentModel.DataAnnotations;

namespace Services.Model.DTO;

public class UserLoginDTO
{
    public string NameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
