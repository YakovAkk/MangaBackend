using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.InputModel;
using Services.Services.Base;
using System.Net;
using System.Security.Claims;
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
        var result = await _userService.GetAllFavoriteGenresAsync(userid);

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
        var result = await _userService.GetAllFavoriteMangasAsync(userid);

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    #endregion

    [HttpGet("remember-reading")]
    public async Task<IActionResult> GetAllReadingItems()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return BadRequest();

        try
        {
            var result = await _userService.GetAllReadingItemsAsync(userId);
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(errorMessage: ex.Message);
            return BadRequest(wrapperResult);
        }
    }

    [HttpPost("remember-reading")]
    public async Task<IActionResult> CreateReadingItem([FromBody] RememberReadingItemInputModel inputModel)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return BadRequest();

        try
        {
            await _userService.CreateReadingItemAsync(userId, inputModel);
            return Ok();
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(errorMessage: ex.Message);
            return BadRequest(wrapperResult);
        }
    }

    [HttpGet("remember-reading/{mangaId}")]
    public async Task<IActionResult> GetReadingItem([FromRoute] string mangaId)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return BadRequest();

        try
        {
            var result = await _userService.GetReadingItemAsync(userId, mangaId);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return BadRequest(wrapperResult);
        }
    }
}
