using System.ComponentModel.DataAnnotations;

namespace Services.Model.InputModel
{
    public class SendVerifyEmailLetterInputModel
    {
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        public string Email { get; set; }
    }
}
