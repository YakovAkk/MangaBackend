using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Services.Base;
using WrapperService.Model.ResponseModel;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService )
        {
            _authService = authService;
        }


        #region Authorization
        [HttpPost("sign in")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(userLoginDTO);
                HttpContext.Response.Headers.Add("refresh-token", response.RefreshToken);
                var wrapperResult = WrapperResponseService.Wrap<object>(
                    new { 
                        User_Id  = response.User_Id,
                        AccessToken = response.AccessToken,
                    });
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("sign up")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
        {
            try
            {
                var result = await _authService.RegisterAsync(user);
                var wrapperResult = WrapperResponseService.Wrap<object>(result);

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }
        #endregion

        #region Refresh and send
        [HttpGet("update-refresh-token"), Authorize]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Headers["refresh-token"];
            var userId = HttpContext.Request.Headers["userId"];

            var tokenDTO = new RefreshTokenDTO()
            {
                User_Id = userId,
                RefreshToken = refreshToken
            };

            try
            {
                var response = await _authService.RefreshToken(tokenDTO);
                HttpContext.Response.Headers.Add("refresh-token", response.RefreshToken);
                var wrapperResult = WrapperResponseService.Wrap<object>(
                    new
                    {
                        User_Id = response.User_Id,
                        AccessToken = response.AccessToken,
                    });

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }

        [HttpGet("verify-email")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Verify()
        {
            try
            {
                var query = HttpContext.Request.Query;
                var verifyDTO = new VerifyDTO() 
                { 
                    UserID = query["userId"],
                    Token = query["token"]
                };
                var response = await _authService.VerifyEmailAsync(verifyDTO);
                var wrapperResult = WrapperResponseService.Wrap<object>(response);

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("resend-verify-email-letter")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendVerifyEmailLetter(ResendVerifyEmailLetterInputModel email)
        {
            try
            {
                var response = await _authService.ResendVerifyEmailLetter(email);
                var wrapperResult = WrapperResponseService.Wrap<object>(response);

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("send-reset-password-token")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendResetPasswordToken(SendResetTokenDTO sendResetTokenDTO)
        {
            try
            {
                var response = await _authService.SendResetTokenAsync(sendResetTokenDTO);
                var wrapperResult = WrapperResponseService.Wrap<object>(response);

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordInputModel inputModel)
        {
            try
            {
                var response = await _authService.ResetPasswordAsync(inputModel);
                var wrapperResult = WrapperResponseService.Wrap<object>(response);

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
                return BadRequest(wrapperResult);
            }
        }


        #endregion
    }
}
