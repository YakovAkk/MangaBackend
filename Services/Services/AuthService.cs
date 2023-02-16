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
            var userExist = await _userService.GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

            if (!VerifyPasswordHash(userDTOLogin.NameOrEmail, userExist.PasswordHash, userExist.PasswordSalt))
            {
                throw new Exception("Password is incorrect!");
            }

            var refreshToken = GenereteRefreshToken();
            await _userRespository.SetRefreshToken(refreshToken, userExist);

            var token = new TokensViewModel()
            {
                User_Id = userExist.Id,
                AccessToken = CreateToken(userExist),
                RefreshToken = refreshToken.Token
            };

            return token;
        }
        public async Task<UserViewModel> RegisterAsync(UserRegistrationDTO userDTO)
        {
            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                var errorMessage = "Both of passwords must be equal!";
                throw new Exception(errorMessage);
            }

            if (await _userService.IsUserExists(userDTO))
            {
                var errorMessage = "User already exists!";
                throw new Exception(errorMessage);
            }

            CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userModel = userDTO.toEntity(passwordHash, passwordSalt);

            var result = await _userRespository.CreateAsync(userModel);

            return result.toViewModel();
        }
        public async Task<TokensViewModel> RefreshToken(RefreshTokenDTO tokenDTO)
        {
            var userExist = await _userService.GetByIdAsync(tokenDTO.User_Id);

            if (userExist.RefreshToken != tokenDTO.RefreshToken)
            {
                throw new UnauthorizedAccessException("Invalid refresh token!");
            }
            else if (userExist.TokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token Expired!");
            }

            var token = CreateToken(userExist);
            var newRefreshToken = GenereteRefreshToken();
            await _userRespository.SetRefreshToken(newRefreshToken, userExist);

            return new TokensViewModel()
            {
                User_Id = userExist.Id,
                AccessToken = token,
                RefreshToken = newRefreshToken.Token
            };
        }

        #region Private
        private string CreateToken(UserEntity user)
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }   
        #endregion
    }
}
