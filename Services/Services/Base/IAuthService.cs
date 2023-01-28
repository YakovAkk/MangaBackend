using Data.Entities;
using Services.DTO;

namespace Services.Services.Base
{
    public interface IAuthService
    {
        Task<UserEntity> LoginAsync(UserLoginDTO userDTOLogin);
        Task<UserEntity> RegisterAsync(UserRegistrationDTO userDTO);
    }
}
