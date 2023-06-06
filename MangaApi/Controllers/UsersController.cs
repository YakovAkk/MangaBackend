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

    [HttpPut("favorite/genre/{genreid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromRoute] int genreid)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(userId, genreid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpPut("favorite/manga{mangaid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromRoute] int mangaid)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(userId,mangaid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/genre/{genreid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveGenreFromFavorite([FromRoute] int genreid)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        try
        {
            var result = await _userService.RemoveGenreFromFavoriteAsync(userId, genreid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/manga/{mangaid}")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMangaFromFavorite([FromRoute] int mangaid)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        try
        {
            var result = await _userService.RemoveMangaFromFavoriteAsync(userId, mangaid);
            var wrapperResult = WrapperResponseService.Wrap<object>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<object>(errorMessage: ex.Message);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("favorite/genre")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteGenre()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var result = await _userService.GetAllFavoriteGenresAsync(userId);

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("favorite/manga")]
    [ProducesResponseType(typeof(WrapViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteManga()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var result = await _userService.GetAllFavoriteMangasAsync(userId);

        var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);

        if (wrapperResult.StatusCode != HttpStatusCode.OK)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    #endregion

    #region RememberReading
    [HttpGet("remember-reading")]
    public async Task<IActionResult> GetAllReadingItems()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

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
            return Unauthorized();

        try
        {
            await _userService.CreateReadingItemAsync(userId, inputModel);
            var wrapperResult = WrapperResponseService.Wrap<object>(true);
            return Ok(wrapperResult);
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
            return Unauthorized();

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
    #endregion

    #region Recommendation
    [HttpGet("recommendation")]
    public async Task<IActionResult> GetRecommendationsItems()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        try
        {
            var result = await _userService.GetRecommendationsAsync(userId);
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(result);
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap<IEnumerable<object>>(errorMessage: ex.Message);
            return BadRequest(wrapperResult);
        }
    }
    #endregion
}
