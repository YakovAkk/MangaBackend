using System.ComponentModel.DataAnnotations;

namespace Services.Model.DTO
{
    public class SendResetTokenDTO
    {
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string Email { get; set; }
    }
}
