using Data.Entities;
using Data.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.Base;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Model.ViewModel;
using Services.Services.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

            var userExist = await _userService.GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

            if (userDTOLogin.Password != userExist.Password)
            {
                throw new Exception("Password is incorrect!");
            }

            var refreshToken = GenereteRefreshToken();
            await _userRespository.SetRefreshToken(refreshToken, userExist);

            var token = new TokensViewModel()
            {
                AccessToken = CreateTocken(userExist),
                RefreshToken = refreshToken.Token
            };

            return token;
        }
        public async Task<UserViewModel> RegisterAsync(UserRegistrationDTO userDTO)
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

            var result = await _userRespository.CreateAsync(userModel);

            return result.toViewModel();
        }
        public async Task<TokensViewModel> RefreshToken(RefreshTokenDTO tokenDTO)
        {
            if (tokenDTO == null)
            {
                var errorMessage = "User is null";

                throw new ArgumentNullException(errorMessage);
            }

            if (string.IsNullOrEmpty(tokenDTO.RefreshToken) || string.IsNullOrEmpty(tokenDTO.User_NameOrEmail))
            {
                var errorMessage = "RefreshToken Or User_id field is null or empty";

                throw new ArgumentNullException(errorMessage);
            }
            var userExist = await _userService.GetUserByNameOrEmail(tokenDTO.User_NameOrEmail);

            if (userExist.RefreshToken != tokenDTO.RefreshToken)
            {
                throw new UnauthorizedAccessException("Invalid refresh token!");
            }
            else if (userExist.TokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token Expired!");
            }

            var token = CreateTocken(userExist);
            var newRefreshToken = GenereteRefreshToken();
            await _userRespository.SetRefreshToken(newRefreshToken, userExist);

            return new TokensViewModel()
            {
                AccessToken = token,
                RefreshToken = newRefreshToken.Token
            };
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
        private RefreshToken GenereteRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }
        #endregion
    }
}
