using Data.Helping.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.DTO;
using Services.Services.Base;
using WrapperService.Model.InputModel;
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
        [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var response = await _authService.LoginAsync(userLoginDTO);
                var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
                {
                    Data = new object[] {response}
                });

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap(null);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("registration")]
        [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
        {
            try
            {
                var result = await _authService.RegisterAsync(user);
                var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
                {
                    Data = result.ToList(),
                });

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap(null);
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("refresh-token"), Authorize]
        [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO tokenDTO)
        {
            try
            {
                var response = await _authService.RefreshToken(tokenDTO);
                var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
                {
                    Data = new object[] { response }
                });

                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = WrapperResponseService.Wrap(null);
                return BadRequest(wrapperResult);
            }
        }
    }
}
