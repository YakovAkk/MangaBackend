using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Services.Base
{
    public interface IAuthService
    {
        Task<TokenViewModel> LoginAsync(UserLoginDTO userDTOLogin);
        Task<TokenViewModel> RefreshToken(RefreshTokenDTO tokenDTO);
        Task<UserViewModel> RegisterAsync(UserRegistrationDTO userDTO);
        Task<bool> ResendVerifyEmailLetter(ResendVerifyEmailLetterInputModel email);
        Task<bool> ResetPasswordAsync(ResetPasswordInputModel inputModel);
        Task<bool> SendResetTokenAsync(SendResetTokenDTO sendResetTokenDTO);
        Task<bool> VerifyEmailAsync(VerifyDTO verifyDTO);
    }
}
