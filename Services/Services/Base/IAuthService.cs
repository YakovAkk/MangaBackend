using Data.Entities;
using Services.Model.DTO;
using Services.Model.ViewModel;

namespace Services.Services.Base
{
    public interface IAuthService
    {
        Task<TokensViewModel> LoginAsync(UserLoginDTO userDTOLogin);
        Task<TokensViewModel> RefreshToken(RefreshTokenDTO tokenDTO);
        Task<UserViewModel> RegisterAsync(UserRegistrationDTO userDTO);
    }
}
