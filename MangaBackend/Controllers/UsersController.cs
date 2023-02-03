using Data.Entities;
using Data.Helping.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Model.DTO;
using Services.Services.Base;
using WrapperService.Model.InputModel;
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
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result,
        });

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UserEntity userEntity)
    {
        try
        {
            var result = await _userService.UpdateAsync(userEntity);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList(),
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    #endregion

    #region Favorite

    [HttpPost("favorite/genre")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddGenreToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList()
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpPost("favorite/manga")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddMangaToFavorite([FromBody] FavoriteDTO addTOFavoriteDTO)
    {
        try
        {
            var result = await _userService.AddMangaToFavoriteAsync(addTOFavoriteDTO);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList()
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/genre/{userid}/{genreid}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveGenreFromFavorite([FromRoute] string userid, string genreid)
    {
        try
        {
            var result = await _userService.RemoveGenreFromFavoriteAsync(userid, genreid);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList()
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("favorite/manga/{userid}/{mangaid}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMangaFromFavorite([FromRoute] string userid, string mangaid)
    {
        try
        {
            var result = await _userService.RemoveMangaFromFavoriteAsync(userid, mangaid);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList()
            });
            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpGet("favorite/genre/{userid}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteGenre([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteGenreAsync(userid);

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result.ToList()
        });

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    [HttpGet("favorite/manga/{userid}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFavoriteManga([FromRoute] string userid)
    {
        var result = await _userService.GetAllFavoriteMangaAsync(userid);

        var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
        {
            Data = result.ToList()
        });

        if (wrapperResult.StatusCode != CodeStatus.Successful)
        {
            return NotFound(wrapperResult);
        }

        return Ok(wrapperResult);
    }

    #endregion
}
