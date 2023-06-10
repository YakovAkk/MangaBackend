using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Services.Base
{
    public interface IAuthService
    {
        Task<TokenViewModel> LoginAsync(UserLoginInputModel userDTOLogin);
        Task<TokenViewModel> RefreshToken(TokenInputModel tokenDTO);
        Task<UserViewModel> RegisterAsync(UserRegistrationInputModel userDTO);
        Task<bool> ResendVerifyEmailLetter(SendVerifyEmailLetterInputModel email);
        Task<bool> ResetPasswordAsync(ResetPasswordInputModel inputModel);
        Task<bool> SendResetTokenAsync(SendVerifyEmailLetterInputModel sendResetTokenDTO);
        Task<bool> VerifyEmailAsync(TokenInputModel verifyDTO);
    }
}
