using System.ComponentModel.DataAnnotations;

namespace Services.Model.DTO
{
    public class VerifyResetPasswordTokenDTO
    {

        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
