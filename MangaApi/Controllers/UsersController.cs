using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.DTO;
using Services.Model.InputModel;
using Services.Services.Base;
using System.Net;
using WrapperService.Model.ResponseModel;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    #region User

    [HttpGet, Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpPut]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UserInputModel userInputModel)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(userInputModel);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    #endregion

    #region Favorite

    [HttpPut("favorite/genre/{userid}/{genreid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromRoute] int userid, int genreid)
    {
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(userid, genreid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPut("favorite/manga/{userid}/{mangaid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromRoute] int userid, int mangaid)
    {
        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(userid,mangaid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/genre/{userid}/{genreid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveGenreFromFavorite([FromRoute] int userid, int genreid)
    {
        try
        {
            var result = await _userService.RemoveGenreFromFavoriteAsync(userid, genreid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/manga/{userid}/{mangaid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMangaFromFavorite([FromRoute] int userid, int mangaid)
    {
        try
        {
            var result = await _userService.RemoveMangaFromFavoriteAsync(userid, mangaid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("favorite/genre/{userid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteGenre([FromRoute] int userid)
    {
        var result = await _userService.GetAllFavoriteGenreAsync(userid);

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("favorite/manga/{userid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteManga([FromRoute] int userid)
    {
        var result = await _userService.GetAllFavoriteMangaAsync(userid);

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    #endregion
}
