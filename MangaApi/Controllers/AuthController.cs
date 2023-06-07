﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.InputModel;
using Services.Services.Base;
using Services.Shared.Constants;
using WrapperService.Model.ResponseModel;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Authorization
        /// <summary>
        /// Return refresh-token in header  
        /// </summary>
        [HttpPost("sign-in")] 
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserLoginInputModel userLoginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(userLoginDTO);
                HttpContext.Response.Headers.Add(Constants.RefreshToken, response.RefreshToken);
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

        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationInputModel user)
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
        /// <summary>
        /// Header params user-id, refresh-token. Return refresh-token in header  
        /// </summary>
        [HttpGet("update-refresh-token"), Authorize]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Request.Headers[Constants.RefreshToken];
            var userId = HttpContext.Request.Headers[Constants.UserId];

            var tokenDTO = new TokenInputModel()
            {
                UserId = userId,
                Token = refreshToken
            };

            try
            {
                var response = await _authService.RefreshToken(tokenDTO);
                HttpContext.Response.Headers.Add(Constants.RefreshToken, response.RefreshToken);
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
        /// <summary>
        /// Header params user-id, token.
        /// </summary>
        [HttpGet("verify-email")]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyEmail()
        {
            try
            {
                var query = HttpContext.Request.Query;
                var verifyDTO = new TokenInputModel() 
                { 
                    UserId = query[Constants.UserId],
                    Token = query[Constants.Token]
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
        public async Task<IActionResult> ResendVerifyEmailLetter([FromBody] SendVerifyEmailLetterInputModel email)
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
        public async Task<IActionResult> SendResetPasswordToken([FromBody] SendVerifyEmailLetterInputModel sendResetTokenDTO)
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputModel inputModel)
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
