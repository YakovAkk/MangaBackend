using Data.Helping.Extension;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
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

        //[HttpPost("login")]
        //[ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        //public Task<IActionResult> Login()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}

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

        //[HttpPost("refresh-token")]
        //[ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> RefreshToken()
        //{

        //}
    }
}
