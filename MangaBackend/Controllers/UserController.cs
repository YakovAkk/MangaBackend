using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo;
using MySqlX.XDevAPI.Common;
using Repositories.LogsTools;
using Repositories.LogsTools.Base;
using Services.DTO;
using Services.Response;
using Services.Services;
using Services.Services.Base;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace MangaBackend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IWrapperUserService _userWrapper;
    private readonly IWrapperGenreService _genreWrapper;
    private readonly IWrapperMangaService _mangaWrapper;
    private readonly ILogger<GenresController> _logger;
    private readonly IUserService _userService;
    private readonly ITool _logTool;

    public UserController(IWrapperUserService userWrapper, IWrapperGenreService genreWrapper, IWrapperMangaService mangaWrapper, ILogger<GenresController> logger, IUserService userService, ITool logTool)
    {
        _userWrapper = userWrapper;
        _genreWrapper = genreWrapper;
        _mangaWrapper = mangaWrapper;
        _logger = logger;
        _userService = userService;
        _logTool = logTool;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logTool.NameOfMethod = nameof(GetAll);

        _logTool.WriteToLog(_logger, LogPosition.Begin);
        var result = await _userService.GetAllAsync();

        var wrapperResult = _userWrapper.WrapTheResponseListOfModels(result);

        _logTool.WriteToLog(_logger, LogPosition.End,
            $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpPost("registration")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Registration([FromBody] UserDTORegistration user)
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
            _logTool.WriteToLog(_logger, LogPosition.End, $"Status Code = {(int)wrapperResult.StatusCode}  result = {wrapperResult}");
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserDTOLogin user)
    {
        try
        {
            var result = await _userService.LoginAsync(user);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
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
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("favorite/genre")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("favorite/manga")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/genre/{userid}/{genreid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveGenreFromFavorite([FromRoute] string userid, string genreid)
    {
        try
        {
            var result = await _userService.RemoveGenreFromFavoriteAsync(userid, genreid);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/manga/{userid}/{mangaid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMangaFromFavorite([FromRoute] string userid, string mangaid)
    {
        try
        {
            var result = await _userService.RemoveMangaFromFavoriteAsync(userid, mangaid);
            var wrapperResult = _userWrapper.WrapTheResponseModel(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = _userWrapper.WrapTheResponseModel(null, ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("favorite/genre/{userid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteGenre([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteGenreAsync(userid);

        var wrapperResult = _genreWrapper.WrapTheResponseListOfModels(result);

        _logTool.WriteToLog(_logger, LogPosition.End,
            $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("favorite/manga/{userid}")]
    [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteManga([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteMangaAsync(userid);

        var wrapperResult = _mangaWrapper.WrapTheResponseListOfModels(result);

        _logTool.WriteToLog(_logger, LogPosition.End,
            $"Status Code = {(int)wrapperResult.StatusCode} {wrapperResult}");

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }
}
