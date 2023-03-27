using Data.Database;
using Data.Entities;
using Data.Helping.Model;
using Data.Model.ViewModel;
using EmailingService.Model;
using EmailingService.Services.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.ExtensionMapper;
using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Model.ViewModel;
using Services.Services.Base;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public class AuthService : DbService<AppDBContext>, IAuthService
    {
        private static readonly Random random = new Random();

        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthService(
            IUserService userService, 
            IConfiguration configuration, 
            IEmailService emailService,
            DbContextOptions<AppDBContext> dbContextOptions) : base(dbContextOptions)
        {
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<bool> SendResetTokenAsync(SendResetTokenDTO sendResetTokenDTO)
        {
            var user = await _userService.GetUserByNameOrEmail(sendResetTokenDTO.Email);

            if(user == null)
            {
                throw new Exception("User doesn't exist!");
            }

            if(string.IsNullOrEmpty(user.ResetPasswordToken))
            {
                var resetPasswordToken = CreateResetPasswordToken();
                await _userService.SetResetPasswordToken(resetPasswordToken, user);
            }

            var message = new Message(new string[] { user.Email }, "Manga APP",
                 $"Token : {user.ResetPasswordToken}");

            _emailService.SendEmail(message);

            return true;
        }
        public async Task<TokensViewModel> LoginAsync(UserLoginDTO userDTOLogin)
        {
            var user = await _userService.GetUserByNameOrEmail(userDTOLogin.NameOrEmail);

            if (user == null)
            {
                throw new Exception("User doesn't exist!");
            }

            if (!VerifyPasswordHash(userDTOLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Password is incorrect!");
            }

            if(user.VerifiedAt == null)
            {
                throw new Exception("Please, verify you email!");  
            }

            var refreshToken = CreateRefreshToken();
            await _userService.SetRefreshToken(refreshToken, user);

            var token = new TokensViewModel()
            {
                User_Id = user.Id,
                AccessToken = CreateAccessToken(user),
                RefreshToken = refreshToken.Token
            };

            return token;
        }
        public async Task<UserViewModel> RegisterAsync(UserRegistrationDTO userDTO)
        {
            if (await _userService.IsUserExists(userDTO.Email, userDTO.Name))
            {
                throw new Exception("User is already registered!");
            }

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                var errorMessage = "Both of passwords must be equal!";
                throw new Exception(errorMessage);
            }

            CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var verificationToken = CreateRandomToken();

            var userModel = userDTO.toEntity(passwordHash, passwordSalt, verificationToken);

            var user = await _userService.CreateAsync(userModel);

            var message = CreateVerifyEmailTemplate(user);
            _emailService.SendEmail(message);

            return user.toViewModel();
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
            await _userService.SetRefreshToken(newRefreshToken, userExist);

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

            await _userService.VerifyAsync(user);

            return true;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordInputModel inputModel)
        {
            var user = await _userService.GetUserByNameOrEmail(inputModel.Email);

            if (user == null)
            {
                throw new Exception("User doesn't exist!");
            }

            VerifyResetPasswordTokenAsync(user, inputModel.Token);

            if (inputModel.Password != inputModel.ConfirmPassword)
            {
                var errorMessage = "Both of passwords must be equal!";
                throw new Exception(errorMessage);
            }

            CreatePasswordHash(inputModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var resetPasswordToken = CreateResetPasswordToken();
            await _userService.SetResetPasswordToken(resetPasswordToken, user);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            using (var dbContext = CreateDbContext())
            {
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync(); 
            }

            return true;
        }
        public async Task<bool> ResendVerifyEmailLetter(ResendVerifyEmailLetterInputModel InputModel)
        {
            var user = await _userService.GetUserByNameOrEmail(InputModel.Email);

            if (user == null)
            {
                throw new Exception("User doesn't exist!");
            }

            var message = CreateVerifyEmailTemplate(user);
            _emailService.SendEmail(message);

            return true;
        }

        #region Private
        private bool VerifyResetPasswordTokenAsync(UserEntity user, string token)
        {
            if (user.ResetPasswordToken != token)
            {
                throw new UnauthorizedAccessException("Token isn't correct!");
            }

            if (user.ResetPasswordTokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Token Expired!");
            }

            return true;
        }
        private Message CreateVerifyEmailTemplate(UserEntity user)
        {
            var applicationURL = _configuration.GetSection("ApplicationSettings:url").Value;
            return new Message(new string[] { user.Email }, "Manga APP",
                $"{applicationURL}/api/Auth/verify-email?userId={user.Id}&token={user.VerificationToken}");
        }
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
