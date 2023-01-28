using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService )
        {
            _userService = userService;
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

        //[HttpPost("registration")]
        //[ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
        //{

        //    try
        //    {
        //        var result = await _userService.CreateAsync(user);
        //        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        //        {
        //            Data = result.ToList(),
        //        });

        //        return Ok(wrapperResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        var wrapperResult = WrapperResponseService.Wrap(null);
        //        return BadRequest(wrapperResult);
        //    }
        //}

        //[HttpPost("refresh-token")]
        //[ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        //public async Task<IActionResult> RefreshToken()
        //{

        //}
    }
}
