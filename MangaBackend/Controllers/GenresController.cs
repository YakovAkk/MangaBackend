using Data.Helping.Extension;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Base;
using WrapperService.Model.InputModel;
using WrapperService.Model.ResponseModel;
using WrapperService.StatusCode;
using WrapperService.Wrapper;

namespace MangaBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet("favorite")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetfavoriteGentes()
    {
        var result = await _genreService.GetAllFavoriteAsync();

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

    [HttpGet("pagination/{pagesize}/{page}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertainNumber([FromRoute] string pagesize, string page)
    {
        var result = await _genreService.GetCertainPage(pagesize, page);

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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _genreService.GetAllAsync();

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

    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenreById([FromRoute] string Id)
    {
        
        try
        {
            var result = await _genreService.GetByIdAsync(Id);
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

    [HttpPost("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddGenreToFavorite([FromRoute] string Id)
    {
        try
        {
            var result = await _genreService.AddToFavorite(Id);
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

    [HttpPost("filtrarion/{name}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> FiltrarionGenreByName([FromRoute] string name)
    {

        try
        {
            var result = await _genreService.FiltrationByName(name);
            var wrapperResult = WrapperResponseService.Wrap(new WrapInputModel()
            {
                Data = result.ToList()
            });

            if (wrapperResult.StatusCode != CodeStatus.Successful)
            {
                return BadRequest(wrapperResult);
            }

            return Ok(wrapperResult);
        }
        catch (Exception ex)
        {
            var wrapperResult = WrapperResponseService.Wrap(null);
            return NotFound(wrapperResult);
        }
    }

    [HttpDelete("set/favorite/{Id}")]
    [ProducesResponseType(typeof(ResponseWrapModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFavoriteGenreById([FromRoute] string Id)
    {
        try
        {
            var result = await _genreService.RemoveFavorite(Id);
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
}
