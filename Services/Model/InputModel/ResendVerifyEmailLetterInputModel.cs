using System.ComponentModel.DataAnnotations;

namespace Services.Model.InputModel
{
    public class ResendVerifyEmailLetterInputModel
    {
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string Email { get; set; }
    }
}
