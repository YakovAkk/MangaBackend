using System.ComponentModel.DataAnnotations;

namespace Services.Model.InputModel;

public class UserRegistrationInputModel
{
    public string Name { get; set; }
    [EmailAddress(ErrorMessage = "Not a valid email address.")]
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string DeviceToken { get; set; }
}
