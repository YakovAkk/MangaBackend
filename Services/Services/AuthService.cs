using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using EmailingService.Model;
using EmailingService.Services.Base;
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
        private static readonly Random random = new Random();

        private readonly IUserService _userService;
        private readonly IUserRespository _userRespository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthService(IUserService userService, IUserRespository userRespository, IConfiguration configuration, IEmailService emailService)
        {
            _userService = userService;
            _userRespository = userRespository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<bool> SendResetTokenAsync(SendResetTokenDTO sendResetTokenDTO)
        {
            var userExist = await _userService.GetUserByNameOrEmail(sendResetTokenDTO.Email);

            if(string.IsNullOrEmpty(userExist.ResetPasswordToken))
            {
                var resetPasswordToken = CreateResetPasswordToken();
                await _userRespository.SetResetPasswordToken(resetPasswordToken, userExist);
            }

            var message = new Message(new string[] { userExist.Email }, "Manga APP",
                 $"Token : {userExist.ResetPasswordToken}");

            _emailService.SendEmail(message);

            return true;
        }

        public async Task<TokensViewModel> LoginAsync(UserLoginDTO userDTOLogin)
        {
            var userExist = await _userService.GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

            if (!VerifyPasswordHash(userDTOLogin.Password, userExist.PasswordHash, userExist.PasswordSalt))
            {
                throw new Exception("Password is incorrect!");
            }

            if(userExist.VerifiedAt == null)
            {
                throw new Exception("Please, verify you email!");  
            }

            var refreshToken = CreateRefreshToken();
            await _userRespository.SetRefreshToken(refreshToken, userExist);

            var token = new TokensViewModel()
            {
                User_Id = userExist.Id,
                AccessToken = CreateAccessToken(userExist),
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

            var verificationToken = CreateRandomToken();

            var userModel = userDTO.toEntity(passwordHash, passwordSalt, verificationToken);

            var result = await _userRespository.CreateAsync(userModel);

            var message = new Message(new string[] { result.Email }, "Manga APP",
                $"https://localhost:5000/api/Auth/verify-email?userId={result.Id}&token={result.VerificationToken}");

            _emailService.SendEmail(message);

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

            var token = CreateAccessToken(userExist);
            var newRefreshToken = CreateRefreshToken();
            await _userRespository.SetRefreshToken(newRefreshToken, userExist);

            return new TokensViewModel()
            {
                User_Id = userExist.Id,
                AccessToken = token,
                RefreshToken = newRefreshToken.Token
            };
        }
        public async Task<bool> VerifyEmailAsync(VerifyDTO verifyDTO)
        {
            var user = await _userService.GetByIdAsync(verifyDTO.UserID);

            if(user.VerificationToken != verifyDTO.Token)
            {
                throw new UnauthorizedAccessException("Token isn't correct!");
            }

            await _userRespository.VerifyAsync(user);

            return true;
        }
        public async Task<bool> VerifyResetPasswordToken(VerifyResetPasswordTokenDTO tokenDTO)
        {
            var user = await _userService.GetUserByNameOrEmail(tokenDTO.Email);

            if (user.ResetPasswordToken != tokenDTO.Token)
            {
                throw new UnauthorizedAccessException("Token isn't correct!");
            }

            if (user.ResetPasswordTokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token Expired!");
            }

            var resetPasswordToken = CreateResetPasswordToken();
            await _userRespository.SetResetPasswordToken(resetPasswordToken, user);

            return true;
        }

        #region Private
        private ResetPasswordToken CreateResetPasswordToken()
        {
            return new ResetPasswordToken()
            {
                Expires = DateTime.Now.AddDays(1),
                Token = random.Next(10000, 100000).ToString()
            };
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private string CreateAccessToken(UserEntity user)
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
        private RefreshToken CreateRefreshToken()
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
