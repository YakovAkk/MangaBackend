using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using MySqlX.XDevAPI.Common;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.DTO;
using Services.Response;
using Services.Services.Base;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace MangaBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IWrapperUserService _wrapper;
    private readonly ILogger<GenresController> _logger;
    private readonly IUserService _userService;
    private readonly ITool _logTool;

    public UserController(IWrapperUserService wrapper, ILogger<GenresController> logger, IUserService userService, ITool logTool)
    {
        _wrapper = wrapper;
        _logger = logger;
        _userService = userService;
        _logTool = logTool;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Registration([FromBody] UserDTORegistration user)
    {
        _logTool.NameOfMethod = nameof(Registration);

        _logTool.WriteToLog(_logger, LogPosition.Begin, $"UserDTORegistration = {user}");

        try
        {
            var result = await _userService.CreateAsync(user);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            _logTool.WriteToLog(_logger, LogPosition.End, 
                $"Status Code = {(int)wrapperResult.StatusCode} result = {wrapperResult}");
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode}  result = {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserDTOLogin user)
    {
        try
        {
            var result = await _userService.LoginAsync(user);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UserEntity userEntity)
    {
        try
        {
            var result = await _userService.UpdateAsync(userEntity);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFaforite([FromBody] string user_Id, string genre_Id)
    {
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(user_Id, genre_Id);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFaforite([FromBody] string user_Id, string manga_Id)
    {
        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(user_Id, manga_Id);
            var wrapperResult = _wrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _wrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }
}
