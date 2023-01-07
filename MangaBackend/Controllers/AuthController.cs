using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.DTO;
using Services.Response;
using Services.Services.Base;
using Services.Wrappers.Base;

namespace MangaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWrapperUserService _userWrapper;
        private readonly ILogger<AuthController> _logger;
        private readonly ITool _logTool;

        public AuthController(IUserService userService, IWrapperUserService userWrapper,
            ILogger<AuthController> logger, ITool logTool)
        {
            _userService = userService;
            _userWrapper = userWrapper;
            _logger = logger;
            _logTool = logTool;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        public Task<IActionResult> Login()
        {
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest()
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistrationDTO user)
        {
            _logTool.NameOfMethod = nameof(Registration);

            _logTool.WriteToLog(_logger, LogPosition.Begin, $"UserDTORegistration = {user}");

            try
            {
                var result = await _userService.CreateAsync(user);
                var wrapperResult = _userWrapper.WrapTheResponseModel(result);
                _logTool.WriteToLog(_logger, LogPosition.End,
                    $"Status Code = {(int)wrapperResult.StatusCode} result = {wrapperResult}");
                return Ok(wrapperResult);
            }
            catch (Exception ex)
            {
                var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
                _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode} result = {wrapperResult}");
                return BadRequest(wrapperResult);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {

        }
    }
}
