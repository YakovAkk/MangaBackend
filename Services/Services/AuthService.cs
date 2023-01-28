using Data.Entities;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserRespository _userRespository;
        public AuthService(IUserService userService, IUserRespository userRespository)
        {
            _userService = userService;
            _userRespository = userRespository;
        }

        public async Task<UserEntity> LoginAsync(UserLoginDTO userDTOLogin)
        {
            if (userDTOLogin == null)
            {
                var errorMessage = "User is null";

                throw new ArgumentNullException(errorMessage);
            }

            if (string.IsNullOrEmpty(userDTOLogin.NameOrEmail))
            {
                var errorMessage = "Name Or Email field is null or empty";

                throw new ArgumentNullException(errorMessage);
            }

            try
            {
                var userExist = await _userService.GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

                return userExist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<UserEntity> RegisterAsync(UserRegistrationDTO userDTO)
        {

            if (userDTO == null)
            {
                var errorMessage = "User is null";
                throw new ArgumentNullException(errorMessage);
            }

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                var errorMessage = "Both of passwords must be equal!";
                throw new Exception(errorMessage);
            }

            var userModel = userDTO.toEntity();

            try
            {
                var res = await _userRespository.CreateAsync(userModel);
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
