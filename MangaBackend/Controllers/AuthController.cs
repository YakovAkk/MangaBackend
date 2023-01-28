using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.DTO;
using Services.Services.Base;
using WrapperService.Model.ResponseModel;

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

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
        public Task<IActionResult> Login()
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
        {

            try
            {
                var result = await _userService.CreateAsync(user);
                var wrapperResult = _userWrapper.WrapTheResponseModel(result);
              
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
                
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {

        }
    }
}
