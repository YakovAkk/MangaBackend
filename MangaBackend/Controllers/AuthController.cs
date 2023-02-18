﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.DTO;
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

        [HttpPost("login")]
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

        [HttpPost("registration")]
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

        [HttpGet("refresh-token/{userId}"), Authorize]
        [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromRoute] string userId)
        {
            var refreshToken = HttpContext.Request.Headers["refresh-token"];

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
    }
}