using Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Model.ViewModel;
using Services.Services.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserRespository _userRespository;
        public readonly IConfiguration _configuration;
        public AuthService(IUserService userService, IUserRespository userRespository, IConfiguration configuration)
        {
            _userService = userService;
            _userRespository = userRespository;
            _configuration = configuration;
        }

        public async Task<TokensViewModel> LoginAsync(UserLoginDTO userDTOLogin)
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

                if(userDTOLogin.Password != userExist.Password) 
                {
                    throw new Exception("Password is incorrect!");
                }

                var token = new TokensViewModel();
                token.AccessToken = CreateTocken(userExist);

                return token;
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

        #region Private
        private string CreateTocken(UserEntity user)
        {
            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("TokenSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        #endregion
    }
}
